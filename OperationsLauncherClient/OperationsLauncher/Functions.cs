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

namespace OperationsLauncher
{
    public partial class Form1 : Form
    {
        LauncherConfigJson repoConfigJson = new LauncherConfigJson();

        public class LauncherSettingsJson
        {
            public string arma3ExePath = Directory.GetCurrentDirectory() + "\\arma3_x64.exe";
            public string repoFolderPath = Directory.GetCurrentDirectory();
            public string steamWorkshopFolderPath = Directory.GetCurrentDirectory() + "\\!Workshop";
            public bool joinServerOnLaunch = false;
            public string[] customMods = new string[0];
            public string[] checkedCustomMods = new string[0];
            public string advancedStartLine = "";
            public string teamSpeakAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TS3Client";
        }

        public void ReadXmlFile()
        {
            try
            {
                var LauncherSettingsJson = JsonConvert.DeserializeObject<LauncherSettingsJson>(File.ReadAllText(xmlPath_textBox.Text));

                pathToArma3_textBox.Text = LauncherSettingsJson.arma3ExePath;
                pathToMods_textBox.Text = LauncherSettingsJson.repoFolderPath;
                steamWorkshopFolderTextBox.Text = LauncherSettingsJson.steamWorkshopFolderPath;
                joinTheServer_checkBox.Checked = LauncherSettingsJson.joinServerOnLaunch;
                advancedStartLine_textBox.Text = LauncherSettingsJson.advancedStartLine;
                teamSpeakFolder_textBox.Text = LauncherSettingsJson.teamSpeakAppDataFolder;

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
                LauncherSettingsJson.joinServerOnLaunch = joinTheServer_checkBox.Checked;
                LauncherSettingsJson.customMods = customMods_listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToArray();
                LauncherSettingsJson.checkedCustomMods = customMods_listView.CheckedItems.Cast<ListViewItem>().Select(x => x.Text).ToArray();
                LauncherSettingsJson.advancedStartLine = advancedStartLine_textBox.Text;
                LauncherSettingsJson.teamSpeakAppDataFolder = teamSpeakFolder_textBox.Text;

                string json = JsonConvert.SerializeObject(LauncherSettingsJson, Formatting.Indented);

                File.WriteAllText(xmlPath_textBox.Text, json);
            }
            catch (Exception error)
            {
                MessageBox.Show("Saving settings failed. " + error.Message);
            }
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

            try {
                folderFiles = Shared.GetFolderFilesToHash(pathToMods_textBox.Text, repoConfigJson.mods);

                steamFolderFiles = Shared.GetFolderFilesToHash(steamWorkshopFolderTextBox.Text, repoConfigJson.steamMods);
            }
            catch (Exception error) {
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

            foreach (LauncherConfigJsonFile X in repoConfigJson.files.Concat(repoConfigJson.steamFiles))
            {
                long size = X.size;
                string date = X.date;
                string md5 = X.md5;

                if (!fullVerify)
                    launcherFiles_listView.Items.Add(X.filePath + ":" + size);
                else
                    launcherFiles_listView.Items.Add(X.filePath + ":" + md5);
            }

            launcherFiles_listView.EndUpdate();

            List<string> allRepoFiles = repoConfigJson.files.Concat(repoConfigJson.steamFiles).Select(x => x.filePath).ToList();
            List<string> allLocalFiles = folderFiles.Concat(steamFolderFiles).ToList();

            List<string> missingFilesList = allRepoFiles.Where(x => !allLocalFiles.Contains(x)).ToList();
            List<string> excessFilesList = allLocalFiles.Where(x => !allRepoFiles.Contains(x)).ToList();

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
                if (tabControl1.SelectedTab != tabPage2)
                {
                    MessageBox.Show("You have missing or excess files.");
                }

                UnlockInterface();

                return false;
            }

            if (!CheckACRE2()) {
                UnlockInterface();

                return false;
            }                

            UnlockInterface();

            return true;
        }

