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

namespace MurshunLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            pathToTheLauncher = Directory.GetCurrentDirectory();
            iniDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MurshunLauncher";
            iniFilePath = iniDirectoryPath + "\\MurshunLauncherClient.ini";
            iniFilePathServer = iniDirectoryPath + "\\MurshunLauncherServer.ini";

            textBox3.Text = iniFilePath;
            textBox4.Text = "-world=empty -nosplash -skipintro -nologs -nofilepatching";

            if (!Directory.Exists(iniDirectoryPath))
            {
                Directory.CreateDirectory(iniDirectoryPath);
            }

            if (File.Exists(iniFilePath))
            {
                ReadIniFile();
                ReadIniFileServer();
                RefreshInterface();
            }
            else
            {
                MessageBox.Show("MurshunLauncherClient.ini not found.");

                pathToArma3EXE = pathToTheLauncher + "\\arma3.exe";
                pathToArma3Mods = pathToTheLauncher;

                SaveIniFile();
                RefreshInterface();
            }

            if (File.Exists(iniDirectoryPath + "\\MurshunLauncherPreset.txt"))
            {
                string[] infoFromPresetFile = File.ReadAllLines(iniDirectoryPath + "\\MurshunLauncherPreset.txt");

                string modsStringArray = infoFromPresetFile[0];

                presetModsList = modsStringArray.Split(';').ToList();

                presetModsList = presetModsList.Select(s => s.Replace("-mod=", "")).ToList();
                presetModsList = presetModsList.Select(s => s.Replace(";", "")).ToList();
                presetModsList.RemoveAll(s => String.IsNullOrEmpty(s.Trim()));

                RefreshPresetModsList();

                Thread NewThread = new Thread(() => GetWebModLine());
                NewThread.Start();
            }
            else
            {
                MessageBox.Show("MurshunLauncherPreset.txt not found. Trying to get it from Poddy...");

                GetWebModLine();
            }

            label3.Text = "Version " + launcherVersion.ToString();
        }

        string iniDirectoryPath;
        string iniFilePath;
        string iniFilePathServer;
        string pathToArma3EXE;
        string pathToArma3Mods;
        string pathToTheLauncher;

        string[] missingFilesArray;
        string[] excessFilesArray;

        List<string> presetModsList = new List<string>();
        List<string> customModsList = new List<string>();
        List<string> checkedModsList = new List<string>();

        double launcherVersion = 0.22;

        public void ReadIniFile()
        {
            string[] infoFromIniFile = File.ReadAllLines(iniFilePath);

            foreach (string X in infoFromIniFile)
            {
                if (X.Contains("GAMEPATH="))
                {
                    pathToArma3EXE = X.Replace("GAMEPATH=", "");
                }
                if (X.Contains("MODPATH="))
                {
                    pathToArma3Mods = X.Replace("MODPATH=", "");
                }
                if (X.Contains("SHOWSCRIPTERRORS=True"))
                {
                    checkBox1.Checked = true;
                }
                if (X.Contains("CONNECTTOTHESERVER=True"))
                {
                    checkBox2.Checked = true;
                }
                if (X.Contains("CUSTOMMOD="))
                {
                    customModsList.Add(X.Replace("CUSTOMMOD=", ""));
                }
                if (X.Contains("CHECKEDMOD="))
                {
                    checkedModsList.Add(X.Replace("CHECKEDMOD=", ""));
                }
                if (X.Contains("DEFAULTLINE="))
                {
                    textBox4.Text = (X.Replace("DEFAULTLINE=", ""));
                }
                if (X.Contains("STARTLINE="))
                {
                    textBox5.Text = (X.Replace("STARTLINE=", ""));
                }
                if (X.Contains("VERIFYBEFORELAUNCH=True"))
                {
                    checkBox3.Checked = true;
                }
            }

            foreach (string X in customModsList)
            {
                listView7.Items.Add(X);
            }

            for (int i = 0; i < listView7.Items.Count; i++)
            {
                if (checkedModsList.Contains(listView7.Items[i].Text))
                {
                    listView7.Items[i].Checked = true;
                }
                else
                {
                    listView7.Items[i].Checked = false;
                }
            }
        }

        public void ReadIniFileServer()
        {
            string[] infoFromIniFile = File.ReadAllLines(iniFilePathServer);

            foreach (string X in infoFromIniFile)
            {
                if (X.Contains("SERVERPATH="))
                {
                    textBox6.Text = X.Replace("SERVERPATH=", "");
                }
                if (X.Contains("MODPATH="))
                {
                    textBox7.Text = X.Replace("MODPATH=", "");
                }
                if (X.Contains("CUSTOMMOD="))
                {
                    listView9.Items.Add(X.Replace("CUSTOMMOD=", ""));
                }
                if (X.Contains("CONFIG="))
                {
                    textBox8.Text = X.Replace("CONFIG=", "");
                }
                if (X.Contains("CFG="))
                {
                    textBox9.Text = X.Replace("CFG=", "");
                }
                if (X.Contains("PROFILES="))
                {
                    textBox10.Text = X.Replace("PROFILES=", "");
                }
            }
        }

        public void SaveIniFile()
        {
            List<string> infoForSave = new List<string>();

            infoForSave.Add("GAMEPATH=" + pathToArma3EXE);

            infoForSave.Add("MODPATH=" + pathToArma3Mods);

            infoForSave.Add("SHOWSCRIPTERRORS=" + checkBox1.Checked);

            infoForSave.Add("CONNECTTOTHESERVER=" + checkBox2.Checked);

            foreach (string X in customModsList)
            {
                infoForSave.Add("CUSTOMMOD=" + X);
            }

            foreach (string X in checkedModsList)
            {
                infoForSave.Add("CHECKEDMOD=" + X);
            }

            if (textBox4.Text != "")
            {
                infoForSave.Add("DEFAULTLINE=" + textBox4.Text);
            }

            if (textBox5.Text != "")
            {
                infoForSave.Add("STARTLINE=" + textBox5.Text);
            }

            infoForSave.Add("VERIFYBEFORELAUNCH=" + checkBox3.Checked);

            File.WriteAllLines(iniFilePath, infoForSave);
        }

        public void RefreshInterface()
        {
            listView5.Items.Clear();

            listView5.Items.Add("GAMEPATH=" + pathToArma3EXE);

            listView5.Items.Add("MODPATH=" + pathToArma3Mods);

            listView5.Items.Add("SHOWSCRIPTERRORS=" + checkBox1.Checked);

            listView5.Items.Add("CONNECTTOTHESERVER=" + checkBox2.Checked);

            foreach (string X in customModsList)
            {
                listView5.Items.Add("CUSTOMMOD=" + X);
            }

            foreach (string X in checkedModsList)
            {
                listView5.Items.Add("CHECKEDMOD=" + X);
            }

            if (textBox4.Text != "")
            {
                listView5.Items.Add("DEFAULTLINE=" + textBox4.Text);
            }

            if (textBox5.Text != "")
            {
                listView5.Items.Add("STARTLINE=" + textBox5.Text);
            }

            listView5.Items.Add("VERIFYBEFORELAUNCH=" + checkBox3.Checked);

            foreach (ListViewItem X in listView6.Items)
            {
                if (Directory.Exists(pathToArma3Mods + "\\" + X.Text + "\\addons") || Directory.Exists(pathToArma3Mods + "\\" + X.Text + "\\Addons"))
                {
                    X.BackColor = Color.Green;
                }
                else
                {
                    X.BackColor = Color.Red;
                }
            }

            foreach (ListViewItem X in listView7.Items)
            {
                if (Directory.Exists(X.Text + "\\addons") || Directory.Exists(X.Text + "\\Addons"))
                {
                    X.BackColor = Color.Green;
                }
                else
                {
                    X.BackColor = Color.Red;
                }
            }

            foreach (ListViewItem X in listView8.Items)
            {
                if (Directory.Exists(textBox7.Text + "\\" + X.Text + "\\addons") || Directory.Exists(textBox7.Text + "\\" + X.Text + "\\Addons"))
                {
                    X.BackColor = Color.Green;
                }
                else
                {
                    X.BackColor = Color.Red;
                }
            }

            foreach (ListViewItem X in listView9.Items)
            {
                if (Directory.Exists(X.Text + "\\addons") || Directory.Exists(X.Text + "\\Addons"))
                {
                    X.BackColor = Color.Green;
                }
                else
                {
                    X.BackColor = Color.Red;
                }
            }
        }

        public void VerifyMods()
        {
            if (File.Exists(pathToArma3Mods + "\\MurshunLauncherFiles.txt"))
            {
                textBox2.Text = pathToArma3Mods + "\\MurshunLauncherFiles.txt";

                string[] btsync_fileLinesArray = File.ReadAllLines(pathToArma3Mods + "\\MurshunLauncherFiles.txt");

                List<string> btsync_fileLinesList = new List<string>();

                btsync_fileLinesList = btsync_fileLinesArray.ToList();

                btsync_fileLinesList.Add("\\Arma 3\\Userconfig\\task_force_radio\\radio_settings.hpp");

                btsync_fileLinesArray = btsync_fileLinesList.ToArray();

                List<string> btsync_filesList = new List<string>();
                List<string> btsync_foldersList = new List<string>();

                foreach (string X in btsync_fileLinesArray)
                {
                    int count = X.Count(f => f == '\\');

                    if (count > 1)
                    {
                        btsync_filesList.Add(X);
                    }
                    else
                    {
                        btsync_foldersList.Add(X + "\\");
                    }
                }

                listView4.Items.Clear();

                foreach (string X in btsync_filesList)
                {
                    listView4.Items.Add(X);
                }

                textBox1.Text = pathToArma3Mods;

                string[] folder_foldersArray = Directory.GetDirectories(pathToArma3Mods, "*", SearchOption.TopDirectoryOnly).Where(s => s.Contains("@")).ToArray();
                string[] folder_filesArray = Directory.GetFiles(pathToArma3Mods, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToArray();

                folder_foldersArray = folder_foldersArray.Select(s => s.Replace(pathToArma3Mods, "")).ToArray();
                folder_filesArray = folder_filesArray.Select(s => s.Replace(pathToArma3Mods, "")).ToArray();

                List<string> folder_filesList = new List<string>();

                foreach (string X in folder_foldersArray)
                {
                    if (btsync_foldersList.Any(X.Contains) && X.Substring(0, 2) == "\\@")
                    {
                        folder_filesList.Add(X);
                    }
                }

                foreach (string X in folder_filesArray)
                {
                    if (btsync_foldersList.Any(X.Contains) && X.Substring(0, 2) == "\\@")
                    {
                        FileInfo F = new FileInfo(pathToArma3Mods + X);
                        folder_filesList.Add(X + ":" + F.Length);
                    }
                }

                listView1.Items.Clear();

                if (File.Exists(pathToArma3EXE.Replace("\\arma3.exe", "") + "\\Userconfig\\task_force_radio\\radio_settings.hpp"))
                {
                    folder_filesList.Add("\\Arma 3\\Userconfig\\task_force_radio\\radio_settings.hpp");
                }

                foreach (string X in folder_filesList)
                {
                    listView1.Items.Add(X);
                }

                folder_filesArray = folder_filesList.ToArray();

                excessFilesArray = folder_filesArray.Where(x => !btsync_fileLinesArray.Contains(x)).ToArray();

                missingFilesArray = btsync_fileLinesArray.Where(x => !folder_filesArray.Contains(x)).ToArray();
                missingFilesArray = missingFilesArray.Where(x => !folder_foldersArray.Contains(x)).ToArray();

                listView2.Items.Clear();

                listView3.Items.Clear();

                if (missingFilesArray.Length == 0)
                {
                    listView2.Items.Add("No missing files.");
                }
                else
                {
                    foreach (string X in missingFilesArray)
                    {
                        listView2.Items.Add(X);
                    }
                }

                if (excessFilesArray.Length == 0)
                {
                    listView3.Items.Add("No excess files.");
                }
                else
                {
                    foreach (string X in excessFilesArray)
                    {
                        listView3.Items.Add(X);
                    }
                }
            }
            else
            {
                MessageBox.Show("MurshunLauncherFiles.txt not found.");
            }
        }

        public void GetWebModLine()
        {
            WebClient client = new WebClient();

            try
            {
                string webModLine = client.DownloadString("http://dedick.podkolpakom.net/arma/mods.php");

                File.WriteAllText(iniDirectoryPath + "\\MurshunLauncherPreset.txt", webModLine);

                presetModsList = webModLine.Split(';').ToList();

                presetModsList = presetModsList.Select(s => s.Replace("-mod=", "")).ToList();
                presetModsList = presetModsList.Select(s => s.Replace(";", "")).ToList();
                presetModsList.RemoveAll(s => String.IsNullOrEmpty(s.Trim()));

                if (InvokeRequired)
                {
                    this.Invoke(new Action(() => RefreshPresetModsList()));
                    this.Invoke(new Action(() => RefreshInterface()));
                }
                else
                {
                    RefreshPresetModsList();
                    RefreshInterface();
                }
            }
            catch (Exception)
            {
                if (!InvokeRequired)
                {
                    MessageBox.Show("Couldn't retrieve MurshunLauncherPreset.txt from Poddy.");
                }
            }
        }

        public void RefreshPresetModsList()
        {
            listView6.Items.Clear();

            listView8.Items.Clear();

            foreach (string X in presetModsList)
            {
                listView6.Items.Add(X);

                listView8.Items.Add(X.Replace("@allinarmaterrainpack", "@allinarmaterrainpacklite"));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog choosenFolder = new FolderBrowserDialog();
            choosenFolder.Description = "Select your mods folder.";
            choosenFolder.SelectedPath = pathToArma3Mods;

            if (choosenFolder.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = choosenFolder.SelectedPath;

                string[] folder_foldersArray = Directory.GetDirectories(choosenFolder.SelectedPath, "*", SearchOption.TopDirectoryOnly).Where(s => s.Contains("@")).ToArray();
                string[] folder_filesArray = Directory.GetFiles(choosenFolder.SelectedPath, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToArray();

                folder_foldersArray = folder_foldersArray.Select(s => s.Replace(choosenFolder.SelectedPath, "")).ToArray();
                folder_filesArray = folder_filesArray.Select(s => s.Replace(choosenFolder.SelectedPath, "")).ToArray();

                listView1.Items.Clear();

                foreach (string X in folder_foldersArray)
                {
                    if (X.Substring(0, 2) == "\\@")
                    {
                        listView1.Items.Add(X);
                    }
                }

                foreach (string X in folder_filesArray)
                {
                    if (X.Substring(0, 2) == "\\@")
                    {
                        FileInfo F = new FileInfo(choosenFolder.SelectedPath + X);
                        listView1.Items.Add(X + ":" + F.Length);
                    }
                }

                SaveFileDialog saveFile = new SaveFileDialog();

                saveFile.Title = "Save File Dialog";
                saveFile.Filter = "Text File (.txt) | *.txt";
                saveFile.FileName = "MurshunLauncherFiles.txt";
                saveFile.InitialDirectory = pathToArma3Mods;
                saveFile.RestoreDirectory = true;

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllLines(saveFile.FileName.ToString(), listView1.Items.Cast<ListViewItem>().Select(X => X.Text));
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VerifyMods();
        }

        private void button4_Click(object sender, EventArgs e)
        {
                SaveIniFile();
                RefreshInterface();

                if (checkBox3.Checked)
                {
                    VerifyMods();

                    if (missingFilesArray.Length == 0 && excessFilesArray.Length == 0)
                    {
                        MessageBox.Show("No missing or excess files.");
                    }
                    else
                    {
                        tabControl1.SelectedTab = tabPage2;
                        return;
                    }
                }

                string modLine;

                modLine = textBox4.Text;

                if (textBox5.Text != "")
                {
                    modLine = modLine + " " + textBox5.Text;
                }

                if (checkBox1.Checked)
                {
                    modLine = modLine + " -showscripterrors";
                }

                modLine = modLine + " \"-mod=";

                foreach (string X in presetModsList)
                {
                    modLine = modLine + pathToArma3Mods + "\\" + X + ";";
                }

                foreach (string X in checkedModsList)
                {
                    modLine = modLine + X + ";";
                }

                modLine = modLine + "\"";

                if (checkBox2.Checked)
                {
                    modLine = modLine + " -connect=109.87.76.153 -port=2302 -password=v";
                }

                if (File.Exists(pathToArma3EXE))
                {
                    Process myProcess = new Process();

                    myProcess.StartInfo.FileName = pathToArma3EXE;
                    myProcess.StartInfo.Arguments = modLine;
                    myProcess.Start();

                    Thread.Sleep(1000);

                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    MessageBox.Show("EXE not found.");
                }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectArmaPath = new OpenFileDialog();

            selectArmaPath.Title = "Select arma3.exe";
            selectArmaPath.Filter = "Executable File (.exe) | *.exe";
            selectArmaPath.InitialDirectory = pathToTheLauncher;

            if (selectArmaPath.ShowDialog() == DialogResult.OK)
            {
                pathToArma3EXE = selectArmaPath.FileName;
            }

            FolderBrowserDialog selectModsPath = new FolderBrowserDialog();
            selectModsPath.Description = "Select your mods folder.";
            selectModsPath.SelectedPath = pathToTheLauncher;

            if (selectModsPath.ShowDialog() == DialogResult.OK)
            {
                pathToArma3Mods = selectModsPath.SelectedPath;
            }

            SaveIniFile();
            RefreshInterface();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog choosenFolder = new FolderBrowserDialog();
            choosenFolder.Description = "Select custom mod folder.";
            choosenFolder.SelectedPath = pathToArma3Mods;
            
            if (choosenFolder.ShowDialog() == DialogResult.OK)
            {
                listView7.Items.Add(choosenFolder.SelectedPath);
                customModsList.Add(choosenFolder.SelectedPath);
            }

            SaveIniFile();
            RefreshInterface();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            customModsList = customModsList.Except(listView7.CheckedItems.Cast<ListViewItem>().Select(X => X.Text)).ToList();

            listView7.Items.Clear();

            foreach (string X in customModsList)
            {
                listView7.Items.Add(X);
            }

            for (int i = 0; i < listView7.Items.Count; i++)
            {
                if (checkedModsList.Contains(listView7.Items[i].Text))
                {
                    listView7.Items[i].Checked = true;
                }
                else
                {
                    listView7.Items[i].Checked = false;
                }
            }

            SaveIniFile();
            RefreshInterface();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            SaveIniFile();
            RefreshInterface();
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            SaveIniFile();
            RefreshInterface();
        }

        private void listView7_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            checkedModsList.Clear();

            checkedModsList = listView7.CheckedItems.Cast<ListViewItem>().Select(X => X.Text).ToList();

            if (e.CurrentValue == CheckState.Unchecked)
            {
                checkedModsList.Add(listView7.Items[e.Index].Text);
            }
            else
            {
                checkedModsList.Remove(listView7.Items[e.Index].Text);
            }

            SaveIniFile();
            RefreshInterface();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://murshun.club/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/groups/murshun");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveIniFile();
            RefreshInterface();
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            SaveIniFile();
            RefreshInterface();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Text = "-world=empty -nosplash -skipintro -nologs -nofilepatching";

            SaveIniFile();
            RefreshInterface();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button9_Click(sender, e);

            if (listView12.Items.Count != 0 || listView10.Items.Count != 0)
            {
                tabControl1.SelectedTab = tabPage4;
                return;
            }

            List<string> clientMissionlist = Directory.GetFiles("D:\\Program Files\\Steam\\steamapps\\common\\Arma 3\\MPMissions", "*", SearchOption.AllDirectories).Where(s => s.Contains(".pbo")).ToList();

            foreach (string X in clientMissionlist)
            {
                File.Copy(X, X.Replace("D:\\Program Files\\Steam\\steamapps\\common\\Arma 3\\MPMissions", "G:\\Program Files\\Steam\\SteamApps\\common\\Arma 3 Server\\mpmissions"), true);
            }

            string modLine;

            modLine = "-port=2302";

            modLine = modLine + " \"-config=" + textBox8.Text + "\"";

            modLine = modLine + " \"-cfg=" + textBox9.Text + "\"";

            modLine = modLine + " \"-profiles=" + textBox10.Text + "\"";

            modLine = modLine + " -name=server";

            modLine = modLine + " \"-mod=";

            foreach (ListViewItem X in listView8.Items)
            {
                modLine = modLine + textBox7.Text + "\\" + X.Text + ";";
            }

            foreach (ListViewItem X in listView9.Items)
            {
                modLine = modLine + X.Text + ";";
            }

            modLine = modLine + "\"";

            modLine = modLine + " -nologs -nofilepatching";

            if (File.Exists(textBox6.Text))
            {                
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = textBox6.Text;
                myProcess.StartInfo.Arguments = modLine;
                myProcess.Start();
                myProcess.ProcessorAffinity = (System.IntPtr)12;
                myProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            else
            {
                MessageBox.Show("EXE not found.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox12.Text = pathToArma3Mods;
            textBox11.Text = textBox7.Text;

            List<string> folder_clientFilesList = Directory.GetFiles(pathToArma3Mods, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToList();
            List<string> folder_serverFilesList = Directory.GetFiles(textBox7.Text, "*", SearchOption.AllDirectories).Where(s => s.Contains("@")).ToList();

            listView13.Items.Clear();
            listView11.Items.Clear();

            foreach (string X in folder_clientFilesList)
            {
                FileInfo F = new FileInfo(X);
                listView13.Items.Add(X.Replace(pathToArma3Mods,"") + ":" + F.Length);
            }

            foreach (string X in folder_serverFilesList)
            {
                FileInfo F = new FileInfo(X);
                listView11.Items.Add(X.Replace(textBox7.Text, "") + ":" + F.Length);
            }

            folder_clientFilesList = listView13.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();
            folder_serverFilesList = listView11.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();

            List<string> missingFilesList = folder_clientFilesList.Where(x => !folder_serverFilesList.Contains(x)).ToList();
            List<string> excessFilesList = folder_serverFilesList.Where(x => !folder_clientFilesList.Contains(x)).ToList();

            listView12.Items.Clear();
            listView10.Items.Clear();

            foreach (string X in missingFilesList)
            {
                if (!X.Contains("@allinarmaterrainpack"))
                {
                    listView12.Items.Add(X);
                }
            }

            foreach (string X in excessFilesList)
            {
                if (!X.Contains("@allinarmaterrainpack"))
                {
                    listView10.Items.Add(X);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView10.Items)
            {
                MessageBox.Show("Deleting " + textBox7.Text + item.Text.Split(':')[0]);
                File.Delete(textBox7.Text + item.Text.Split(':')[0]);
            }

            foreach (ListViewItem item in listView12.Items)
            {
                MessageBox.Show("Copying " + pathToArma3Mods + item.Text.Split(':')[0] + " to " + textBox7.Text + item.Text.Split(':')[0]);
                File.Copy(pathToArma3Mods + item.Text.Split(':')[0], textBox7.Text + item.Text.Split(':')[0], true);
            }
        }
    }
}
