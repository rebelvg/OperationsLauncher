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
                teamSpeakFolder_textBox.Text = LauncherSettings.teamSpeakFolder_textBox;

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
                LauncherSettings.teamSpeakFolder_textBox = teamSpeakFolder_textBox.Text;

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

        public bool VerifyMods(bool fullVerify)
        {
            bool verifySuccess = true;

            LockInterface("Verifying...");

            string murshunLauncherFilesPath = pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.json";

            if (File.Exists(murshunLauncherFilesPath))
            {
                Dictionary<string, dynamic> json = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(murshunLauncherFilesPath));

                verifySuccess = CheckLauncherFiles(json["verify_link"], GetMD5(murshunLauncherFilesPath));

                List<string> folderFiles = Directory.GetFiles(pathToArma3ClientMods_textBox.Text, "*", SearchOption.AllDirectories).ToList();

                folderFiles = folderFiles.Select(a => a.Replace(pathToArma3ClientMods_textBox.Text, "")).Select(b => b.ToLower()).ToList();

                folderFiles = folderFiles.Where(a => presetModsList.Any(b => a.StartsWith("\\" + b + "\\"))).Where(c => c.EndsWith(".pbo") || c.EndsWith(".dll")).ToList();

                this.Invoke(new Action(() =>
                {
                    clientModsFiles_listView.Items.Clear();
                    murshunLauncherFiles_listView.Items.Clear();

                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = folderFiles.Count();
                    progressBar1.Value = 0;
                    progressBar1.Step = 1;
                }));

                List<string> clientFiles = new List<string>();

                foreach (string X in folderFiles)
                {
                    FileInfo file = new FileInfo(pathToArma3ClientMods_textBox.Text + X);

                    if (!fullVerify)
                        clientFiles.Add(X + ":" + file.Length);
                    else
                        clientFiles.Add(X + ":" + GetMD5(pathToArma3ClientMods_textBox.Text + X));

                    this.Invoke(new Action(() => progressBar1.PerformStep()));

                    ChangeHeader("Verifying... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + file.Name + "/" + file.Length / 1024 / 1024 + "mb");
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

                    clientMods_textBox.Text = "Client Mods (" + clientModsFiles_listView.Items.Count + " files / " + clientMissingFiles_listView.Items.Count + " missing)";
                    murshunLauncherFiles_textBox.Text = "MurshunLauncherFiles.json (" + murshunLauncherFiles_listView.Items.Count + " files / " + clientExcessFiles_listView.Items.Count + " excess)";

                    if (clientMissingFiles_listView.Items.Count != 0 || clientExcessFiles_listView.Items.Count != 0)
                    {
                        if (tabControl1.SelectedTab != tabPage2)
                        {
                            MessageBox.Show("You have missing or excess files.");
                            tabControl1.SelectedTab = tabPage2;
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

            CheckACRE2();

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

        public void RefreshPresetModsList()
        {
            SetColorOnText(pathToArma3Client_textBox);
            SetColorOnText(pathToArma3ClientMods_textBox);
            SetColorOnText(teamSpeakFolder_textBox);

            SetColorOnPresetList(clientPresetMods_listView, pathToArma3ClientMods_textBox.Text);

            SetColorOnCustomList(clientCustomMods_listView, columnHeader7);
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
            string murshunLauncherFilesPath = pathToArma3ClientMods_textBox.Text + "\\MurshunLauncherFiles.json";

            if (File.Exists(murshunLauncherFilesPath))
            {
                dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(murshunLauncherFilesPath));

                presetModsList = json.mods.ToObject<List<string>>();

                server = json["server"];
                password = json["password"];
            }

            RefreshPresetModsList();
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

        public void CheckACRE2()
        {
            string acre32plugin = @"\acre2_win32.dll";
            string acre64plugin = @"\acre2_win64.dll";

            string acre32mods = pathToArma3ClientMods_textBox.Text + @"\@acre2\plugin" + acre32plugin;
            string acre64mods = pathToArma3ClientMods_textBox.Text + @"\@acre2\plugin" + acre64plugin;

            if (File.Exists(acre32mods) && File.Exists(acre64mods))
            {
                if (!Directory.Exists(teamSpeakFolder_textBox.Text))
                    MessageBox.Show("Can't find your TS folder to automaticly copy ACRE2 plugins in.");
                else
                    CopyPlugins(teamSpeakFolder_textBox.Text + @"\plugins");
            }
        }

        private void CopyPlugins(string tspath)
        {
            string acre32plugin = @"\acre2_win32.dll";
            string acre64plugin = @"\acre2_win64.dll";

            string acre32mods = pathToArma3ClientMods_textBox.Text + @"\@acre2\plugin" + acre32plugin;
            string acre64mods = pathToArma3ClientMods_textBox.Text + @"\@acre2\plugin" + acre64plugin;

            if (File.Exists(tspath + acre32plugin) && File.Exists(tspath + acre64plugin))
            {
                if (!(GetMD5(tspath + acre32plugin) == GetMD5(acre32mods) && GetMD5(tspath + acre64plugin) == GetMD5(acre64mods)))
                {
                    try
                    {
                        File.Copy(acre32mods, tspath + acre32plugin, true);
                        File.Copy(acre64mods, tspath + acre64plugin, true);

                        MessageBox.Show("New ACRE2 plugins successfully copied into your TS folder.");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Can't overwrite ACRE2 plugins.\n" + e.Message);
                    }
                }
            }
            else
            {
                try
                {
                    File.Copy(acre32mods, tspath + acre32plugin, true);
                    File.Copy(acre64mods, tspath + acre64plugin, true);

                    MessageBox.Show("New ACRE2 plugins successfully copied into your TS folder.");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Can't overwrite ACRE2 plugins.\n" + e.Message);
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
                ChangeHeader("Murshun Launcher");
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
