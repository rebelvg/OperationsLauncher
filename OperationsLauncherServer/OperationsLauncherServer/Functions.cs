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
using SharedNamespace;

namespace OperationsLauncherServer
{
    public partial class Form1 : Form
    {
        LauncherConfigJson repoConfigJson = new LauncherConfigJson();

        public class LauncherSettingsJson
        {          
            public string arma3ExePath = Directory.GetCurrentDirectory() + "\\arma3server_x64.exe";
            public string repoFolderPath = Directory.GetCurrentDirectory();
            public string steamWorkshopFolderPath = Directory.GetCurrentDirectory() + "\\!Workshop";
            public string[] customMods = new string[0];
            public string[] checkedCustomMods = new string[0];
            public string serverConfig = "";
            public string serverCfg = "";
            public string serverProfiles = "";
            public string serverProfileName = "";
            public bool hideServerWindow = false;
            public bool removeNoLogs = false;
        }

        public void ReadXmlFile()
        {
            try
            {
                var LauncherSettingsJson = JsonConvert.DeserializeObject<LauncherSettingsJson>(File.ReadAllText(xmlPath_textBox.Text));

                pathToArma3_textBox.Text = LauncherSettingsJson.arma3ExePath;
                pathToMods_textBox.Text = LauncherSettingsJson.repoFolderPath;
                steamWorkshopFolderTextBox.Text = LauncherSettingsJson.steamWorkshopFolderPath;
                serverConfig_textBox.Text = LauncherSettingsJson.serverConfig;
                serverCfg_textBox.Text = LauncherSettingsJson.serverCfg;
                serverProfiles_textBox.Text = LauncherSettingsJson.serverProfiles;
                serverProfileName_textBox.Text = LauncherSettingsJson.serverProfileName;
                hideWindow_checkBox.Checked = LauncherSettingsJson.hideServerWindow;

                foreach (string X in LauncherSettingsJson.customMods)
                {
                    if (!customMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).Contains(X))
                    {
                        customMods_listView.Items.Add(X);
                    }
                }

                foreach (ListViewItem X in customMods_listView.Items)
                {
                    if (LauncherSettingsJson.checkedCustomMods.Contains(X.Text))
                    {
                        X.Checked = true;
                    }
                }

                if (LauncherSettingsJson.removeNoLogs) {
                    defaultStartLineServer_textBox.Text = defaultStartLineServer_textBox.Text.Replace(" -nologs", "");
                }
            }
            catch (Exception error)
            {
                DialogResult dialogResult = MessageBox.Show("Create a new one? " + error.Message, "Settings file is corrupted.", MessageBoxButtons.YesNo);

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
                var LauncherSettingsJson = new LauncherSettingsJson();

                LauncherSettingsJson.arma3ExePath = pathToArma3_textBox.Text;
                LauncherSettingsJson.repoFolderPath = pathToMods_textBox.Text;
                LauncherSettingsJson.steamWorkshopFolderPath = steamWorkshopFolderTextBox.Text;
                LauncherSettingsJson.customMods = customMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToArray();
                LauncherSettingsJson.checkedCustomMods = customMods_listView.CheckedItems.Cast<ListViewItem>().Select(x => x.Text).ToArray();
                LauncherSettingsJson.serverConfig = serverConfig_textBox.Text;
                LauncherSettingsJson.serverCfg = serverCfg_textBox.Text;
                LauncherSettingsJson.serverProfiles = serverProfiles_textBox.Text;
                LauncherSettingsJson.serverProfileName = serverProfileName_textBox.Text;
                LauncherSettingsJson.hideServerWindow = hideWindow_checkBox.Checked;
                LauncherSettingsJson.removeNoLogs = !defaultStartLineServer_textBox.Text.Contains("-nologs");

                string json = JsonConvert.SerializeObject(LauncherSettingsJson, Formatting.Indented);

                File.WriteAllText(xmlPath_textBox.Text, json);
            }
            catch (Exception error)
            {
                MessageBox.Show("Saving settings failed. " + error.Message);
            }
        }

        public bool ReadPresetFile()
        {
            string operationsLauncherFilesPath = pathToMods_textBox.Text + "\\OperationsLauncherFiles.json";

            if (!File.Exists(operationsLauncherFilesPath))
            {
                RefreshPresetModsList(false);

                MessageBox.Show("OperationsLauncherFiles.json not found. Select your BTsync folder as Arma 3 Mods folder.");

                return false;
            }

            repoConfigJson = JsonConvert.DeserializeObject<LauncherConfigJson>(File.ReadAllText(operationsLauncherFilesPath));

            RefreshPresetModsList(true);

            return true;
        }

