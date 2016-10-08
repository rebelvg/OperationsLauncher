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
using System.Security.Cryptography;

namespace MurshunLauncherServer
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

                pathToArma3Server_textBox.Text = LauncherSettings.pathToArma3Server_textBox;
                pathToArma3ServerMods_textBox.Text = LauncherSettings.pathToArma3ServerMods_textBox;
                serverConfig_textBox.Text = LauncherSettings.serverConfig_textBox;
                serverCfg_textBox.Text = LauncherSettings.serverCfg_textBox;
                serverProfiles_textBox.Text = LauncherSettings.serverProfiles_textBox;
                serverProfileName_textBox.Text = LauncherSettings.serverProfileName_textBox;
                hideWindow_checkBox.Checked = LauncherSettings.hideWindow_checkBox;
                missionFolder_textBox.Text = LauncherSettings.missionFolder;
                copyMissions_checkBox.Checked = LauncherSettings.copyMissions_checkBox;

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

                LauncherSettings.pathToArma3Server_textBox = pathToArma3Server_textBox.Text;
                LauncherSettings.pathToArma3ServerMods_textBox = pathToArma3ServerMods_textBox.Text;
                LauncherSettings.serverCustomMods_listView = serverCustomMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();
                LauncherSettings.serverCheckedModsList_listView = serverCustomMods_listView.CheckedItems.Cast<ListViewItem>().Select(x => x.Text).ToList();
                LauncherSettings.serverConfig_textBox = serverConfig_textBox.Text;
                LauncherSettings.serverCfg_textBox = serverCfg_textBox.Text;
                LauncherSettings.serverProfiles_textBox = serverProfiles_textBox.Text;
                LauncherSettings.serverProfileName_textBox = serverProfileName_textBox.Text;
                LauncherSettings.hideWindow_checkBox = hideWindow_checkBox.Checked;
                LauncherSettings.missionFolder = missionFolder_textBox.Text;
                LauncherSettings.copyMissions_checkBox = copyMissions_checkBox.Checked;

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
            string murshunLauncherFilesPath = pathToArma3ServerMods_textBox.Text + "\\MurshunLauncherFiles.json";

            if (File.Exists(murshunLauncherFilesPath))
            {
                dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(murshunLauncherFilesPath));

                presetModsList = json.mods.ToObject<List<string>>();
            }

            RefreshPresetModsList();
        }

        public void RefreshPresetModsList()
        {
            SetColorOnText(pathToArma3Server_textBox);
            SetColorOnText(pathToArma3ServerMods_textBox);
            SetColorOnText(serverConfig_textBox);
            SetColorOnText(serverCfg_textBox);
            SetColorOnText(serverProfiles_textBox);
            SetColorOnText(missionFolder_textBox);

            SetColorOnPresetList(serverPresetMods_listView, pathToArma3ServerMods_textBox.Text);

            SetColorOnCustomList(serverCustomMods_listView, columnHeader9);
        }

        public void SetColorOnText(TextBox box)
        {
            if (Directory.Exists(box.Text) || File.Exists(box.Text))
                box.BackColor = Color.Green;
            else
                box.BackColor = Color.Red;
        }

        public void SetColorOnPresetList(ListView list, string path)
        {
            list.Items.Clear();

            foreach (string X in presetModsList)
            {
                list.Items.Add(X);
            }

            foreach (ListViewItem X in list.Items)
            {
                if (Directory.Exists(path + "\\" + X.Text + "\\addons"))
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
        }

        public void SetColorOnCustomList(ListView list, ColumnHeader header)
        {
            foreach (ListViewItem X in list.Items)
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

            header.Width = -2;
        }

        public bool VerifyMods(bool fullVerify)
        {
            bool verifySuccess = true;

            LockInterface("Verifying...");

            string murshunLauncherFilesPath = pathToArma3ServerMods_textBox.Text + "\\MurshunLauncherFiles.json";

            if (File.Exists(murshunLauncherFilesPath))
            {
                Dictionary<string, dynamic> json = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(murshunLauncherFilesPath));

                verifySuccess = CheckLauncherFiles(json["verify_link"], GetMD5(murshunLauncherFilesPath));

                List<string> folderFiles = Directory.GetFiles(pathToArma3ServerMods_textBox.Text, "*", SearchOption.AllDirectories).ToList();

                folderFiles = folderFiles.Select(a => a.Replace(pathToArma3ServerMods_textBox.Text, "")).Select(b => b.ToLower()).ToList();

                folderFiles = folderFiles.Where(a => presetModsList.Any(b => a.StartsWith("\\" + b + "\\"))).Where(c => c.EndsWith(".pbo") || c.EndsWith(".dll")).ToList();

                this.Invoke(new Action(() =>
                {
                    clientModsFiles_listView.Items.Clear();
                    murshunLauncherFiles_listView.Items.Clear();

                    progressBar2.Minimum = 0;
                    progressBar2.Maximum = folderFiles.Count();
                    progressBar2.Value = 0;
                    progressBar2.Step = 1;
                }));

                List<string> clientFiles = new List<string>();

                foreach (string X in folderFiles)
                {
                    FileInfo file = new FileInfo(pathToArma3ServerMods_textBox.Text + X);

                    ChangeHeader("Verifying... (" + progressBar2.Value + "/" + progressBar2.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");

                    if (!fullVerify)
                        clientFiles.Add(X + ":" + file.Length);
                    else
                        clientFiles.Add(X + ":" + GetMD5(pathToArma3ServerMods_textBox.Text + X));

                    this.Invoke(new Action(() => progressBar2.PerformStep()));

                    ChangeHeader("Verifying... (" + progressBar2.Value + "/" + progressBar2.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
                }

                this.Invoke(new Action(() =>
                {
                    foreach (string X in clientFiles)
                    {
                        clientModsFiles_listView.Items.Add(X);
                    }

                    foreach (dynamic X in json["files"])
                    {
                        dynamic size = X.First.size;
                        dynamic date = X.First.date;
                        dynamic md5 = X.First.md5;

                        if (!fullVerify)
                            murshunLauncherFiles_listView.Items.Add(X.Name + ":" + size);
                        else
                            murshunLauncherFiles_listView.Items.Add(X.Name + ":" + md5);
                    }

                    folderFiles = clientModsFiles_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();

                    List<string> jsonFiles = murshunLauncherFiles_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();

                    List<string> missingFilesList = jsonFiles.Where(x => !folderFiles.Contains(x)).ToList();
                    List<string> excessFilesList = folderFiles.Where(x => !jsonFiles.Contains(x)).ToList();

                    clientMissingFiles_listView.Items.Clear();
                    clientExcessFiles_listView.Items.Clear();

                    foreach (string X in missingFilesList)
                    {
                        clientMissingFiles_listView.Items.Add(X);
                    }

                    foreach (string X in excessFilesList)
                    {
                        clientExcessFiles_listView.Items.Add(X);
                    }

                    clientMods_textBox.Text = "Server Mods (" + clientModsFiles_listView.Items.Count + " files / " + clientMissingFiles_listView.Items.Count + " missing)";
                    murshunLauncherFiles_textBox.Text = "MurshunLauncherFiles.json (" + murshunLauncherFiles_listView.Items.Count + " files / " + clientExcessFiles_listView.Items.Count + " excess)";

                    if (clientMissingFiles_listView.Items.Count != 0 || clientExcessFiles_listView.Items.Count != 0)
                    {
                        if (tabControl1.SelectedTab != tabPage4)
                        {
                            MessageBox.Show("You have missing or excess files.");
                            tabControl1.SelectedTab = tabPage4;
                            verifySuccess = false;
                        }
                    }
                }));
            }
            else
            {
                MessageBox.Show("MurshunLauncherFiles.json not found. Select your BTsync folder as Arma 3 Mods folder.");
                verifySuccess = false;
            }

            UnlockInterface();

            return verifySuccess;
        }

        public bool CheckLauncherFiles(string link, string localJsonMD5)
        {
            bool success = false;

            if (link == "")
                return true;

            ChangeHeader("Connecting to the server... " + link);

            WebClient client = new WebClient();

            try
            {
                string modLineString = client.DownloadString(link);

                if (modLineString != localJsonMD5)
                {
                    this.Invoke(new Action(() => MessageBox.Show("Your MurshunLauncherFiles.json is not up-to-date. Launch BTsync to update.")));
                }
                else
                {
                    success = true;
                }
            }
            catch
            {
                this.Invoke(new Action(() => MessageBox.Show("Couldn't connect to the server to check the integrity of your files. " + link)));
            }

            return success;
        }

        public bool CopyMissions(bool auto)
        {
            bool copySuccess = true;

            if (auto && !copyMissions_checkBox.Checked)
                return true;

            string arma3ClientMissionFolder = missionFolder_textBox.Text.ToLower();
            string arma3ServerMissionFolder = pathToArma3Server_textBox.Text.ToLower().Replace("arma3server.exe", "mpmissions");

            if (arma3ClientMissionFolder == arma3ServerMissionFolder)
                return true;

            try
            {
                List<string> clientMissionList = Directory.GetFiles(arma3ClientMissionFolder, "*", SearchOption.TopDirectoryOnly).Where(s => s.Contains(".pbo")).ToList();

                foreach (string X in clientMissionList)
                {
                    File.Copy(X, X.Replace(arma3ClientMissionFolder, arma3ServerMissionFolder), true);
                }

                if (!auto)
                    MessageBox.Show(clientMissionList.Count + " missions synced.");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error copying missions.\n\n" + e);
                copySuccess = false;
            }

            return copySuccess;
        }

        public void CheckSyncFolderSize()
        {
            string archivePath = pathToArma3ServerMods_textBox.Text + "\\.sync\\Archive";

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
                    MessageBox.Show("Your BTsync archive folder is too large. It's size is over " + (bytes / 1024 / 1024 / 1024) + " GB. You can clear it and disable archiving in the BTsync client.");
                    System.Diagnostics.Process.Start(archivePath);
                }
            }
        }

        public string GetMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }
        }

        public void LockInterface(string text)
        {
            this.Invoke(new Action(() =>
            {
                this.Enabled = false;
                ChangeHeader(text);
            }));
        }

        public void UnlockInterface()
        {
            this.Invoke(new Action(() =>
            {
                this.Enabled = true;
                ChangeHeader("Murshun Launcher Server");
            }));
        }

        public void ChangeHeader(string text)
        {
            this.Invoke(new Action(() =>
            {
                this.Text = text;
            }));
        }
    }
}
