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

                repoConfigPath_textBox.Text = LauncherSettings.modListLink;
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

                LauncherSettings.modListLink = repoConfigPath_textBox.Text;

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

        public struct RepoConfigJson
        {
            public string server;
            public string password;
            public string verify_link;
            public string verify_password;
            public string missions_link;
            public string[] mods;
            public string mods_folder;
            public string sync_folder;
        }

        public bool ReadPresetFile()
        {
            string murshunLauncherFilesPath = repoConfigPath_textBox.Text;

            presetModsList = new List<string>();

            try
            {
                RepoConfigJson json = JsonConvert.DeserializeObject<RepoConfigJson>(File.ReadAllText(murshunLauncherFilesPath));

                presetModsList = json.mods.ToList();

                server = json.server;
                password = json.password;
                verifyModsLink = json.verify_link;
                verifyModsPassword = json.verify_password;
                missionsLink = json.missions_link;
                pathToModsFolder_textBox.Text = json.mods_folder;
                pathToSyncFolder_textBox.Text = json.sync_folder;
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error reading the repo config.\n" + e.Message);
                return false;
            }

            RefreshPresetModsList();

            return true;
        }

        public bool SetLauncherFiles(string localJsonMD5)
        {
            if (string.IsNullOrEmpty(verifyModsLink))
                return true;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("auth", verifyModsPassword);
                    client.Headers.Add("hash", localJsonMD5);

                    client.UploadString(verifyModsLink, "POST");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                Invoke(new Action(() => MessageBox.Show("There was an error on accessing the server.\n" + verifyModsLink)));
                return false;
            }

            return true;
        }

        public void RefreshPresetModsList()
        {
            SetColorOnText(pathToSyncFolder_textBox);
            SetColorOnText(pathToModsFolder_textBox);
            SetColorOnText(repoConfigPath_textBox);

            SetColorOnPresetList(presetMods_listView, pathToModsFolder_textBox.Text);
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

        public bool CompareFolders()
        {
            if (!Directory.Exists(pathToModsFolder_textBox.Text) || !Directory.Exists(pathToSyncFolder_textBox.Text))
            {
                MessageBox.Show("Server or Sync folder doesn't exist.");
                return false;
            }

            if (!ReadPresetFile())
                return false;

            List<string> folder_clientFilesList = Directory.GetFiles(pathToModsFolder_textBox.Text, "*", SearchOption.AllDirectories).ToList();

            folder_clientFilesList = folder_clientFilesList.Select(a => a.Replace(pathToModsFolder_textBox.Text, "")).Select(b => b.ToLower()).ToList();

            folder_clientFilesList = folder_clientFilesList.Where(a => presetModsList.Any(b => a.StartsWith("\\" + b + "\\"))).ToList();

            List<string> folder_serverFilesList = Directory.GetFiles(pathToSyncFolder_textBox.Text, "*", SearchOption.AllDirectories).ToList();

            folder_serverFilesList = folder_serverFilesList.Select(a => a.Replace(pathToSyncFolder_textBox.Text, "")).Select(b => b.ToLower()).ToList();

            folder_serverFilesList = folder_serverFilesList.Where(a => presetModsList.Any(b => a.StartsWith("\\" + b + "\\"))).ToList();

            compareClientFiles_listView.Items.Clear();
            compareServerFiles_listView.Items.Clear();

            progressBar1.Minimum = 0;
            progressBar1.Maximum = folder_clientFilesList.Count() + folder_serverFilesList.Count();
            progressBar1.Value = 0;
            progressBar1.Step = 1;

            LockInterface("Comparing...");

            foreach (string X in folder_clientFilesList)
            {
                FileInfo file = new FileInfo(pathToModsFolder_textBox.Text + X);
                compareClientFiles_listView.Items.Add(X + ":" + file.Length + ":" + file.LastWriteTimeUtc);

                progressBar1.PerformStep();

                ChangeHeader("Comparing... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
            }

            foreach (string X in folder_serverFilesList)
            {
                FileInfo file = new FileInfo(pathToSyncFolder_textBox.Text + X);
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

            if (compareMissingFiles_listView.Items.Count != 0 || compareExcessFiles_listView.Items.Count != 0)
                return false;

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

        public string GetMD5FromPath(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }
        }

        public string GetMD5FromBuffer(string text)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.Default.GetBytes(text);
                return BitConverter.ToString(md5.ComputeHash(inputBytes)).Replace("-", "").ToLower();
            }
        }

        public void SaveLauncherFiles(string json_new)
        {
            try
            {
                File.WriteAllText(pathToModsFolder_textBox.Text + "\\MurshunLauncherFiles.json", json_new);

                if (Directory.Exists(pathToSyncFolder_textBox.Text) && pathToSyncFolder_textBox.Text.ToLower() != pathToModsFolder_textBox.Text.ToLower())
                    File.WriteAllText(pathToSyncFolder_textBox.Text + "\\MurshunLauncherFiles.json", json_new);

                MessageBox.Show("MurshunLauncherFiles.json was saved.");
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error on saving of MurshunLauncherFiles.json.\n\n" + e.Message);
            }
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

        public async void CreateVerifyFile()
        {
            if (!ReadPresetFile())
                return;

            List<string> folderFiles = Directory.GetFiles(pathToModsFolder_textBox.Text, "*", SearchOption.AllDirectories).ToList();

            folderFiles = folderFiles.Select(a => a.Replace(pathToModsFolder_textBox.Text, "")).Select(b => b.ToLower()).ToList();

            Console.WriteLine(folderFiles.Count);

            folderFiles = folderFiles.Where(a => presetModsList.Any(b => a.StartsWith("\\" + b.ToLower() + "\\"))).Where(c => c.EndsWith(".pbo") || c.EndsWith(".dll")).ToList();

            Console.WriteLine(folderFiles.Count);

            LauncherConfigJson json = new LauncherConfigJson();

            json.server = server;
            json.password = password;
            json.verify_link = verifyModsLink;
            json.missions_link = missionsLink;
            json.mods = presetModsList.ToArray();
            json.files = new Dictionary<string, LauncherConfigJsonFile>();

            LauncherConfigJson json_old = new LauncherConfigJson();

            string murshunLauncherFilesPath = pathToModsFolder_textBox.Text + "\\MurshunLauncherFiles.json";

            if (File.Exists(murshunLauncherFilesPath))
                json_old = JsonConvert.DeserializeObject<LauncherConfigJson>(File.ReadAllText(murshunLauncherFilesPath));

            progressBar1.Minimum = 0;
            progressBar1.Maximum = folderFiles.Count();
            progressBar1.Value = 0;
            progressBar1.Step = 1;

            json = await Task.Run(() => BuildVerifyList(folderFiles, json_old, json));

            string json_new = JsonConvert.SerializeObject(json, Formatting.Indented);

            SetLauncherFiles(GetMD5FromBuffer(json_new));

            SaveLauncherFiles(json_new);
        }

        public struct LauncherConfigJsonFile
        {
            public long size;
            public string date;
            public string md5;
        }

        public Int32 GetUnixTime(DateTime date)
        {
            return (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public LauncherConfigJson BuildVerifyList(List<string> folderFiles, LauncherConfigJson json_old, LauncherConfigJson json)
        {
            LockInterface("Building Verify File...");

            foreach (string X in folderFiles)
            {
                FileInfo file = new FileInfo(pathToModsFolder_textBox.Text + X);

                ChangeHeader("Reading... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");

                LauncherConfigJsonFile data = new LauncherConfigJsonFile();

                data.size = file.Length;
                data.date = GetUnixTime(file.LastWriteTimeUtc).ToString();

                try
                {
                    if (json_old.files[X].date == GetUnixTime(file.LastWriteTimeUtc).ToString())
                        data.md5 = json_old.files[X].md5;
                    else
                        data.md5 = GetMD5FromPath(pathToModsFolder_textBox.Text + X);
                }
                catch
                {
                    data.md5 = GetMD5FromPath(pathToModsFolder_textBox.Text + X);
                }

                json.files[X] = data;

                Invoke(new Action(() => progressBar1.PerformStep()));

                ChangeHeader("Reading... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
            }

            UnlockInterface();

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
