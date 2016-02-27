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

namespace MurshunLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (Process.GetProcessesByName("MurshunLauncher").Length > 1)
            {
                MessageBox.Show("Launcher is already running.");
                System.Environment.Exit(1);
            }

            pathToTheLauncher = Directory.GetCurrentDirectory();
            iniDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MurshunLauncher";
            
            xmlFilePath = iniDirectoryPath + "\\MurshunLauncher.xml";
            xmlPath_textBox.Text = xmlFilePath;

            string launcherVersion = "0.235";
            
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

            if (File.Exists(xmlFilePath))
            {
                ReadXmlFile();
            }
            else
            {
                try
                {
                    LauncherSettings = new MurshunLauncherXmlSettings();

                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MurshunLauncherXmlSettings));

                    System.IO.FileStream writer = System.IO.File.Create(xmlFilePath);
                    serializer.Serialize(writer, LauncherSettings);
                    writer.Close();

                    ReadXmlFile();
                }
                catch
                {
                    MessageBox.Show("Saving xml settings failed.");
                }
            }

            ReadPresetFile();

            label3.Text = "Version " + launcherVersion;

            CheckSyncFolderSize();
        }

        string pathToTheLauncher;
        string iniDirectoryPath;
        string xmlFilePath;
        string lastSelectedFolder;
        string verifyFilePath;

        bool serverTabEnabled;

        string[] missingFilesArray;
        string[] excessFilesArray;

        List<string> presetModsList;

        MurshunLauncherXmlSettings LauncherSettings;

        public class MurshunLauncherXmlSettings
        {
            public string pathToArma3Client_textBox = Directory.GetCurrentDirectory() + "\\arma3.exe";
            public string pathToArma3ClientMods_textBox = Directory.GetCurrentDirectory();
            public bool joinTheServer_checkBox;
            public List<string> clientCustomMods_listView;
            public List<string> clientCheckedModsList_listView;
            public string defaultStartLine_textBox = "-world=empty -nosplash -skipintro -nologs -nofilepatching";
            public string advancedStartLine_textBox;
            public bool verifyBeforeLaunch_checkBox;
            public string lastSelectedFolder = Directory.GetCurrentDirectory();
            public bool serverTabEnabled;
            public string verifyFilePath = Directory.GetCurrentDirectory() + "\\MurshunLauncherFiles.txt";

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

        public void ReadXmlFile()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MurshunLauncherXmlSettings));

            StreamReader reader = new StreamReader(xmlFilePath);

            try
            {
                LauncherSettings = (MurshunLauncherXmlSettings)serializer.Deserialize(reader);
                reader.Close();

                pathToArma3Client_textBox.Text = LauncherSettings.pathToArma3Client_textBox;
                pathToArma3ClientMods_textBox.Text = LauncherSettings.pathToArma3ClientMods_textBox;
                joinTheServer_checkBox.Checked = LauncherSettings.joinTheServer_checkBox;

                foreach (string X in LauncherSettings.clientCustomMods_listView)
                {
                    if (!clientCustomMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).Contains(X))
                    {
                        clientCustomMods_listView.Items.Add(X);
                    }
                }

                for (int i = 0; i < clientCustomMods_listView.Items.Count; i++)
                {
                    if (LauncherSettings.clientCheckedModsList_listView.Contains(clientCustomMods_listView.Items[i].Text))
                    {
                        clientCustomMods_listView.Items[i].Checked = true;
                    }
                }

                defaultStartLine_textBox.Text = LauncherSettings.defaultStartLine_textBox;
                advancedStartLine_textBox.Text = LauncherSettings.advancedStartLine_textBox;
                verifyBeforeLaunch_checkBox.Checked = LauncherSettings.verifyBeforeLaunch_checkBox;
                lastSelectedFolder = LauncherSettings.lastSelectedFolder;
                serverTabEnabled = LauncherSettings.serverTabEnabled;
                verifyFilePath = LauncherSettings.verifyFilePath;

                if (serverTabEnabled)
                {
                    button1.Enabled = true;
                    button2.Enabled = false;
                }

                pathToArma3Server_textBox.Text = LauncherSettings.pathToArma3Server_textBox;
                pathToArma3ServerMods_textBox.Text = LauncherSettings.pathToArma3ServerMods_textBox;

                foreach (string X in LauncherSettings.serverCustomMods_listView)
                {
                    if (!serverCustomMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).Contains(X))
                    {
                        serverCustomMods_listView.Items.Add(X);
                    }
                }

                for (int i = 0; i < serverCustomMods_listView.Items.Count; i++)
                {
                    if (LauncherSettings.serverCheckedModsList_listView.Contains(serverCustomMods_listView.Items[i].Text))
                    {
                        serverCustomMods_listView.Items[i].Checked = true;
                    }
                }

                serverConfig_textBox.Text = LauncherSettings.serverConfig_textBox;
                serverCfg_textBox.Text = LauncherSettings.serverCfg_textBox;
                serverProfiles_textBox.Text = LauncherSettings.serverProfiles_textBox;
                serverProfileName_textBox.Text = LauncherSettings.serverProfileName_textBox;
                hideWindow_checkBox.Checked = LauncherSettings.hideWindow_checkBox;
            }
            catch
            {
                reader.Close();

                DialogResult dialogResult = MessageBox.Show("Create a new one?", "Xml file is corrupted.", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    SaveXmlFile();
                }
                if (dialogResult == DialogResult.No)
                {
                    System.Environment.Exit(1);
                }
            }
        }

        public void SaveXmlFile()
        {
            try
            {
                LauncherSettings = new MurshunLauncherXmlSettings();

                LauncherSettings.pathToArma3Client_textBox = pathToArma3Client_textBox.Text;
                LauncherSettings.pathToArma3ClientMods_textBox = pathToArma3ClientMods_textBox.Text;
                LauncherSettings.joinTheServer_checkBox = joinTheServer_checkBox.Checked;
                LauncherSettings.clientCustomMods_listView = clientCustomMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();
                LauncherSettings.clientCheckedModsList_listView = clientCustomMods_listView.CheckedItems.Cast<ListViewItem>().Select(x => x.Text).ToList();
                LauncherSettings.defaultStartLine_textBox = defaultStartLine_textBox.Text;
                LauncherSettings.advancedStartLine_textBox = advancedStartLine_textBox.Text;
                LauncherSettings.verifyBeforeLaunch_checkBox = verifyBeforeLaunch_checkBox.Checked;
                LauncherSettings.lastSelectedFolder = lastSelectedFolder;
                LauncherSettings.serverTabEnabled = serverTabEnabled;
                LauncherSettings.verifyFilePath = verifyFilePath;

                LauncherSettings.pathToArma3Server_textBox = pathToArma3Server_textBox.Text;
                LauncherSettings.pathToArma3ServerMods_textBox = pathToArma3ServerMods_textBox.Text;
                LauncherSettings.serverCustomMods_listView = serverCustomMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();
                LauncherSettings.serverCheckedModsList_listView = serverCustomMods_listView.CheckedItems.Cast<ListViewItem>().Select(x => x.Text).ToList();
                LauncherSettings.serverConfig_textBox = serverConfig_textBox.Text;
                LauncherSettings.serverCfg_textBox = serverCfg_textBox.Text;
                LauncherSettings.serverProfiles_textBox = serverProfiles_textBox.Text;
                LauncherSettings.serverProfileName_textBox = serverProfileName_textBox.Text;
                LauncherSettings.hideWindow_checkBox = hideWindow_checkBox.Checked;

                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MurshunLauncherXmlSettings));

                System.IO.FileStream writer = System.IO.File.Create(xmlFilePath);
                serializer.Serialize(writer, LauncherSettings);
                writer.Close();
            }
            catch
            {
                MessageBox.Show("Saving xml settings failed.");
            }
        }

        public void ReadPresetFile()
        {
            if (File.Exists(pathToArma3ClientMods_textBox.Text + "\\arma3_murshun_preset.txt"))
            {
                string[] infoFromPresetFile = File.ReadAllLines(pathToArma3ClientMods_textBox.Text + "\\arma3_murshun_preset.txt");

                string modsStringArray = infoFromPresetFile[0];

                presetModsList = modsStringArray.Split(';').ToList();

                presetModsList = presetModsList.Select(s => s.Replace("-mod=", "")).ToList();
                presetModsList = presetModsList.Select(s => s.Replace(";", "")).ToList();
                presetModsList.RemoveAll(s => String.IsNullOrEmpty(s.Trim()));

                RefreshPresetModsList();
            }
            else
            {
                GetWebModLine();
            }
        }

        public void VerifyMods()
        {
            verifyModsFolder_textBox.Text = pathToArma3ClientMods_textBox.Text;

            if (File.Exists(verifyFilePath))
            {
                verifyFilePath_textBox.Text = verifyFilePath;
            }
            else
            {
                if (File.Exists(pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.txt"))
                {
                    verifyFilePath_textBox.Text = pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.txt";
                }
                else
                {
                    button7_Click(null, null);
                }
                
            }

            if (File.Exists(verifyFilePath))
            {
                string[] btsync_fileLinesArray = File.ReadAllLines(verifyFilePath);

                List<string> btsync_fileLinesList = new List<string>();

                btsync_fileLinesList = btsync_fileLinesArray.ToList();

                btsync_fileLinesList.Add("\\Arma 3\\Userconfig\\task_force_radio\\radio_settings.hpp");

                btsync_fileLinesArray = btsync_fileLinesList.ToArray();

                List<string> btsync_filesList = new List<string>();
                List<string> btsync_foldersList = new List<string>();

                foreach (string X in btsync_fileLinesArray)
                {
                    int count = X.Count(f => f == '\\');

                    if (count > 1)
                    {
                        btsync_filesList.Add(X);
                    }
                    else
                    {
                        btsync_foldersList.Add(X + "\\");
                    }
                }

                listView4.Items.Clear();

                foreach (string X in btsync_filesList)
                {
                    listView4.Items.Add(X);
                }                

                string[] folder_foldersArray = Directory.GetDirectories(pathToArma3ClientMods_textBox.Text, "*", SearchOption.TopDirectoryOnly).Where(s => s.Contains("@")).ToArray();
                string[] folder_filesArray = Directory.GetFiles(pathToArma3ClientMods_textBox.Text, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToArray();

                folder_foldersArray = folder_foldersArray.Select(s => s.Replace(pathToArma3ClientMods_textBox.Text, "")).ToArray();
                folder_filesArray = folder_filesArray.Select(s => s.Replace(pathToArma3ClientMods_textBox.Text, "")).ToArray();

                List<string> folder_filesList = new List<string>();

                foreach (string X in folder_foldersArray)
                {
                    if (btsync_foldersList.Any(X.Contains) && X.Substring(0, 2) == "\\@")
                    {
                        folder_filesList.Add(X);
                    }
                }

                foreach (string X in folder_filesArray)
                {
                    if (btsync_foldersList.Any(X.Contains) && X.Substring(0, 2) == "\\@")
                    {
                        FileInfo F = new FileInfo(pathToArma3ClientMods_textBox.Text + X);
                        folder_filesList.Add(X + ":" + F.Length);
                    }
                }

                listView1.Items.Clear();

                if (File.Exists(pathToArma3Client_textBox.Text.Replace("\\arma3.exe", "") + "\\Userconfig\\task_force_radio\\radio_settings.hpp"))
                {
                    folder_filesList.Add("\\Arma 3\\Userconfig\\task_force_radio\\radio_settings.hpp");
                }

                foreach (string X in folder_filesList)
                {
                    listView1.Items.Add(X);
                }

                folder_filesArray = folder_filesList.ToArray();

                excessFilesArray = folder_filesArray.Where(x => !btsync_fileLinesArray.Contains(x)).ToArray();

                missingFilesArray = btsync_fileLinesArray.Where(x => !folder_filesArray.Contains(x)).ToArray();
                missingFilesArray = missingFilesArray.Where(x => !folder_foldersArray.Contains(x)).ToArray();

                listView2.Items.Clear();

                listView3.Items.Clear();

                if (missingFilesArray.Length == 0)
                {
                    listView2.Items.Add("No missing files.");
                }
                else
                {
                    foreach (string X in missingFilesArray)
                    {
                        listView2.Items.Add(X);
                    }
                }

                if (excessFilesArray.Length == 0)
                {
                    listView3.Items.Add("No excess files.");
                }
                else
                {
                    foreach (string X in excessFilesArray)
                    {
                        listView3.Items.Add(X);
                    }
                }
            }
            else
            {
                MessageBox.Show("MurshunLauncherFiles.txt not found.");
            }
        }

        public void GetWebModLine()
        {
            WebClient client = new WebClient();

            try
            {
                string webModLine = client.DownloadString("http://dedick.podkolpakom.net/arma/mods.php");
                
                presetModsList = webModLine.Split(';').ToList();

                presetModsList = presetModsList.Select(s => s.Replace("-mod=", "")).ToList();
                presetModsList = presetModsList.Select(s => s.Replace(";", "")).ToList();
                presetModsList.RemoveAll(s => String.IsNullOrEmpty(s.Trim()));
                
                if (InvokeRequired)
                {
                    this.Invoke(new Action(() => RefreshPresetModsList()));
                }
                else
                {
                    RefreshPresetModsList();
                }
            }
            catch (Exception)
            {
                if (!InvokeRequired)
                {
                    MessageBox.Show("Couldn't retrieve preset list from Poddy. Press Refresh to try again.");
                }
                else
                {
                    this.Invoke(new Action(() => MessageBox.Show("Couldn't retrieve preset list from Poddy. Press Refresh to try again.")));

                    this.Invoke(new Action(() => RefreshPresetModsList()));
                }
            }
        }

        public void RefreshPresetModsList()
        {
            clientPresetMods_listView.Items.Clear();

            serverPresetMods_listView.Items.Clear();

            foreach (string X in presetModsList)
            {
                clientPresetMods_listView.Items.Add(X);
            }

            foreach (string X in presetModsList)
            {
                serverPresetMods_listView.Items.Add(X);
            }

            foreach (ListViewItem X in clientPresetMods_listView.Items)
            {
                if (Directory.Exists(pathToArma3ClientMods_textBox.Text + "\\" + X.Text + "\\addons") || Directory.Exists(pathToArma3ClientMods_textBox.Text + "\\" + X.Text + "\\Addons"))
                {
                    if (X.BackColor != Color.Green)
                        X.BackColor = Color.Green;
                }
                else
                {
                    if (X.BackColor != Color.Red)
                        X.BackColor = Color.Red;
                }
            }

            foreach (ListViewItem X in clientCustomMods_listView.Items)
            {
                if (Directory.Exists(X.Text + "\\addons") || Directory.Exists(X.Text + "\\Addons"))
                {
                    if (X.BackColor != Color.Green)
                        X.BackColor = Color.Green;
                }
                else
                {
                    if (X.BackColor != Color.Red)
                        X.BackColor = Color.Red;
                }
            }

            columnHeader7.Width = -2;

            foreach (ListViewItem X in serverPresetMods_listView.Items)
            {
                if (Directory.Exists(pathToArma3ServerMods_textBox.Text + "\\" + X.Text + "\\addons") || Directory.Exists(pathToArma3ServerMods_textBox.Text + "\\" + X.Text + "\\Addons"))
                {
                    if (X.BackColor != Color.Green)
                        X.BackColor = Color.Green;
                }
                else
                {
                    if (X.BackColor != Color.Red)
                        X.BackColor = Color.Red;
                }
            }

            foreach (ListViewItem X in serverCustomMods_listView.Items)
            {
                if (Directory.Exists(X.Text + "\\addons") || Directory.Exists(X.Text + "\\Addons"))
                {
                    if (X.BackColor != Color.Green)
                        X.BackColor = Color.Green;
                }
                else
                {
                    if (X.BackColor != Color.Red)
                        X.BackColor = Color.Red;
                }
            }

            columnHeader9.Width = -2;
        }

        public void CheckSyncFolderSize()
        {
            string path = pathToArma3ClientMods_textBox.Text + "\\.sync\\Archive";

            if (Directory.Exists(path))
            {
                string[] a = Directory.GetFiles(path, "*", SearchOption.AllDirectories).ToArray();

                long b = 0;
                foreach (string name in a)
                {
                    FileInfo info = new FileInfo(name);
                    b += info.Length;
                }

                if ((b / 1024 / 1024 / 1024) >= 1)
                {
                    MessageBox.Show("Your btsync archive folder is too large. It's size is over " + (b / 1024 / 1024 / 1024) + " GB. You can clear it and disable archiving in the btsync client.");
                    System.Diagnostics.Process.Start(path);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            verifyModsFolder_textBox.Text = pathToArma3ClientMods_textBox.Text;

            string[] folder_foldersArray = Directory.GetDirectories(pathToArma3ClientMods_textBox.Text, "*", SearchOption.TopDirectoryOnly).Where(s => s.Contains("@")).ToArray();
            string[] folder_filesArray = Directory.GetFiles(pathToArma3ClientMods_textBox.Text, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToArray();

            folder_foldersArray = folder_foldersArray.Select(s => s.Replace(pathToArma3ClientMods_textBox.Text, "")).ToArray();
            folder_filesArray = folder_filesArray.Select(s => s.Replace(pathToArma3ClientMods_textBox.Text, "")).ToArray();

            listView1.Items.Clear();

            foreach (string X in folder_foldersArray)
            {
                if (X.Substring(0, 2) == "\\@" && presetModsList.Any(X.Contains))
                {
                    listView1.Items.Add(X);
                }
            }

            foreach (string X in folder_filesArray)
            {
                if (X.Substring(0, 2) == "\\@" && presetModsList.Any(X.Contains))
                {
                    FileInfo F = new FileInfo(pathToArma3ClientMods_textBox.Text + X);
                    listView1.Items.Add(X + ":" + F.Length);
                }
            }

            SaveFileDialog saveFile = new SaveFileDialog();

            saveFile.Title = "Save File Dialog";
            saveFile.Filter = "Text File (.txt) | *.txt";
            saveFile.FileName = "MurshunLauncherFiles.txt";
            saveFile.RestoreDirectory = true;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(saveFile.FileName, listView1.Items.Cast<ListViewItem>().Select(X => X.Text));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VerifyMods();
        }

        private void launch_button_Click(object sender, EventArgs e)
        {
            RefreshPresetModsList();

            if (verifyBeforeLaunch_checkBox.Checked)
            {
                VerifyMods();

                if (missingFilesArray.Length == 0 && excessFilesArray.Length == 0)
                {
                    MessageBox.Show("No missing or excess files.");
                }
                else
                {
                    tabControl1.SelectedTab = tabPage2;
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
                if (X.BackColor == Color.Red)
                {
                    MessageBox.Show(X.Text + " not found.");
                    return;
                }
            }

            foreach (ListViewItem X in clientCustomMods_listView.CheckedItems)
            {
                modLine = modLine + X.Text + ";";
                if (X.BackColor == Color.Red)
                {
                    MessageBox.Show(X.Text + " not found.");
                    return;
                }
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
            FolderBrowserDialog chosenFolder = new FolderBrowserDialog();
            chosenFolder.Description = "Select custom mod folder.";
            chosenFolder.SelectedPath = lastSelectedFolder;

            if (chosenFolder.ShowDialog() == DialogResult.OK)
            {
                lastSelectedFolder = chosenFolder.SelectedPath;

                clientCustomMods_listView.Items.Add(chosenFolder.SelectedPath);
                
                RefreshPresetModsList();
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://murshun.club/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/groups/murshun");
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            defaultStartLine_textBox.Text = "-world=empty -nosplash -skipintro -nologs -nofilepatching";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            RefreshPresetModsList();

            button9_Click(sender, e);

            if (listView12.Items.Count != 0 || listView10.Items.Count != 0)
            {
                tabControl1.SelectedTab = tabPage4;
                return;
            }

            try
            {
                List<string> clientMissionlist = Directory.GetFiles(pathToArma3Client_textBox.Text.Replace("arma3.exe", "MPMissions"), "*", SearchOption.AllDirectories).Where(s => s.Contains(".pbo")).ToList();
                
                foreach (string X in clientMissionlist)
                {
                    File.Copy(X, X.Replace(pathToArma3Client_textBox.Text.Replace("arma3.exe", "MPMissions"), pathToArma3Server_textBox.Text.Replace("arma3server.exe", "mpmissions")), true);
                }
            }
            catch
            {
                MessageBox.Show("Error copying missions.");
                return;
            }

            string modLine;

            modLine = "-port=2302";

            modLine = modLine + " \"-config=" + serverConfig_textBox.Text + "\"";

            modLine = modLine + " \"-cfg=" + serverCfg_textBox.Text + "\"";

            modLine = modLine + " \"-profiles=" + serverProfiles_textBox.Text + "\"";

            modLine = modLine + " -name=" + serverProfileName_textBox.Text;

            modLine = modLine + " \"-mod=";

            foreach (ListViewItem X in serverPresetMods_listView.Items)
            {
                modLine = modLine + pathToArma3ServerMods_textBox.Text + "\\" + X.Text + ";";
                if (X.BackColor == Color.Red)
                {
                    MessageBox.Show(X.Text + " not found.");
                    return;
                }
            }

            foreach (ListViewItem X in serverCustomMods_listView.CheckedItems)
            {
                modLine = modLine + X.Text + ";";
                if (X.BackColor == Color.Red)
                {
                    MessageBox.Show(X.Text + " not found.");
                    return;
                }
            }

            modLine = modLine + "\"";

            modLine = modLine + " -nologs -nofilepatching";

            if (File.Exists(pathToArma3Server_textBox.Text))
            {
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = pathToArma3Server_textBox.Text;
                myProcess.StartInfo.Arguments = modLine;
                if (hideWindow_checkBox.Checked) {
                    myProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                }
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
            textBox12.Text = pathToArma3ClientMods_textBox.Text;
            textBox11.Text = pathToArma3ServerMods_textBox.Text;

            List<string> folder_clientFilesList = Directory.GetFiles(pathToArma3ClientMods_textBox.Text, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToList();
            List<string> folder_serverFilesList = Directory.GetFiles(pathToArma3ServerMods_textBox.Text, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToList();

            listView13.Items.Clear();
            listView11.Items.Clear();

            foreach (string X in folder_clientFilesList)
            {
                FileInfo F = new FileInfo(X);
                listView13.Items.Add(X.Replace(pathToArma3ClientMods_textBox.Text, "") + ":" + F.Length + ":" + F.LastWriteTimeUtc);
            }

            foreach (string X in folder_serverFilesList)
            {
                FileInfo F = new FileInfo(X);
                listView11.Items.Add(X.Replace(pathToArma3ServerMods_textBox.Text, "") + ":" + F.Length + ":" + F.LastWriteTimeUtc);
            }

            folder_clientFilesList = listView13.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();
            folder_serverFilesList = listView11.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();

            List<string> missingFilesList = folder_clientFilesList.Where(x => !folder_serverFilesList.Contains(x)).ToList();
            List<string> excessFilesList = folder_serverFilesList.Where(x => !folder_clientFilesList.Contains(x)).ToList();

            listView12.Items.Clear();
            listView10.Items.Clear();

            foreach (string X in missingFilesList)
            {
                if (presetModsList.Any(X.Contains))
                {
                    listView12.Items.Add(X);
                }
            }

            foreach (string X in excessFilesList)
            {
                if (presetModsList.Any(X.Contains))
                {
                    listView10.Items.Add(X);
                }
            }
        }

        private void CheckPath(string path)
        {
            string[] paths = Path.GetDirectoryName(path).Split(Path.DirectorySeparatorChar);

            string fullpath = paths[0];

            foreach (string X in paths) {
                if (X != paths[0])
                {
                    fullpath = fullpath + "\\" + X;
                    if (!Directory.Exists(fullpath))
                        Directory.CreateDirectory(fullpath);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Removing " + listView10.Items.Count + " files.");

            foreach (ListViewItem item in listView10.Items)
            {
                File.Delete(pathToArma3ServerMods_textBox.Text + item.Text.Split(':')[0]);
            }

            MessageBox.Show("Copying " + listView12.Items.Count + " files.");

            foreach (ListViewItem item in listView12.Items)
            {
                CheckPath(pathToArma3ServerMods_textBox.Text + item.Text.Split(':')[0]);

                File.Copy(pathToArma3ClientMods_textBox.Text + item.Text.Split(':')[0], pathToArma3ServerMods_textBox.Text + item.Text.Split(':')[0], true);
            }

            MessageBox.Show("Done.");
        }

        private void clientCustomMods_listView_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void clientCustomMods_listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {

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
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog chosenFolder = new FolderBrowserDialog();
            chosenFolder.Description = "Select client mods folder.";
            chosenFolder.SelectedPath = lastSelectedFolder;

            if (chosenFolder.ShowDialog() == DialogResult.OK)
            {
                lastSelectedFolder = chosenFolder.SelectedPath;

                pathToArma3ClientMods_textBox.Text = chosenFolder.SelectedPath;
                
                ReadPresetFile();
                RefreshPresetModsList();
            }
        }

        private void defaultStartLine_textBox_Leave(object sender, EventArgs e)
        {

        }

        private void advancedStartLine_textBox_Leave(object sender, EventArgs e)
        {

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
            }
        }

        private void changePathToArma3ServerMods_button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog chosenFolder = new FolderBrowserDialog();
            chosenFolder.Description = "Select server mods folder.";
            chosenFolder.SelectedPath = lastSelectedFolder;

            if (chosenFolder.ShowDialog() == DialogResult.OK)
            {
                lastSelectedFolder = chosenFolder.SelectedPath;

                pathToArma3ServerMods_textBox.Text = chosenFolder.SelectedPath;

                ReadPresetFile();
                RefreshPresetModsList();
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
            }
        }

        private void changeServerProfiles_button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog chosenFolder = new FolderBrowserDialog();
            chosenFolder.Description = "Select profiles folder.";
            chosenFolder.SelectedPath = lastSelectedFolder;

            if (chosenFolder.ShowDialog() == DialogResult.OK)
            {
                lastSelectedFolder = chosenFolder.SelectedPath;

                serverProfiles_textBox.Text = chosenFolder.SelectedPath;
            }
        }

        private void serverProfileName_textBox_Leave(object sender, EventArgs e)
        {

        }

        private void addCustomServerMod_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog chosenFolder = new FolderBrowserDialog();
            chosenFolder.Description = "Select custom mod folder.";
            chosenFolder.SelectedPath = lastSelectedFolder;

            if (chosenFolder.ShowDialog() == DialogResult.OK)
            {
                lastSelectedFolder = chosenFolder.SelectedPath;

                serverCustomMods_listView.Items.Add(chosenFolder.SelectedPath);

                RefreshPresetModsList();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Removing " + excessFilesArray.Length + " files.");

            foreach (string X in excessFilesArray)
            {
                File.Delete(pathToArma3ClientMods_textBox.Text + X.Split(':')[0]);
            }

            MessageBox.Show("Done.");
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPage3 || e.TabPage == tabPage4)
            {
                if (!serverTabEnabled)
                {
                    e.Cancel = true;
                }
            }
        }

        private void removeUncheckedMod_button_Click(object sender, EventArgs e)
        {
            ListView clientCustomMods_listViewTemp;

            clientCustomMods_listViewTemp = clientCustomMods_listView;

            foreach (ListViewItem item in clientCustomMods_listViewTemp.Items)
            {
                if (!item.Checked)
                    item.Remove();
            }
        }

        private void removeUncheckedServerMod_button_Click(object sender, EventArgs e)
        {
            ListView clientCustomMods_listViewTemp;

            clientCustomMods_listViewTemp = serverCustomMods_listView;

            foreach (ListViewItem item in serverCustomMods_listView.Items)
            {
                if (!item.Checked)
                    item.Remove();
            }
        }

        private void serverCustomMods_listView_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void serverCustomMods_listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {

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
            }
            catch
            {

            }
        }

        private void hideWindow_checkBox_Click(object sender, EventArgs e)
        {

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

        private void button4_Click_1(object sender, EventArgs e)
        {
            Thread NewThread = new Thread(() => GetWebModLine());
            NewThread.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread NewThread = new Thread(() => GetWebModLine());
            NewThread.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();

            selectFile.Title = "Select MurshunLauncherFiles.txt";
            selectFile.Filter = "Text File (.txt) | *.txt";
            selectFile.RestoreDirectory = true;

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                verifyFilePath = selectFile.FileName;
            }
        }
    }
}
