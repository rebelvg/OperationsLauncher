using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;

namespace MurshunLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            try
            {
                if (Process.GetProcessesByName("MurshunLauncher").Length > 1)
                {
                    MessageBox.Show("Launcher is already running.");
                    System.Environment.Exit(1);
                }

                string iniDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MurshunLauncher";

                xmlPath_textBox.Text = iniDirectoryPath + "\\MurshunLauncher.xml";

                if (!Directory.Exists(iniDirectoryPath))
                {
                    try
                    {
                        Directory.CreateDirectory(iniDirectoryPath);
                    }
                    catch
                    {
                        MessageBox.Show("Couldn't create a folder at " + iniDirectoryPath);
                    }
                }

                if (File.Exists(xmlPath_textBox.Text))
                {
                    ReadXmlFile();
                }
                else
                {
                    try
                    {
                        LauncherSettings = new MurshunLauncherXmlSettings();

                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MurshunLauncherXmlSettings));

                        System.IO.FileStream writer = System.IO.File.Create(xmlPath_textBox.Text);
                        serializer.Serialize(writer, LauncherSettings);
                        writer.Close();

                        ReadXmlFile();
                    }
                    catch
                    {
                        MessageBox.Show("Saving xml settings failed.");
                    }
                }

                label3.Text = "Version " + launcherVersion;
            }
            catch (Exception e)
            {
                MessageBox.Show("Launcher crashed while initializing. Try running it as administrator.\n\n" + e);
                System.Environment.Exit(1);
            }
        }

        public class MurshunLauncherXmlSettings
        {
            public string pathToArma3Client_textBox = Directory.GetCurrentDirectory() + "\\arma3.exe";
            public string pathToArma3ClientMods_textBox = Directory.GetCurrentDirectory();
            public bool joinTheServer_checkBox;
            public List<string> clientCustomMods_listView;
            public List<string> clientCheckedModsList_listView;
            public string defaultStartLine_textBox = "-world=empty -nosplash -skipintro -nofilepatching -nologs";
            public string advancedStartLine_textBox;
            public bool showServerTabs_checkBox;

            public string pathToArma3Server_textBox = Directory.GetCurrentDirectory() + "\\arma3server.exe";
            public string pathToArma3ServerMods_textBox = Directory.GetCurrentDirectory();
            public List<string> serverCustomMods_listView;
            public List<string> serverCheckedModsList_listView;
            public string serverConfig_textBox;
            public string serverCfg_textBox;
            public string serverProfiles_textBox;
            public string serverProfileName_textBox;
            public bool hideWindow_checkBox;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VerifyMods(false);
        }

        private void launch_button_Click(object sender, EventArgs e)
        {
            ReadPresetFile();

            bool verifySuccess = VerifyMods(false);

            if (!verifySuccess)
            {
                DialogResult dialogResult = MessageBox.Show("Launch the client anyway?", "", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    tabControl1.SelectedTab = tabPage1;
                }
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            string modLine;

            modLine = defaultStartLine_textBox.Text;

            if (advancedStartLine_textBox.Text != "")
            {
                modLine = modLine + " " + advancedStartLine_textBox.Text;
            }

            modLine = modLine + " \"-mod=";

            foreach (ListViewItem X in clientPresetMods_listView.Items)
            {
                modLine = modLine + pathToArma3ClientMods_textBox.Text + "\\" + X.Text + ";";
            }

            foreach (ListViewItem X in clientCustomMods_listView.CheckedItems)
            {
                modLine = modLine + X.Text + ";";
            }

            modLine = modLine + "\"";

            if (joinTheServer_checkBox.Checked)
            {
                modLine = modLine + " -connect=109.87.76.153 -port=2302 -password=v";
            }

            if (File.Exists(pathToArma3Client_textBox.Text))
            {
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = pathToArma3Client_textBox.Text;
                myProcess.StartInfo.Arguments = modLine;
                myProcess.Start();
            }
            else
            {
                MessageBox.Show("arma3.exe not found.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.Description = "Select custom mod folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                clientCustomMods_listView.Items.Add(chosenFolder.SelectedPath);

                ReadPresetFile();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://murshun.club/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/groups/murshun");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            defaultStartLine_textBox.Text = "-world=empty -nosplash -skipintro -nofilepatching -nologs";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            GetWebModLineNewThread();

            bool compareSuccess = CompareFolders();
            bool copySuccess = CopyMissions();

            if (!compareSuccess || !copySuccess)
            {
                DialogResult dialogResult = MessageBox.Show("Launch the server anyway?", "", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    tabControl1.SelectedTab = tabPage3;
                }
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            string modLine;

            modLine = defaultStartLineServer_textBox.Text;

            modLine = modLine + " \"-config=" + serverConfig_textBox.Text + "\"";

            modLine = modLine + " \"-cfg=" + serverCfg_textBox.Text + "\"";

            modLine = modLine + " \"-profiles=" + serverProfiles_textBox.Text + "\"";

            modLine = modLine + " -name=" + serverProfileName_textBox.Text;

            modLine = modLine + " \"-mod=";

            foreach (ListViewItem X in serverPresetMods_listView.Items)
            {
                modLine = modLine + pathToArma3ServerMods_textBox.Text + "\\" + X.Text + ";";
            }

            modLine = modLine + "\"";

            modLine = modLine + " \"-servermod=";

            foreach (ListViewItem X in serverCustomMods_listView.CheckedItems)
            {
                modLine = modLine + X.Text + ";";
            }

            modLine = modLine + "\"";

            if (File.Exists(pathToArma3Server_textBox.Text))
            {
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = pathToArma3Server_textBox.Text;
                myProcess.StartInfo.Arguments = modLine;
                if (hideWindow_checkBox.Checked)
                    myProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                myProcess.Start();
                myProcess.ProcessorAffinity = (System.IntPtr)12;
                myProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            else
            {
                MessageBox.Show("arma3server.exe not found.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CompareFolders();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sync " + (compareExcessFiles_listView.Items.Count + compareMissingFiles_listView.Items.Count) + " files?", "", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                //MessageBox.Show("Removing " + compareExcessFiles_listView.Items.Count + " files.");

                foreach (ListViewItem item in compareExcessFiles_listView.Items)
                {
                    File.Delete(pathToArma3ServerMods_textBox.Text + item.Text.Split(':')[0]);
                }

                //MessageBox.Show("Copying " + compareMissingFiles_listView.Items.Count + " files.");

                foreach (ListViewItem item in compareMissingFiles_listView.Items)
                {
                    CheckPath(pathToArma3ServerMods_textBox.Text + item.Text.Split(':')[0]);

                    File.Copy(pathToArma3ClientMods_textBox.Text + item.Text.Split(':')[0], pathToArma3ServerMods_textBox.Text + item.Text.Split(':')[0], true);
                }

                MessageBox.Show("Done.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();

            selectFile.Title = "Select arma3.exe";
            selectFile.Filter = "Executable File (.exe) | *.exe";
            selectFile.RestoreDirectory = true;

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                pathToArma3Client_textBox.Text = selectFile.FileName;

                ReadPresetFile();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.Description = "Select client mods folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                pathToArma3ClientMods_textBox.Text = chosenFolder.SelectedPath;

                ReadPresetFile();
            }
        }

        private void changePathToArma3Server_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();

            selectFile.Title = "Select arma3server.exe";
            selectFile.Filter = "Executable File (.exe) | *.exe";
            selectFile.RestoreDirectory = true;

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                pathToArma3Server_textBox.Text = selectFile.FileName;

                ReadPresetFile();
            }
        }

        private void changePathToArma3ServerMods_button_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.Description = "Select server mods folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                pathToArma3ServerMods_textBox.Text = chosenFolder.SelectedPath;

                ReadPresetFile();
            }
        }

        private void changeServerConfig_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();

            selectFile.Title = "Select Config";
            selectFile.Filter = "Config File (.cfg) | *.cfg";
            selectFile.RestoreDirectory = true;

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                serverConfig_textBox.Text = selectFile.FileName;

                ReadPresetFile();
            }
        }

        private void changeServerCfg_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();

            selectFile.Title = "Select Cfg";
            selectFile.Filter = "Cfg File (.cfg) | *.cfg";
            selectFile.RestoreDirectory = true;

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                serverCfg_textBox.Text = selectFile.FileName;

                ReadPresetFile();
            }
        }

        private void changeServerProfiles_button_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.Description = "Select profiles folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                serverProfiles_textBox.Text = chosenFolder.SelectedPath;

                ReadPresetFile();
            }
        }

        private void addCustomServerMod_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.Description = "Select custom mod folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                serverCustomMods_listView.Items.Add(chosenFolder.SelectedPath);

                GetWebModLineNewThread();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Remove " + clientExcessFiles_listView.Items.Count + " excess files?", "", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                foreach (ListViewItem X in clientExcessFiles_listView.Items)
                {
                    File.Delete(pathToArma3ClientMods_textBox.Text + X.Text.Split(':')[0]);
                }

                MessageBox.Show("Done.");
            }
        }

        private void removeUncheckedMod_button_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in clientCustomMods_listView.Items)
            {
                if (!item.Checked)
                    item.Remove();
            }
        }

        private void removeUncheckedServerMod_button_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in serverCustomMods_listView.Items)
            {
                if (!item.Checked)
                    item.Remove();
            }
        }

        private void closeServer_button_Click(object sender, EventArgs e)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("arma3server");

                foreach (Process process in processes)
                {
                    process.Kill();
                }

                if (processes.Count() > 0)
                    MessageBox.Show("Server process closed.");
                else
                    MessageBox.Show("Server process not found.");
            }
            catch
            {

            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/rebelvg/MurshunLauncher/releases");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveXmlFile();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://community.bistudio.com/wiki/Arma_3_Startup_Parameters");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (defaultStartLine_textBox.Text.Contains("-showscripterrors"))
            {
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text.Replace(" -showscripterrors", "");
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text.Replace("-showscripterrors", "");
            }
            else
            {
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text + " -showscripterrors";
            }
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (defaultStartLine_textBox.Text.Contains("-nologs"))
            {
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text.Replace(" -nologs", "");
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text.Replace("-nologs", "");
            }
            else
            {
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text + " -nologs";
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ReadPresetFile();

            CheckSyncFolderSize();

            VerifyMods(false);
        }

        private void createVerifyFile_button_Click(object sender, EventArgs e)
        {
            GetWebModLineNewThread();

            List<string> folderFiles = Directory.GetFiles(pathToArma3ClientMods_textBox.Text, "*", SearchOption.AllDirectories).ToList();

            folderFiles = folderFiles.Select(a => a.Replace(pathToArma3ClientMods_textBox.Text, "")).Select(b => b.ToLower()).ToList();

            folderFiles = folderFiles.Where(a => presetModsList.Any(b => a.StartsWith("\\" + b + "\\"))).ToList();

            Dictionary<string, dynamic> files = new Dictionary<string, dynamic>();

            files["mods"] = presetModsList;
            files["files"] = new Dictionary<string, dynamic>();

            Dictionary<string, dynamic> json_old = new Dictionary<string, dynamic>();

            if (File.Exists(pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.json"))
                json_old = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.json"));

            progressBar2.Minimum = 0;
            progressBar2.Maximum = folderFiles.Count();
            progressBar2.Value = 0;
            progressBar2.Step = 1;

            long totalSize = 0;

            foreach (string X in folderFiles)
            {
                FileInfo file = new FileInfo(pathToArma3ClientMods_textBox.Text + X);

                Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

                data["size"] = file.Length;
                data["date"] = file.LastWriteTimeUtc;

                try
                {
                    if (json_old["files"][X].date == file.LastWriteTimeUtc)
                        data["md5"] = json_old["files"][X].md5;
                    else
                        data["md5"] = GetMD5(pathToArma3ClientMods_textBox.Text + X);
                }
                catch
                {
                    data["md5"] = GetMD5(pathToArma3ClientMods_textBox.Text + X);
                }

                files["files"][X] = data;

                totalSize += file.Length;

                progressBar2.PerformStep();
            }

            string json_new = JsonConvert.SerializeObject(files, Formatting.Indented);

            File.WriteAllText(pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.json", json_new);
            File.WriteAllText(pathToArma3ServerMods_textBox.Text + "\\MurshunLauncherFiles.json", json_new);

            MessageBox.Show("MurshunLauncherFiles.json was saved to client and server mods folder.");

            Thread NewThread = new Thread(() => SetLauncherFiles(totalSize));
            NewThread.Start();
        }

        private void refreshClient_button_Click(object sender, EventArgs e)
        {
            ReadPresetFile();

            VerifyMods(false);
        }

        private void refreshServer_button_Click(object sender, EventArgs e)
        {
            GetWebModLineNewThread();
        }

        private void showServerTabs_checkBox_Click(object sender, EventArgs e)
        {
            if (showServerTabs_checkBox.Checked)
            {
                tabControl1.Controls.Add(tabPage3);
                tabControl1.Controls.Add(tabPage4);
            }
            else
            {
                tabControl1.Controls.Remove(tabPage3);
                tabControl1.Controls.Remove(tabPage4);
            }
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (defaultStartLineServer_textBox.Text.Contains("-nologs"))
            {
                defaultStartLineServer_textBox.Text = defaultStartLineServer_textBox.Text.Replace(" -nologs", "");
                defaultStartLineServer_textBox.Text = defaultStartLineServer_textBox.Text.Replace("-nologs", "");
            }
            else
            {
                defaultStartLineServer_textBox.Text = defaultStartLineServer_textBox.Text + " -nologs";
            }
        }

        private void copyMissions_button_Click(object sender, EventArgs e)
        {
            CopyMissions();
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            SaveXmlFile();
        }

        private void fullVerify_button_Click(object sender, EventArgs e)
        {
            VerifyMods(true);
        }
    }
}
