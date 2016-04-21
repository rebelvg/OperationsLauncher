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

                string launcherVersion = "0.242";
                label3.Text = "Version " + launcherVersion;                
            }
            catch (Exception e)
            {
                MessageBox.Show("Launcher crashed while initializing. Try running it as administrator.\n\n" + e);
                System.Environment.Exit(1);
            }
        }

        List<string> presetModsList = new List<string>();

        MurshunLauncherXmlSettings LauncherSettings;

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

        public void ReadXmlFile()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MurshunLauncherXmlSettings));

            StreamReader reader = new StreamReader(xmlPath_textBox.Text);

            try
            {
                LauncherSettings = (MurshunLauncherXmlSettings)serializer.Deserialize(reader);
                reader.Close();

                pathToArma3Client_textBox.Text = LauncherSettings.pathToArma3Client_textBox;
                pathToArma3ClientMods_textBox.Text = LauncherSettings.pathToArma3ClientMods_textBox;
                joinTheServer_checkBox.Checked = LauncherSettings.joinTheServer_checkBox;
                defaultStartLine_textBox.Text = LauncherSettings.defaultStartLine_textBox;
                advancedStartLine_textBox.Text = LauncherSettings.advancedStartLine_textBox;
                showServerTabs_checkBox.Checked = LauncherSettings.showServerTabs_checkBox;

                pathToArma3Server_textBox.Text = LauncherSettings.pathToArma3Server_textBox;
                pathToArma3ServerMods_textBox.Text = LauncherSettings.pathToArma3ServerMods_textBox;
                serverConfig_textBox.Text = LauncherSettings.serverConfig_textBox;
                serverCfg_textBox.Text = LauncherSettings.serverCfg_textBox;
                serverProfiles_textBox.Text = LauncherSettings.serverProfiles_textBox;
                serverProfileName_textBox.Text = LauncherSettings.serverProfileName_textBox;
                hideWindow_checkBox.Checked = LauncherSettings.hideWindow_checkBox;

                foreach (string X in LauncherSettings.clientCustomMods_listView)
                {
                    if (!clientCustomMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).Contains(X))
                    {
                        clientCustomMods_listView.Items.Add(X);
                    }
                }

                foreach (ListViewItem X in clientCustomMods_listView.Items)
                {
                    if (LauncherSettings.clientCheckedModsList_listView.Contains(X.Text))
                    {
                        X.Checked = true;
                    }
                }

                foreach (string X in LauncherSettings.serverCustomMods_listView)
                {
                    if (!serverCustomMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).Contains(X))
                    {
                        serverCustomMods_listView.Items.Add(X);
                    }
                }

                foreach (ListViewItem X in serverCustomMods_listView.Items)
                {
                    if (LauncherSettings.serverCheckedModsList_listView.Contains(X.Text))
                    {
                        X.Checked = true;
                    }
                }

                if (!LauncherSettings.showServerTabs_checkBox)
                {
                    tabControl1.Controls.Remove(tabPage3);
                    tabControl1.Controls.Remove(tabPage4);
                }
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
                LauncherSettings.showServerTabs_checkBox = showServerTabs_checkBox.Checked;

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

                System.IO.FileStream writer = System.IO.File.Create(xmlPath_textBox.Text);
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
            string murshunLauncherFilesPath = pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.txt";

            if (File.Exists(murshunLauncherFilesPath))
            {
                string[] btsync_fileLinesArray = File.ReadAllLines(murshunLauncherFilesPath);

                presetModsList = btsync_fileLinesArray.Where(x => x.Count(f => f == '\\') == 1).Select(s => s.Replace("\\", "")).ToList();

                RefreshPresetModsList();
            }
            else
            {
                Thread GetModsThread = new Thread(() => GetWebModLineNewThread());
                GetModsThread.Start();
            }
        }

        public void GetWebModLineNewThread()
        {
            WebClient client = new WebClient();

            try
            {
                string modLineString = client.DownloadString("http://dedick.podkolpakom.net/arma/mods.php?json");

                dynamic modLineJson = JsonConvert.DeserializeObject(modLineString);

                presetModsList.Clear();

                foreach (string X in modLineJson)
                {
                    presetModsList.Add(X);
                }
                
                this.Invoke(new Action(() => RefreshPresetModsList()));
            }
            catch
            {

            }
        }

        public void VerifyMods()
        {
            string murshunLauncherFilesPath = pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.txt";

            if (File.Exists(murshunLauncherFilesPath))
            {
                string[] btsync_fileLinesArray = File.ReadAllLines(murshunLauncherFilesPath);

                List<string> btsync_foldersList = btsync_fileLinesArray.Where(x => x.Count(f => f == '\\') == 1).Select(s => s.ToLower()).ToList();
                List<string> btsync_filesList = btsync_fileLinesArray.Where(x => x.Count(f => f == '\\') > 1).Select(s => s.ToLower()).Where(x => x.Split(':')[0].EndsWith(".pbo")).ToList();

                List<string> folder_foldersArray = Directory.GetDirectories(pathToArma3ClientMods_textBox.Text, "*", SearchOption.TopDirectoryOnly).Where(s => s.Contains("@")).ToList();
                List<string> folder_filesArray = Directory.GetFiles(pathToArma3ClientMods_textBox.Text, "*.pbo", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToList();

                folder_foldersArray = folder_foldersArray.Select(s => s.Replace(pathToArma3ClientMods_textBox.Text, "")).Select(s => s.ToLower()).ToList();
                folder_filesArray = folder_filesArray.Select(s => s.Replace(pathToArma3ClientMods_textBox.Text, "")).Select(s => s.ToLower()).Where(x => x.StartsWith("\\@")).ToList();
                
                clientModsFiles_listView.Items.Clear();
                murshunLauncherFiles_listView.Items.Clear();

                foreach (string X in folder_filesArray)
                {
                    if (btsync_foldersList.Select(x => x + "\\").Any(X.Contains))
                    {
                        FileInfo file = new FileInfo(pathToArma3ClientMods_textBox.Text + X);
                        clientModsFiles_listView.Items.Add(X + ":" + file.Length);
                    }
                }

                foreach (string X in btsync_filesList)
                {
                    murshunLauncherFiles_listView.Items.Add(X);
                }

                folder_filesArray = clientModsFiles_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();

                clientMissingFiles_listView.Items.Clear();
                clientExcessFiles_listView.Items.Clear();

                foreach (string X in btsync_filesList.Where(x => !folder_filesArray.Contains(x)))
                {
                    clientMissingFiles_listView.Items.Add(X);
                }

                foreach (string X in folder_filesArray.Where(x => !btsync_filesList.Contains(x)))
                {
                    clientExcessFiles_listView.Items.Add(X);
                }

                clientMods_textBox.Text = "Client Mods (" + clientModsFiles_listView.Items.Count + " pbos / " + clientMissingFiles_listView.Items.Count + " missing)";
                murshunLauncherFiles_textBox.Text = "MurshunLauncherFiles.txt (" + murshunLauncherFiles_listView.Items.Count + " pbos / " + clientExcessFiles_listView.Items.Count + " excess)";
            }
            else
            {
                MessageBox.Show("MurshunLauncherFiles.txt not found. Select your BTsync folder as Arma 3 Mods folder.");
            }
        }

        public void RefreshPresetModsList()
        {
            if (File.Exists(pathToArma3Client_textBox.Text))
                pathToArma3Client_textBox.BackColor = Color.Green;
            else
                pathToArma3Client_textBox.BackColor = Color.Red;

            if (Directory.Exists(pathToArma3ClientMods_textBox.Text))
                pathToArma3ClientMods_textBox.BackColor = Color.Green;
            else
                pathToArma3ClientMods_textBox.BackColor = Color.Red;

            if (File.Exists(pathToArma3Server_textBox.Text))
                pathToArma3Server_textBox.BackColor = Color.Green;
            else
                pathToArma3Server_textBox.BackColor = Color.Red;

            if (Directory.Exists(pathToArma3ServerMods_textBox.Text))
                pathToArma3ServerMods_textBox.BackColor = Color.Green;
            else
                pathToArma3ServerMods_textBox.BackColor = Color.Red;

            if (File.Exists(serverConfig_textBox.Text))
                serverConfig_textBox.BackColor = Color.Green;
            else
                serverConfig_textBox.BackColor = Color.Red;

            if (File.Exists(serverCfg_textBox.Text))
                serverCfg_textBox.BackColor = Color.Green;
            else
                serverCfg_textBox.BackColor = Color.Red;

            if (Directory.Exists(serverProfiles_textBox.Text))
                serverProfiles_textBox.BackColor = Color.Green;
            else
                serverProfiles_textBox.BackColor = Color.Red;

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
                if (Directory.Exists(pathToArma3ClientMods_textBox.Text + "\\" + X.Text + "\\addons"))
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
                if (Directory.Exists(X.Text + "\\addons"))
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
                if (Directory.Exists(pathToArma3ServerMods_textBox.Text + "\\" + X.Text + "\\addons"))
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
                if (Directory.Exists(X.Text + "\\addons"))
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
            string archivePath = pathToArma3ClientMods_textBox.Text + "\\.sync\\Archive";

            if (Directory.Exists(archivePath))
            {
                string[] archiveFilesArray = Directory.GetFiles(archivePath, "*", SearchOption.AllDirectories).ToArray();

                long bytes = 0;
                foreach (string name in archiveFilesArray)
                {
                    FileInfo file = new FileInfo(name);
                    bytes += file.Length;
                }

                if ((bytes / 1024 / 1024 / 1024) >= 1)
                {
                    MessageBox.Show("Your btsync archive folder is too large. It's size is over " + (bytes / 1024 / 1024 / 1024) + " GB. You can clear it and disable archiving in the btsync client.");
                    System.Diagnostics.Process.Start(archivePath);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VerifyMods();
        }

        private void launch_button_Click(object sender, EventArgs e)
        {
            ReadPresetFile();

            VerifyMods();

            if (clientMissingFiles_listView.Items.Count != 0 || clientExcessFiles_listView.Items.Count != 0)
            {
                tabControl1.SelectedTab = tabPage2;

                DialogResult dialogResult = MessageBox.Show("Launch the client anyway?", "You have missing or excess files.", MessageBoxButtons.YesNo);

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

            if (clientPresetMods_listView.Items.Count > 0 || clientCustomMods_listView.CheckedItems.Count > 0)
            {
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
            }

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

            if (chosenFolder.ShowDialog() == DialogResult.OK)
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

            button9_Click(null, null);

            if (compareMissingFiles_listView.Items.Count != 0 || compareExcessFiles_listView.Items.Count != 0)
            {
                tabControl1.SelectedTab = tabPage4;

                DialogResult dialogResult = MessageBox.Show("Launch the server anyway?", "You have missing or excess files.", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    tabControl1.SelectedTab = tabPage3;
                }
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                List<string> clientMissionlist = Directory.GetFiles(pathToArma3Client_textBox.Text.ToLower().Replace("arma3.exe", "mpmissions"), "*", SearchOption.AllDirectories).Where(s => s.Contains(".pbo")).ToList();

                foreach (string X in clientMissionlist)
                {
                    File.Copy(X, X.Replace(pathToArma3Client_textBox.Text.ToLower().Replace("arma3.exe", "mpmissions"), pathToArma3Server_textBox.Text.ToLower().Replace("arma3server.exe", "mpmissions")), true);
                }
            }
            catch
            {
                MessageBox.Show("Error copying missions.");
                return;
            }

            string modLine;

            modLine = defaultStartLineServer_textBox.Text;

            modLine = modLine + " \"-config=" + serverConfig_textBox.Text + "\"";

            modLine = modLine + " \"-cfg=" + serverCfg_textBox.Text + "\"";

            modLine = modLine + " \"-profiles=" + serverProfiles_textBox.Text + "\"";

            modLine = modLine + " -name=" + serverProfileName_textBox.Text;

            if (serverPresetMods_listView.Items.Count > 0)
            {
                modLine = modLine + " \"-mod=";

                foreach (ListViewItem X in serverPresetMods_listView.Items)
                {
                    modLine = modLine + pathToArma3ServerMods_textBox.Text + "\\" + X.Text + ";";
                }

                modLine = modLine + "\"";
            }

            if (serverCustomMods_listView.CheckedItems.Count > 0)
            {
                modLine = modLine + " \"-servermod=";

                foreach (ListViewItem X in serverCustomMods_listView.CheckedItems)
                {
                    modLine = modLine + X.Text + ";";
                }

                modLine = modLine + "\"";
            }

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
            GetWebModLineNewThread();

            List<string> folder_clientFilesList = Directory.GetFiles(pathToArma3ClientMods_textBox.Text, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToList();
            List<string> folder_serverFilesList = Directory.GetFiles(pathToArma3ServerMods_textBox.Text, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToList();

            compareClientFiles_listView.Items.Clear();
            compareServerFiles_listView.Items.Clear();

            foreach (string X in folder_clientFilesList)
            {
                if (presetModsList.Select(x => x + "\\").Any(X.Contains))
                {
                    FileInfo file = new FileInfo(X);
                    compareClientFiles_listView.Items.Add(X.Replace(pathToArma3ClientMods_textBox.Text, "").ToLower() + ":" + file.Length + ":" + file.LastWriteTimeUtc);
                }
            }

            foreach (string X in folder_serverFilesList)
            {
                if (presetModsList.Select(x => x + "\\").Any(X.Contains))
                {
                    FileInfo file = new FileInfo(X);
                    compareServerFiles_listView.Items.Add(X.Replace(pathToArma3ServerMods_textBox.Text, "").ToLower() + ":" + file.Length + ":" + file.LastWriteTimeUtc);
                }
            }

            folder_clientFilesList = compareClientFiles_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();
            folder_serverFilesList = compareServerFiles_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();

            List<string> missingFilesList = folder_clientFilesList.Where(x => !folder_serverFilesList.Contains(x)).ToList();
            List<string> excessFilesList = folder_serverFilesList.Where(x => !folder_clientFilesList.Contains(x)).ToList();

            compareMissingFiles_listView.Items.Clear();
            compareExcessFiles_listView.Items.Clear();

            foreach (string X in missingFilesList)
            {
                compareMissingFiles_listView.Items.Add(X);
            }

            foreach (string X in excessFilesList)
            {
                compareExcessFiles_listView.Items.Add(X);
            }

            compareClientMods_textBox.Text = "Client Mods (" + compareClientFiles_listView.Items.Count + " files / " + compareMissingFiles_listView.Items.Count + " missing)";
            compareServerMods_textBox.Text = "Server Mods (" + compareServerFiles_listView.Items.Count + " files / " + compareExcessFiles_listView.Items.Count + " excess)";
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
            DialogResult dialogResult = MessageBox.Show("Sync " + (compareExcessFiles_listView.Items.Count + compareMissingFiles_listView.Items.Count) + " files?", "", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                MessageBox.Show("Removing " + compareExcessFiles_listView.Items.Count + " files.");

                foreach (ListViewItem item in compareExcessFiles_listView.Items)
                {
                    File.Delete(pathToArma3ServerMods_textBox.Text + item.Text.Split(':')[0]);
                }

                MessageBox.Show("Copying " + compareMissingFiles_listView.Items.Count + " files.");

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
            FolderBrowserDialog chosenFolder = new FolderBrowserDialog();
            chosenFolder.Description = "Select client mods folder.";

            if (chosenFolder.ShowDialog() == DialogResult.OK)
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
            FolderBrowserDialog chosenFolder = new FolderBrowserDialog();
            chosenFolder.Description = "Select server mods folder.";

            if (chosenFolder.ShowDialog() == DialogResult.OK)
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
            FolderBrowserDialog chosenFolder = new FolderBrowserDialog();
            chosenFolder.Description = "Select profiles folder.";

            if (chosenFolder.ShowDialog() == DialogResult.OK)
            {
                serverProfiles_textBox.Text = chosenFolder.SelectedPath;

                ReadPresetFile();
            }
        }

        private void addCustomServerMod_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog chosenFolder = new FolderBrowserDialog();
            chosenFolder.Description = "Select custom mod folder.";

            if (chosenFolder.ShowDialog() == DialogResult.OK)
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

            VerifyMods();

            if (clientMissingFiles_listView.Items.Count != 0 || clientExcessFiles_listView.Items.Count != 0)
            {
                MessageBox.Show("You have missing or excess files.");
                tabControl1.SelectedTab = tabPage2;
                return;
            }
        }

        private void createVerifyFile_button_Click(object sender, EventArgs e)
        {
            GetWebModLineNewThread();

            List<string> folder_filesArray = Directory.GetFiles(pathToArma3ClientMods_textBox.Text, "*.pbo", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToList();

            folder_filesArray = folder_filesArray.Select(s => s.Replace(pathToArma3ClientMods_textBox.Text, "")).Select(x => x.ToLower()).ToList();

            List<string> infoToWrite = new List<string>();

            foreach (string X in presetModsList)
            {
                infoToWrite.Add("\\" + X);
            }

            foreach (string X in folder_filesArray)
            {
                if (presetModsList.Select(x => x + "\\").Any(X.Contains))
                {
                    FileInfo file = new FileInfo(pathToArma3ClientMods_textBox.Text + X);
                    infoToWrite.Add(X + ":" + file.Length);
                }
            }

            File.WriteAllLines(pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.txt", infoToWrite);
            File.WriteAllLines(pathToArma3ServerMods_textBox.Text + "\\MurshunLauncherFiles.txt", infoToWrite);

            MessageBox.Show("MurshunLauncherFiles.txt was saved to client and server mods folder.");
        }

        private void refreshClient_button_Click(object sender, EventArgs e)
        {
            ReadPresetFile();
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
    }
}
