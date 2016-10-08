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

                pathToArma3ClientMods_textBox.Text = LauncherSettings.pathToArma3ClientMods_textBox;
                pathToArma3ServerMods_textBox.Text = LauncherSettings.pathToArma3ServerMods_textBox;
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

                LauncherSettings.pathToArma3ClientMods_textBox = pathToArma3ClientMods_textBox.Text;
                LauncherSettings.pathToArma3ServerMods_textBox = pathToArma3ServerMods_textBox.Text;
                LauncherSettings.modListLink = repoConfigPath_textBox.Text;

                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MurshunLauncherXmlSettings));

                System.IO.FileStream writer = System.IO.File.Create(xmlPath_textBox.Text);
                serializer.Serialize(writer, LauncherSettings);
                writer.Close();
            }
            catch
            {
                PrintMessage("Saving xml settings failed.");
            }
        }

        public void ReadPresetFile()
        {
            string murshunLauncherFilesPath = repoConfigPath_textBox.Text;

            if (File.Exists(murshunLauncherFilesPath))
            {
                dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(murshunLauncherFilesPath));

                presetModsList = json.mods.ToObject<List<string>>();

                server = json["server"];
                password = json["password"];
                verifyModsLink = json["verify_link"];
                verifyModsPassword = json["verify_password"];
            }
            else
            {
                PrintMessage("Couldn't find the config to get the modlist.\n" + repoConfigPath_textBox.Text);
            }

            RefreshPresetModsList();
        }

        public bool SetLauncherFiles(string localJsonMD5)
        {
            bool success = false;

            if (verifyModsLink == "")
                return true;

            ChangeHeader("Writing md5 to the server... " + verifyModsLink);

            WebClient client = new WebClient();

            try
            {
                string modLineString = client.DownloadString(verifyModsLink + "?md5=" + localJsonMD5 + "&password=" + verifyModsPassword);
                success = true;
            }
            catch
            {
                this.Invoke(new Action(() => PrintMessage("There was an error on accessing the server.\n" + verifyModsLink)));
            }

            return success;
        }

        public void RefreshPresetModsList()
        {
            SetColorOnText(pathToArma3ClientMods_textBox);
            SetColorOnText(pathToArma3ServerMods_textBox);
            SetColorOnText(repoConfigPath_textBox);

            SetColorOnPresetList(serverPresetMods_listView, pathToArma3ServerMods_textBox.Text);
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
            bool compareSuccess = true;

            LockInterface("Comparing...");

            ReadPresetFile();

            List<string> folder_clientFilesList = Directory.GetFiles(pathToArma3ClientMods_textBox.Text, "*", SearchOption.AllDirectories).ToList();

            folder_clientFilesList = folder_clientFilesList.Select(a => a.Replace(pathToArma3ClientMods_textBox.Text, "")).Select(b => b.ToLower()).ToList();

            folder_clientFilesList = folder_clientFilesList.Where(a => presetModsList.Any(b => a.StartsWith("\\" + b + "\\"))).ToList();

            List<string> folder_serverFilesList = Directory.GetFiles(pathToArma3ServerMods_textBox.Text, "*", SearchOption.AllDirectories).ToList();

            folder_serverFilesList = folder_serverFilesList.Select(a => a.Replace(pathToArma3ServerMods_textBox.Text, "")).Select(b => b.ToLower()).ToList();

            folder_serverFilesList = folder_serverFilesList.Where(a => presetModsList.Any(b => a.StartsWith("\\" + b + "\\"))).ToList();

            compareClientFiles_listView.Items.Clear();
            compareServerFiles_listView.Items.Clear();

            progressBar2.Minimum = 0;
            progressBar2.Maximum = folder_clientFilesList.Count() + folder_serverFilesList.Count();
            progressBar2.Value = 0;
            progressBar2.Step = 1;

            foreach (string X in folder_clientFilesList)
            {
                FileInfo file = new FileInfo(pathToArma3ClientMods_textBox.Text + X);
                compareClientFiles_listView.Items.Add(X + ":" + file.Length + ":" + file.LastWriteTimeUtc);

                progressBar2.PerformStep();

                ChangeHeader("Comparing... (" + progressBar2.Value + "/" + progressBar2.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
            }

            foreach (string X in folder_serverFilesList)
            {
                FileInfo file = new FileInfo(pathToArma3ServerMods_textBox.Text + X);
                compareServerFiles_listView.Items.Add(X + ":" + file.Length + ":" + file.LastWriteTimeUtc);

                progressBar2.PerformStep();

                ChangeHeader("Comparing... (" + progressBar2.Value + "/" + progressBar2.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
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
                if (tabControl1.SelectedTab != tabPage4)
                {
                    PrintMessage("You have missing or excess files.");
                    tabControl1.SelectedTab = tabPage4;
                    compareSuccess = false;
                }
            }

            UnlockInterface();

            return compareSuccess;
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

        public string GetMD5String(string text)
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
                File.WriteAllText(pathToArma3ServerMods_textBox.Text + "\\MurshunLauncherFiles.json", json_new);

                if (pathToArma3ClientMods_textBox.Text.ToLower() != pathToArma3ServerMods_textBox.Text.ToLower() && Directory.Exists(pathToArma3ClientMods_textBox.Text))
                    File.WriteAllText(pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.json", json_new);

                PrintMessage("MurshunLauncherFiles.json was saved.");
            }
            catch (Exception e)
            {
                PrintMessage("There was an error on saving of MurshunLauncherFiles.json.\n\n" + e);
            }
        }

        public void CreateVerifyFile()
        {
            LockInterface("Creating Verify File...");

            ReadPresetFile();

            List<string> folderFiles = Directory.GetFiles(pathToArma3ServerMods_textBox.Text, "*", SearchOption.AllDirectories).ToList();

            folderFiles = folderFiles.Select(a => a.Replace(pathToArma3ServerMods_textBox.Text, "")).Select(b => b.ToLower()).ToList();

            folderFiles = folderFiles.Where(a => presetModsList.Any(b => a.StartsWith("\\" + b + "\\"))).Where(c => c.EndsWith(".pbo") || c.EndsWith(".dll")).ToList();

            Dictionary<string, dynamic> files = new Dictionary<string, dynamic>();

            files["server"] = server;
            files["password"] = password;
            files["verify_link"] = verifyModsLink;
            files["mods"] = presetModsList;
            files["files"] = new Dictionary<string, dynamic>();

            Dictionary<string, dynamic> json_old = new Dictionary<string, dynamic>();

            if (File.Exists(pathToArma3ServerMods_textBox.Text + "\\MurshunLauncherFiles.json"))
                json_old = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(pathToArma3ServerMods_textBox.Text + "\\MurshunLauncherFiles.json"));

            progressBar2.Minimum = 0;
            progressBar2.Maximum = folderFiles.Count();
            progressBar2.Value = 0;
            progressBar2.Step = 1;

            foreach (string X in folderFiles)
            {
                FileInfo file = new FileInfo(pathToArma3ServerMods_textBox.Text + X);

                ChangeHeader("Reading... (" + progressBar2.Value + "/" + progressBar2.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");

                Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

                data["size"] = file.Length;
                data["date"] = file.LastWriteTimeUtc;

                try
                {
                    if (json_old["files"][X].date == file.LastWriteTimeUtc)
                        data["md5"] = json_old["files"][X].md5;
                    else
                        data["md5"] = GetMD5(pathToArma3ServerMods_textBox.Text + X);
                }
                catch
                {
                    data["md5"] = GetMD5(pathToArma3ServerMods_textBox.Text + X);
                }

                files["files"][X] = data;

                progressBar2.PerformStep();

                ChangeHeader("Reading... (" + progressBar2.Value + "/" + progressBar2.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
            }

            string json_new = JsonConvert.SerializeObject(files, Formatting.Indented);

            if (SetLauncherFiles(GetMD5String(json_new)))
            {
                SaveLauncherFiles(json_new);
            }

            UnlockInterface();
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
                ChangeHeader("Murshun Repo Tool");
            }));
        }

        public void ChangeHeader(string text)
        {
            this.Invoke(new Action(() =>
            {
                this.Text = text;
            }));
        }

        public void PrintMessage(string message)
        {
            if (!runSilent)
                MessageBox.Show(message);
        }
    }
}