        public void RefreshPresetModsList(bool btSyncFolderHasSyncFile)
        {
            SetColorOnText(pathToArma3_textBox);
            SetColorOnText(pathToMods_textBox, btSyncFolderHasSyncFile);
            SetColorOnText(steamWorkshopFolderTextBox);
            SetColorOnText(serverConfig_textBox);
            SetColorOnText(serverCfg_textBox);
            SetColorOnText(serverProfiles_textBox);

            SetColorOnPresetList(presetMods_listView, pathToMods_textBox.Text, steamWorkshopFolderTextBox.Text);

            SetColorOnCustomList(customMods_listView, columnHeader9);
        }

        public void SetColorOnText(TextBox box)
        {
            if (Directory.Exists(box.Text) || File.Exists(box.Text))
                box.BackColor = Color.Green;
            else
                box.BackColor = Color.Red;
        }

        public void SetColorOnText(TextBox box, bool btSyncFolderHasSyncFile)
        {
            if (btSyncFolderHasSyncFile)
                box.BackColor = Color.Green;
            else
                box.BackColor = Color.Red;
        }

        public void SetColorOnPresetList(ListView list, string path, string steamPath)
        {
            list.Items.Clear();

            foreach (string X in repoConfigJson.mods)
            {
                var listViewItem = list.Items.Add(X);

                if (Directory.Exists(path + "\\" + listViewItem.Text + "\\addons"))
                {
                    if (listViewItem.BackColor != Color.Green)
                        listViewItem.BackColor = Color.Green;
                }
                else
                {
                    if (listViewItem.BackColor != Color.Red)
                        listViewItem.BackColor = Color.Red;
                }
            }

            foreach (string X in repoConfigJson.steamMods)
            {
                var listViewItem = list.Items.Add(X);

                if (Directory.Exists(steamPath + "\\" + listViewItem.Text + "\\addons"))
                {
                    if (listViewItem.BackColor != Color.Green)
                        listViewItem.BackColor = Color.Green;
                }
                else
                {
                    if (listViewItem.BackColor != Color.Red)
                        listViewItem.BackColor = Color.Red;
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
            Shared.CheckSyncFolderSize(pathToMods_textBox.Text);

            if (!ReadPresetFile()) {
                return false;
            }                

            LockInterface("Verifying...");

            string operationsLauncherFilesPath = pathToMods_textBox.Text + "\\OperationsLauncherFiles.json";

            if (!await Task.Run(() => CheckLauncherFiles(repoConfigJson.verifyLink, Shared.GetMD5(operationsLauncherFilesPath, true)))) {
                UnlockInterface();

                return false;
            }

            List<string> folderFiles;
            List<string> steamFolderFiles;

            try
            {
                folderFiles = Shared.GetFolderFilesToHash(pathToMods_textBox.Text, repoConfigJson.mods);
                steamFolderFiles = Shared.GetFolderFilesToHash(steamWorkshopFolderTextBox.Text, repoConfigJson.steamMods);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);

                UnlockInterface();

                return false;
            }

            modsFiles_listView.Items.Clear();
            launcherFiles_listView.Items.Clear();

            var clientFiles = await Task.Run(() => GetVerifyList(folderFiles, steamFolderFiles, fullVerify));

            modsFiles_listView.BeginUpdate();

            foreach (string X in clientFiles)
            {
                modsFiles_listView.Items.Add(X);
            }

            modsFiles_listView.EndUpdate();

            launcherFiles_listView.BeginUpdate();

            List<string> allRepoFiles = repoConfigJson.files.Concat(repoConfigJson.steamFiles).Select(x => !fullVerify ? x.filePath + ":" + x.size : x.filePath + ":" + x.md5).ToList();

            foreach (string X in allRepoFiles)
            {
                launcherFiles_listView.Items.Add(X);
            }

            launcherFiles_listView.EndUpdate();

            List<string> missingFilesList = allRepoFiles.Where(x => !clientFiles.Contains(x)).ToList();
            List<string> excessFilesList = clientFiles.Where(x => !allRepoFiles.Contains(x)).ToList();

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
            launcherFiles_textBox.Text = "OperationsLauncherFiles.json (" + launcherFiles_listView.Items.Count + " files / " + excessFiles_listView.Items.Count + " excess)";

            if (missingFiles_listView.Items.Count != 0 || excessFiles_listView.Items.Count != 0)
            {
                if (tabControl1.SelectedTab != tabPage4)
                {
                    MessageBox.Show("You have missing or excess files.");
                }

                UnlockInterface();

                return false;
            }

            UnlockInterface();

            return true;
        }

        public List<string> ProcessFilesList(string sourceType, string baseFolder, List<string> filesList, bool fullVerify)
        {
            var chunkedList = Shared.SplitArrayIntoChunksOfLen(filesList, 4);

            List<string> clientFiles = new List<string>();

            foreach (List<string> chunkedFolderFiles in chunkedList)
            {
                var tasks = new List<Task<string>>();

                foreach (string X in chunkedFolderFiles)
                {
                    FileInfo file = new FileInfo(baseFolder + X);

                    ChangeHeader("Verifying... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");

                    var task = Task.Run(() =>
                    {
                        var filePath = @"\" + sourceType + X;

                        if (!fullVerify)
                            return filePath + ":" + file.Length;
                        else
                            return filePath + ":" + Shared.GetMD5(baseFolder + X, false);
                    });

                    tasks.Add(task);
                }                

                Task.WaitAll(tasks.ToArray());

                clientFiles.AddRange(tasks.Select(x => x.Result));

                BeginInvoke(new Action(() => progressBar1.Value += tasks.Count));
            }

            return clientFiles;
        }

        public IEnumerable<string> GetVerifyList(List<string> folderFiles, List<string> steamFolderFiles, bool fullVerify)
        {
            BeginInvoke(new Action(() => {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = folderFiles.Count() + steamFolderFiles.Count();
                progressBar1.Value = 0;
                progressBar1.Step = 1;
            }));

            var clientFiles = ProcessFilesList("repo", pathToMods_textBox.Text, folderFiles, fullVerify);
            var steamClientFiles = ProcessFilesList("steam", steamWorkshopFolderTextBox.Text, steamFolderFiles, fullVerify);

            return clientFiles.Concat(steamClientFiles);
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
                    Invoke(new Action(() => MessageBox.Show("Your OperationsLauncherFiles.json is not up-to-date. Launch BTsync to update.")));

                    ChangeHeader("Verify failed, hashes do not match.");

                    return false;
                }
            }
            catch (Exception error)
            {
                Invoke(new Action(() => MessageBox.Show("Couldn't connect to the server to check the integrity of your files. " + link + "\n\nError: " + error.Message)));

                ChangeHeader("Verify failed " + error.Message);

                return false;
            }

