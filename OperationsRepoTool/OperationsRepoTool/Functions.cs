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

namespace OperationsRepoTool
{
    public partial class Form1 : Form
    {
        RepoConfigJson repoConfigJson = new RepoConfigJson();

        public class RepoToolSettingsJson
        {
            public string repoConfigPath = Directory.GetCurrentDirectory() + "\\OperationsRepoToolConfig.json";
        }

        public void ReadXmlFile()
        {
            try
            {
                var RepoToolSettingsJson = JsonConvert.DeserializeObject<RepoToolSettingsJson>(File.ReadAllText(xmlPath_textBox.Text));

                repoConfigPath_textBox.Text = RepoToolSettingsJson.repoConfigPath;               
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
                var RepoToolSettingsJson = new RepoToolSettingsJson();

                RepoToolSettingsJson.repoConfigPath = repoConfigPath_textBox.Text;

                string json = JsonConvert.SerializeObject(RepoToolSettingsJson, Formatting.Indented);

                File.WriteAllText(xmlPath_textBox.Text, json);
            }
            catch (Exception error)
            {
                MessageBox.Show("Saving settings failed. " + error.Message);
            }
        }        

        public bool ReadPresetFile()
        {
            string operationsLauncherFilesPath = repoConfigPath_textBox.Text;

            try
            {
                RepoConfigJson json = JsonConvert.DeserializeObject<RepoConfigJson>(File.ReadAllText(operationsLauncherFilesPath));

                repoConfigJson = json;

                compareClientMods_textBox.Text = !String.IsNullOrEmpty(repoConfigJson.modsFolder ) ? repoConfigJson.modsFolder : "N/A";
                compareServerMods_textBox.Text = !String.IsNullOrEmpty(repoConfigJson.syncFolder) ? repoConfigJson.syncFolder : "N/A";
            }
            catch (Exception e)
            {
                RefreshPresetModsList();

                MessageBox.Show("There was an error reading the repo config.\n" + e.Message);

                return false;
            }

            RefreshPresetModsList();

            return true;
        }

        public bool SetLauncherFiles(string localJsonMD5)
        {
            if (string.IsNullOrEmpty(repoConfigJson.verifyLink)) {
                return true;
            }               

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("auth", repoConfigJson.verifyPassword);
                    client.Headers.Add("hash", localJsonMD5);

                    client.UploadString(repoConfigJson.verifyLink, "POST");
                }
            }
            catch (Exception e)
            {
                Invoke(new Action(() => MessageBox.Show("There was an error on accessing the server.\n" + e.Message)));
                return false;
            }