        public List<string> ProcessFilesList(string baseFolder, List<string> filesList, bool fullVerify)
        {
            var chunkedList = new List<List<string>>();

            for (int i = 0; i < filesList.Count; i += 4)
            {
                chunkedList.Add(filesList.GetRange(i, Math.Min(4, filesList.Count - i)));
            }

            var tasks = new List<Task>();

            List<string> clientFiles = new List<string>();

            foreach (List<string> chunkedFolderFiles in chunkedList)
            {
                var task = Task.Run(() => {
                    foreach (string X in chunkedFolderFiles)
                    {
                        FileInfo file = new FileInfo(baseFolder + X);

                        ChangeHeader("Verifying... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");

                        if (!fullVerify)
                            clientFiles.Add(X + ":" + file.Length);
                        else
                            clientFiles.Add(X + ":" + Shared.GetMD5(baseFolder + X, false));

                        Invoke(new Action(() => progressBar1.PerformStep()));

                        ChangeHeader("Verifying... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
                    }
                });

                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            return clientFiles;
        }

        public IEnumerable<string> GetVerifyList(List<string> folderFiles, List<string> steamFolderFiles, bool fullVerify)
        {
            Invoke(new Action(() => {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = folderFiles.Count() + steamFolderFiles.Count();
                progressBar1.Value = 0;
                progressBar1.Step = 1;
            }));

            var clientFiles = ProcessFilesList(pathToMods_textBox.Text, folderFiles, fullVerify);
            var steamClientFiles = ProcessFilesList(steamWorkshopFolderTextBox.Text, steamFolderFiles, fullVerify);

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
            catch(Exception error)
            {
                Invoke(new Action(() => MessageBox.Show("Couldn't connect to the server to check the integrity of your files. " + link + "\n\nError: " + error.Message)));

                ChangeHeader("Verify failed " + error.Message);

                return false;
            }

            ResetHeader();

            return true;
        }

        public void RefreshPresetModsList(bool btSyncFolderHasSyncFile)
        {
            SetColorOnText(pathToArma3_textBox);
            SetColorForBtSyncFolder(pathToMods_textBox, btSyncFolderHasSyncFile);
            SetColorOnText(steamWorkshopFolderTextBox);
            SetColorOnText(teamSpeakFolder_textBox);

            SetColorOnPresetList(presetMods_listView, pathToMods_textBox.Text, steamWorkshopFolderTextBox.Text);

            SetColorOnCustomList(customMods_listView, columnHeader7);
        }

        public void SetColorOnText(TextBox box)
        {
            if (Directory.Exists(box.Text) || File.Exists(box.Text))
                box.BackColor = Color.Green;
            else
                box.BackColor = Color.Red;
        }

        public void SetColorForBtSyncFolder(TextBox box, bool btSyncFolderHasSyncFile = false) {
            if (btSyncFolderHasSyncFile) {
                box.BackColor = Color.Green;
            } else {
                box.BackColor = Color.Red;
            }
        }

        public void SetColorOnPresetList(ListView list, string path, string steamPath)
        {
            list.Items.Clear();

            foreach (string X in repoConfigJson.mods)
            {
                list.Items.Add(X);
            }

            foreach (string X in repoConfigJson.steamMods)
            {
                list.Items.Add(X);
            }

            foreach (ListViewItem X in list.Items)
            {
                if (Directory.Exists(path + "\\" + X.Text + "\\addons") || Directory.Exists(steamPath + "\\" + X.Text + "\\addons"))
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

        public bool CheckACRE2()
        {
            string acre32plugin = @"\acre2_win32.dll";
            string acre64plugin = @"\acre2_win64.dll";

            string acre32mods = pathToMods_textBox.Text + @"\@acre2\plugin" + acre32plugin;
            string acre64mods = pathToMods_textBox.Text + @"\@acre2\plugin" + acre64plugin;

            if (!File.Exists(acre32mods) && !File.Exists(acre64mods))
                return true;

            if (!Directory.Exists(teamSpeakFolder_textBox.Text + @"\plugins"))
            {
                MessageBox.Show("Can't find your TS plugins folder to automatically copy ACRE2 plugins in.");
                return false;
            }

            return CopyPlugins(teamSpeakFolder_textBox.Text + @"\plugins");
        }

        private bool CopyPlugins(string tspath)
        {
            string acre32plugin = @"\acre2_win32.dll";
            string acre64plugin = @"\acre2_win64.dll";

            string acre32mods = pathToMods_textBox.Text + @"\@acre2\plugin" + acre32plugin;
            string acre64mods = pathToMods_textBox.Text + @"\@acre2\plugin" + acre64plugin;

            if (File.Exists(tspath + acre32plugin) && File.Exists(tspath + acre64plugin) && Shared.GetMD5(tspath + acre32plugin, true) == Shared.GetMD5(acre32mods, true) && Shared.GetMD5(tspath + acre64plugin, true) == Shared.GetMD5(acre64mods, true))
                return true;

            try
            {
                File.Copy(acre32mods, tspath + acre32plugin, true);
                File.Copy(acre64mods, tspath + acre64plugin, true);

                MessageBox.Show("New ACRE2 plugins successfully copied into your TS folder.");
            }
            catch (Exception e)
            {
                MessageBox.Show("Can't overwrite ACRE2 plugins.\n" + e.Message);
                return false;
            }

            return true;
        }

        public void LockInterface(string text)
        {
            Invoke(new Action(() =>
            {
                tabControl1.Enabled = false;
                ChangeHeader(text);
            }));
        }

        public void UnlockInterface()
        {
            Invoke(new Action(() =>
            {
                tabControl1.Enabled = true;
                ChangeHeader("Operations Launcher");
            }));
        }

        public void ChangeHeader(string text)
        {
            Invoke(new Action(() =>
            {
                this.Text = text;
            }));
        }

        public void ResetHeader() {
            Invoke(new Action(() =>
            {
                ChangeHeader("Operations Launcher");
            }));
        }
    }
}