            ResetHeader();

            return true;
        }       

        public void DownloadMissions()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        WebClient client = new WebClient();

                        string response = client.DownloadString(repoConfigJson.missionsLink);

                        var missions = JsonConvert.DeserializeObject<IMissionResponse[]>(response);

                        foreach (var mission in missions)
                        {
                            string missionPath = Path.GetDirectoryName(pathToArma3_textBox.Text) + "/mpmissions/" + mission.file;

                            if (!File.Exists(missionPath) || Shared.GetMD5(missionPath, true) != mission.hash)
                            {
                                using (client = new WebClient())
                                {
                                    try
                                    {
                                        client.DownloadFile(repoConfigJson.missionsLink + "/" + mission.file, missionPath);

                                    }
                                    catch (Exception error)
                                    {
                                        Console.WriteLine(error.Message);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception error) {
                        Console.WriteLine(error.Message);
                    }

                    Thread.Sleep(30000);
                }
            });

            thread.IsBackground = true;

            thread.Start();
        }

        public void LockInterface(string text)
        {
            BeginInvoke(new Action(() =>
            {
                tabControl1.Enabled = false;
                ChangeHeader(text);
            }));
        }

        public void UnlockInterface()
        {
            BeginInvoke(new Action(() =>
            {
                tabControl1.Enabled = true;
                ChangeHeader("Operations Launcher Server");
            }));
        }

        public void ChangeHeader(string text)
        {
            BeginInvoke(new Action(() =>
            {
                this.Text = text;
            }));
        }

        public void ResetHeader()
        {
            BeginInvoke(new Action(() =>
            {
                ChangeHeader("Operations Launcher");
            }));
        }
    }
}
