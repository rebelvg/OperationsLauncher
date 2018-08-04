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

                pathToArma3_textBox.Text = LauncherSettings.pathToArma3Server_textBox;
                pathToMods_textBox.Text = LauncherSettings.pathToArma3ServerMods_textBox;
                serverConfig_textBox.Text = LauncherSettings.serverConfig_textBox;
                serverCfg_textBox.Text = LauncherSettings.serverCfg_textBox;
                serverProfiles_textBox.Text = LauncherSettings.serverProfiles_textBox;
                serverProfileName_textBox.Text = LauncherSettings.serverProfileName_textBox;
                hideWindow_checkBox.Checked = LauncherSettings.hideWindow_checkBox;

                foreach (string X in LauncherSettings.serverCustomMods_listView)
                {
                    if (!customMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).Contains(X))
                    {
                        customMods_listView.Items.Add(X);
                    }
                }

                foreach (ListViewItem X in customMods_listView.Items)
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

                LauncherSettings.pathToArma3Server_textBox = pathToArma3_textBox.Text;
                LauncherSettings.pathToArma3ServerMods_textBox = pathToMods_textBox.Text;
                LauncherSettings.serverCustomMods_listView = customMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();
                LauncherSettings.serverCheckedModsList_listView = customMods_listView.CheckedItems.Cast<ListViewItem>().Select(x => x.Text).ToList();
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

        public struct LauncherConfigJsonFile
        {
            public long size;
            public string date;
            public string md5;
        }

        public struct LauncherConfigJson
        {
            public string server;
            public string password;
            public string verify_link;
            public string missions_link;
            public string[] mods;
            public Dictionary<string, LauncherConfigJsonFile> files;
        }

        public bool ReadPresetFile()
        {
            string murshunLauncherFilesPath = pathToMods_textBox.Text + "\\MurshunLauncherFiles.json";

            presetModsList = new List<string>();

            if (!File.Exists(murshunLauncherFilesPath))
            {
                MessageBox.Show("MurshunLauncherFiles.json not found. Select your BTsync folder as Arma 3 Mods folder.");
                return false;
            }

            LauncherConfigJson json = JsonConvert.DeserializeObject<LauncherConfigJson>(File.ReadAllText(murshunLauncherFilesPath));

            presetModsList = json.mods.ToList();

            RefreshPresetModsList();

            return true;
        }

        public dynamic ReturnPresetFile()
        {
            string murshunLauncherFilesPath = pathToMods_textBox.Text + "\\MurshunLauncherFiles.json";

            dynamic json = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(murshunLauncherFilesPath));

            return json;
        }

        public void RefreshPresetModsList()
        {
            SetColorOnText(pathToArma3_textBox);
            SetColorOnText(pathToMods_textBox);
            SetColorOnText(serverConfig_textBox);
            SetColorOnText(serverCfg_textBox);
            SetColorOnText(serverProfiles_textBox);

            SetColorOnPresetList(presetMods_listView, pathToMods_textBox.Text);

            SetColorOnCustomList(customMods_listView, columnHeader9);
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

        public async Task<bool> VerifyMods(bool fullVerify)
        {
            if (!ReadPresetFile())
                return false;

            string murshunLauncherFilesPath = pathToMods_textBox.Text + "\\MurshunLauncherFiles.json";

            LauncherConfigJson json = JsonConvert.DeserializeObject<LauncherConfigJson>(File.ReadAllText(murshunLauncherFilesPath));

            if (!await Task.Run(() => CheckLauncherFiles(json.verify_link, GetMD5(murshunLauncherFilesPath))))
                return false;

            List<string> folderFiles = Directory.GetFiles(pathToMods_textBox.Text, "*", SearchOption.AllDirectories).ToList();

            folderFiles = folderFiles.Select(a => a.Replace(pathToMods_textBox.Text, "")).Select(b => b.ToLower()).ToList();

            folderFiles = folderFiles.Where(a => presetModsList.Any(b => a.StartsWith("\\" + b + "\\"))).Where(c => c.EndsWith(".pbo") || c.EndsWith(".dll")).ToList();

            modsFiles_listView.Items.Clear();
            launcherFiles_listView.Items.Clear();

            progressBar1.Minimum = 0;
            progressBar1.Maximum = folderFiles.Count();
            progressBar1.Value = 0;
            progressBar1.Step = 1;

            List<string> clientFiles = await Task.Run(() => GetVerifyList(folderFiles, fullVerify));

            foreach (string X in clientFiles)
            {
                modsFiles_listView.Items.Add(X);
            }

            foreach (KeyValuePair<string, LauncherConfigJsonFile> X in json.files)
            {
                long size = X.Value.size;
                string date = X.Value.date;
                string md5 = X.Value.md5;

                if (!fullVerify)
                    launcherFiles_listView.Items.Add(X.Key + ":" + size);
                else
                    launcherFiles_listView.Items.Add(X.Key + ":" + md5);
            }

            folderFiles = modsFiles_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();

            List<string> jsonFiles = launcherFiles_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();

            List<string> missingFilesList = jsonFiles.Where(x => !folderFiles.Contains(x)).ToList();
            List<string> excessFilesList = folderFiles.Where(x => !jsonFiles.Contains(x)).ToList();

            missingFiles_listView.Items.Clear();
            excessFiles_listView.Items.Clear();

            foreach (string X in missingFilesList)
            {
                missingFiles_listView.Items.Add(X);
            }

            foreach (string X in excessFilesList)
            {
                excessFiles_listView.Items.Add(X);
            }

            modsFiles_textBox.Text = pathToMods_textBox.Text + " (" + modsFiles_listView.Items.Count + " files / " + missingFiles_listView.Items.Count + " missing)";
            launcherFiles_textBox.Text = "MurshunLauncherFiles.json (" + launcherFiles_listView.Items.Count + " files / " + excessFiles_listView.Items.Count + " excess)";

            if (missingFiles_listView.Items.Count != 0 || excessFiles_listView.Items.Count != 0)
            {
                if (tabControl1.SelectedTab != tabPage4)
                {
                    MessageBox.Show("You have missing or excess files.");
                }

                return false;
            }

            return true;
        }

        public List<string> GetVerifyList(List<string> folderFiles, bool fullVerify)
        {
            LockInterface("Verifying...");

            List<string> clientFiles = new List<string>();

            foreach (string X in folderFiles)
            {
                FileInfo file = new FileInfo(pathToMods_textBox.Text + X);

                ChangeHeader("Verifying... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");

                if (!fullVerify)
                    clientFiles.Add(X + ":" + file.Length);
                else
                    clientFiles.Add(X + ":" + GetMD5(pathToMods_textBox.Text + X));

                Invoke(new Action(() => progressBar1.PerformStep()));

                ChangeHeader("Verifying... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
            }

            UnlockInterface();

            return clientFiles;
        }

        public bool CheckLauncherFiles(string link, string localJsonMD5)
        {
            if (string.IsNullOrEmpty(link))
                return true;

            ChangeHeader("Connecting to the server... " + link);

            WebClient client = new WebClient();

            try
            {
                string modLineString = client.DownloadString(link);

                if (modLineString != localJsonMD5)
                {
                    Invoke(new Action(() => MessageBox.Show("Your MurshunLauncherFiles.json is not up-to-date. Launch BTsync to update.")));
                    return false;
                }
            }
            catch
            {
                Invoke(new Action(() => MessageBox.Show("Couldn't connect to the server to check the integrity of your files. " + link)));
                return false;
            }

            return true;
        }

        public void CheckSyncFolderSize()
        {
            string archivePath = pathToMods_textBox.Text + "\\.sync\\Archive";

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
            Invoke(new Action(() =>
            {
                tabControl1.Enabled = false;
                ChangeHeader(text);
            }));
        }

        public void DownloadMissions()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        dynamic presetFile = ReturnPresetFile();

                        WebClient client = new WebClient();

                        string response = client.DownloadString((string)presetFile["missions_link"]);

                        List<dynamic> missions = JsonConvert.DeserializeObject<List<dynamic>>(response);

                        foreach (dynamic mission in missions)
                        {
                            string missionPath = Path.GetDirectoryName(pathToArma3_textBox.Text) + "/mpmissions/" + (string)mission["file"];

                            if (!File.Exists(missionPath) || GetMD5(missionPath) != (string)mission["hash"])
                            {
                                using (client = new WebClient())
                                {
                                    client.DownloadFile((string)presetFile["missions_link"] + "/" + (string)mission["file"], missionPath);
                                }
                            }
                        }
                    }
                    catch { }

                    Thread.Sleep(30000);
                }
            });

            thread.IsBackground = true;

            thread.Start();
        }

        public void UnlockInterface()
        {
            Invoke(new Action(() =>
            {
                tabControl1.Enabled = true;
                ChangeHeader("Operations Launcher Server");
            }));
        }

        public void ChangeHeader(string text)
        {
            Invoke(new Action(() =>
            {
                Text = text;
            }));
        }
    }
}
