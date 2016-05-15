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

        public bool VerifyMods()
        {
            bool verifySuccess = true;
            string murshunLauncherFilesPath = pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.txt";

            if (File.Exists(murshunLauncherFilesPath))
            {
                string[] btsync_fileLinesArray = File.ReadAllLines(murshunLauncherFilesPath);

                List<string> btsync_foldersList = btsync_fileLinesArray.Where(x => x.Count(f => f == '\\') == 1).Select(s => s.ToLower()).ToList();
                List<string> btsync_filesList = btsync_fileLinesArray.Where(x => x.Count(f => f == '\\') > 1).Select(s => s.ToLower()).ToList();

                List<string> folder_foldersArray = Directory.GetDirectories(pathToArma3ClientMods_textBox.Text, "*", SearchOption.TopDirectoryOnly).Where(s => s.Contains("@")).ToList();
                List<string> folder_filesArray = Directory.GetFiles(pathToArma3ClientMods_textBox.Text, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToList();

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

                long totalSizeLocal = 0;

                foreach (string X in btsync_filesList)
                {
                    murshunLauncherFiles_listView.Items.Add(X);

                    totalSizeLocal = totalSizeLocal + Convert.ToInt64(X.Split(':')[1]);
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

                clientMods_textBox.Text = "Client Mods (" + clientModsFiles_listView.Items.Count + " files / " + clientMissingFiles_listView.Items.Count + " missing)";
                murshunLauncherFiles_textBox.Text = "MurshunLauncherFiles.txt (" + murshunLauncherFiles_listView.Items.Count + " files / " + clientExcessFiles_listView.Items.Count + " excess)";

                if (clientMissingFiles_listView.Items.Count != 0 || clientExcessFiles_listView.Items.Count != 0)
                {
                    MessageBox.Show("You have missing or excess files.");
                    tabControl1.SelectedTab = tabPage2;
                    verifySuccess = false;
                }

                WebClient client = new WebClient();

                try
                {
                    string modLineString = client.DownloadString("http://vps.podkolpakom.net/launcher_files.php");

                    long totalSizeWeb = Convert.ToInt64(modLineString);

                    if (totalSizeWeb != totalSizeLocal)
                    {
                        MessageBox.Show("Your MurshunLauncherFiles.txt is not up-to-date. Launch BTsync to update.");
                        verifySuccess = false;
                    }
                }
                catch
                {

                }
            }
            else
            {
                MessageBox.Show("MurshunLauncherFiles.txt not found. Select your BTsync folder as Arma 3 Mods folder.");
                verifySuccess = false;
            }

            return verifySuccess;
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

        public bool CompareFolders()
        {
            bool compareSuccess = true;

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

            if (compareMissingFiles_listView.Items.Count != 0 || compareExcessFiles_listView.Items.Count != 0)
            {
                MessageBox.Show("You have missing or excess files.");
                tabControl1.SelectedTab = tabPage4;
                compareSuccess = false;
            }            

            return compareSuccess;
        }

        public bool CopyMissions()
        {
            bool copySuccess = true;

            string arma3ClientMissionFolder = pathToArma3Client_textBox.Text.ToLower().Replace("arma3.exe", "mpmissions");
            string arma3ServerMissionFolder = pathToArma3Server_textBox.Text.ToLower().Replace("arma3server.exe", "mpmissions");

            if (arma3ClientMissionFolder == arma3ServerMissionFolder)
                return true;

            try
            {
                List<string> clientMissionlist = Directory.GetFiles(arma3ClientMissionFolder, "*", SearchOption.AllDirectories).Where(s => s.Contains(".pbo")).ToList();

                foreach (string X in clientMissionlist)
                {
                    File.Copy(X, X.Replace(arma3ClientMissionFolder, arma3ServerMissionFolder), true);
                }
            }
            catch
            {
                MessageBox.Show("Error copying missions.");
                copySuccess = false;
            }

            return copySuccess;
        }

        private void CheckPath(string filePath)
        {
            List<string> pathList = Path.GetDirectoryName(filePath).Split(Path.DirectorySeparatorChar).ToList();

            string fullpath = pathList[0];

            pathList.RemoveAt(0);

            foreach (string X in pathList)
            {
                fullpath += "\\" + X;

                if (!Directory.Exists(fullpath))
                    Directory.CreateDirectory(fullpath);
            }
        }
    }
}