            return true;
        }

        public void RefreshPresetModsList()
        {
            SetColorOnText(repoConfigPath_textBox);

            SetColorOnPresetList(presetMods_listView, repoConfigJson.modsFolder, repoConfigJson.steamModsFolder);
        }

        public void SetColorOnText(TextBox box)
        {
            if (Directory.Exists(box.Text) || File.Exists(box.Text))
                box.BackColor = Color.Green;
            else
                box.BackColor = Color.Red;
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

        public bool CompareFolders()
        {
            if (!Directory.Exists(repoConfigJson.modsFolder) || !Directory.Exists(repoConfigJson.syncFolder))
            {
                MessageBox.Show("Server or Sync folder doesn't exist.");
                return false;
            }

            if (!ReadPresetFile()) {
                return false;
            }                

            List<string> folder_clientFilesList = Directory.GetFiles(repoConfigJson.modsFolder, "*", SearchOption.AllDirectories).ToList();

            folder_clientFilesList = folder_clientFilesList.Select(a => a.Replace(repoConfigJson.modsFolder, "")).Select(b => b.ToLower()).ToList();

            folder_clientFilesList = folder_clientFilesList.Where(a => repoConfigJson.mods.Any(b => a.StartsWith("\\" + b + "\\"))).ToList();

            List<string> folder_serverFilesList = Directory.GetFiles(repoConfigJson.syncFolder, "*", SearchOption.AllDirectories).ToList();

            folder_serverFilesList = folder_serverFilesList.Select(a => a.Replace(repoConfigJson.syncFolder, "")).Select(b => b.ToLower()).ToList();

            folder_serverFilesList = folder_serverFilesList.Where(a => repoConfigJson.mods.Any(b => a.StartsWith("\\" + b + "\\"))).ToList();

            compareClientFiles_listView.Items.Clear();
            compareServerFiles_listView.Items.Clear();

            progressBar1.Minimum = 0;
            progressBar1.Maximum = folder_clientFilesList.Count() + folder_serverFilesList.Count();
            progressBar1.Value = 0;
            progressBar1.Step = 1;

            LockInterface("Comparing...");

            foreach (string X in folder_clientFilesList)
            {
                FileInfo file = new FileInfo(repoConfigJson.modsFolder + X);
                compareClientFiles_listView.Items.Add(X + ":" + file.Length + ":" + file.LastWriteTimeUtc);

                progressBar1.PerformStep();

                ChangeHeader("Comparing... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
            }

            foreach (string X in folder_serverFilesList)
            {
                FileInfo file = new FileInfo(repoConfigJson.syncFolder + X);
                compareServerFiles_listView.Items.Add(X + ":" + file.Length + ":" + file.LastWriteTimeUtc);

                progressBar1.PerformStep();

                ChangeHeader("Comparing... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
            }

            UnlockInterface();

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

            compareClientMods_textBox.Text = "Mods Folder (" + compareClientFiles_listView.Items.Count + " files / " + compareMissingFiles_listView.Items.Count + " missing)";
            compareServerMods_textBox.Text = "Sync Folder (" + compareServerFiles_listView.Items.Count + " files / " + compareExcessFiles_listView.Items.Count + " excess)";

            if (compareMissingFiles_listView.Items.Count != 0 || compareExcessFiles_listView.Items.Count != 0) {
                return false;
            }                

            return true;
        }

        private void CheckPath(string filePath)
        {
            List<string> pathList = Path.GetDirectoryName(filePath).Split(Path.DirectorySeparatorChar).ToList();

            string fullPath = pathList[0];

            pathList.RemoveAt(0);

            foreach (string X in pathList)
            {
                fullPath += "\\" + X;

                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
            }
        }

        public void SaveLauncherFiles(string json_new)
        {
            try
            {
                File.WriteAllText(repoConfigJson.modsFolder + "\\OperationsLauncherFiles.json", json_new);

                if (Directory.Exists(repoConfigJson.syncFolder) && repoConfigJson.syncFolder.ToLower() != repoConfigJson.modsFolder.ToLower())
                    File.WriteAllText(repoConfigJson.syncFolder + "\\OperationsLauncherFiles.json", json_new);

                MessageBox.Show("OperationsLauncherFiles.json was saved.");
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error on saving of OperationsLauncherFiles.json.\n\n" + e.Message);
            }
        }

        public async void CreateVerifyFile()
        {
            if (!ReadPresetFile()) {
                return;
            }

            LockInterface("Building Verify File...");

            List<string> folderFiles;

            List<string> steamFolderFiles;

            try {
                folderFiles = Shared.GetFolderFilesToHash(repoConfigJson.modsFolder, repoConfigJson.mods);

                steamFolderFiles = Shared.GetFolderFilesToHash(repoConfigJson.steamModsFolder, repoConfigJson.steamMods);
            }
            catch (Exception error) {
                MessageBox.Show(error.Message);

                UnlockInterface();

                return;
            }            

            LauncherConfigJson json = new LauncherConfigJson();

            json.serverHost = repoConfigJson.serverHost;
            json.serverPassword = repoConfigJson.serverPassword;
            json.verifyLink = repoConfigJson.verifyLink;
            json.missionsLink = repoConfigJson.missionsLink;
            json.mods = repoConfigJson.mods;
            json.steamMods = repoConfigJson.steamMods;
            json.files = new List<LauncherConfigJsonFile>();
            json.steamFiles = new List<LauncherConfigJsonFile>();

            LauncherConfigJson json_old = new LauncherConfigJson() {
                mods = new string[0],
                files = new List<LauncherConfigJsonFile>(),
                steamFiles = new List<LauncherConfigJsonFile>()
            };

            string operationsLauncherFilesPath = repoConfigJson.modsFolder + "\\OperationsLauncherFiles.json";

            if (File.Exists(operationsLauncherFilesPath)) {
                try {
                    json_old = JsonConvert.DeserializeObject<LauncherConfigJson>(File.ReadAllText(operationsLauncherFilesPath));

                }
                catch (Exception error) {
                    Console.WriteLine(error.Message);
                }
            }

            json = await Task.Run(() => BuildVerifyList(folderFiles, steamFolderFiles, json_old, json));

            string json_new = JsonConvert.SerializeObject(json, Formatting.Indented);

            SetLauncherFiles(Shared.GetMD5FromBuffer(json_new));

            SaveLauncherFiles(json_new);

            UnlockInterface();
        }

        public List<LauncherConfigJsonFile> ProcessFilesList(string baseFolder, List<string> filesList, List<LauncherConfigJsonFile> oldFilesConfig) {
            var chunkedList = new List<List<string>>();

            for (int i = 0; i < filesList.Count; i += 4)
            {
                chunkedList.Add(filesList.GetRange(i, Math.Min(4, filesList.Count - i)));
            }

            var tasks = new List<Task>();

            var files = new List<LauncherConfigJsonFile>();

            foreach (List<string> chunkedFolderFiles in chunkedList)
            {
                foreach (string X in chunkedFolderFiles)
                {
                    var task = Task.Run(() => {
                        FileInfo file = new FileInfo(baseFolder + X);

                        ChangeHeader("Reading... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");

                        LauncherConfigJsonFile data = new LauncherConfigJsonFile();

                        data.filePath = X;
                        data.size = file.Length;
                        data.date = Shared.GetUnixTime(file.LastWriteTimeUtc).ToString();

                        try
                        {
                            var fileObj = oldFilesConfig.First(x => x.filePath == X);

                            if (fileObj.date == Shared.GetUnixTime(file.LastWriteTimeUtc).ToString())
                                data.md5 = fileObj.md5;
                            else
                                throw new Exception("need_to_refresh_md5");
                        }
                        catch
                        {
                            data.md5 = Shared.GetMD5(baseFolder + X, false);
                        }

                        files.Add(data);

                        Invoke(new Action(() => progressBar1.PerformStep()));

                        ChangeHeader("Reading... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
                    });

                    tasks.Add(task);
                }
            }

            Task.WaitAll(tasks.ToArray());

            return files;
        }

        public LauncherConfigJson BuildVerifyList(List<string> folderFiles, List<string> steamFolderFiles, LauncherConfigJson json_old, LauncherConfigJson json)
        {
            Invoke(new Action(() => {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = folderFiles.Count() + steamFolderFiles.Count();
                progressBar1.Value = 0;
                progressBar1.Step = 1;
            }));

            json.files = ProcessFilesList(repoConfigJson.modsFolder, folderFiles, json_old.files);
            json.steamFiles = ProcessFilesList(repoConfigJson.steamModsFolder, steamFolderFiles, json_old.steamFiles);

            return json;
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
                ChangeHeader("Operations Repo Tool");
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
