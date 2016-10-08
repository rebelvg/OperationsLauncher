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
        public Form1()
        {
            InitializeComponent();

            try
            {
                if (Process.GetProcessesByName("MurshunLauncher").Length > 1)
                {
                    MessageBox.Show("Launcher is already running.");
                    System.Environment.Exit(1);
                }

                string iniDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MurshunLauncher";

                xmlPath_textBox.Text = iniDirectoryPath + "\\MurshunLauncher.xml";

                if (!Directory.Exists(iniDirectoryPath))
                {
                    try
                    {
                        Directory.CreateDirectory(iniDirectoryPath);
                    }
                    catch
                    {
                        MessageBox.Show("Couldn't create a folder at " + iniDirectoryPath);
                    }
                }

                if (File.Exists(xmlPath_textBox.Text))
                {
                    ReadXmlFile();
                }
                else
                {
                    try
                    {
                        LauncherSettings = new MurshunLauncherXmlSettings();

                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MurshunLauncherXmlSettings));

                        System.IO.FileStream writer = System.IO.File.Create(xmlPath_textBox.Text);
                        serializer.Serialize(writer, LauncherSettings);
                        writer.Close();

                        ReadXmlFile();
                    }
                    catch
                    {
                        MessageBox.Show("Saving xml settings failed.");
                    }
                }

                label3.Text = "Version " + launcherVersion;
            }
            catch (Exception e)
            {
                MessageBox.Show("Launcher crashed while initializing. Try running it as administrator.\n\n" + e);
                System.Environment.Exit(1);
            }
        }

        public class MurshunLauncherXmlSettings
        {
            public string pathToArma3Client_textBox = Directory.GetCurrentDirectory() + "\\arma3.exe";
            public string pathToArma3ClientMods_textBox = Directory.GetCurrentDirectory();
            public bool joinTheServer_checkBox;
            public List<string> clientCustomMods_listView;
            public List<string> clientCheckedModsList_listView;
            public string defaultStartLine_textBox = "-world=empty -nosplash -skipintro -nofilepatching -nologs";
            public string advancedStartLine_textBox;
            public string teamSpeakFolder_textBox = @"C:\Program Files (x86)\TeamSpeak 3 Client";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VerifyMods(false);
        }

        private void launch_button_Click(object sender, EventArgs e)
        {
            ReadPresetFile();

            if (!VerifyMods(false))
            {
                DialogResult dialogResult = MessageBox.Show("Launch the client anyway?", "", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    tabControl1.SelectedTab = tabPage1;
                }
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            string modLine;

            modLine = defaultStartLine_textBox.Text;

            if (advancedStartLine_textBox.Text != "")
            {
                modLine = modLine + " " + advancedStartLine_textBox.Text;
            }

            modLine = modLine + " \"-mod=";

            foreach (ListViewItem X in clientPresetMods_listView.Items)
            {
                modLine = modLine + pathToArma3ClientMods_textBox.Text + "\\" + X.Text + ";";
            }

            foreach (ListViewItem X in clientCustomMods_listView.CheckedItems)
            {
                modLine = modLine + X.Text + ";";
            }

            modLine = modLine + "\"";

            if (joinTheServer_checkBox.Checked)
            {
                if (server != "")
                {
                    modLine = modLine + " -connect=" + server;

                    if (password != "")
                        modLine = modLine + " -password=" + password;
                }
            }

            if (File.Exists(pathToArma3Client_textBox.Text))
            {
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = pathToArma3Client_textBox.Text;
                myProcess.StartInfo.Arguments = modLine;
                myProcess.Start();
            }
            else
            {
                MessageBox.Show("arma3.exe not found.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.Description = "Select custom mod folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                clientCustomMods_listView.Items.Add(chosenFolder.SelectedPath);

                ReadPresetFile();
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            defaultStartLine_textBox.Text = "-world=empty -nosplash -skipintro -nofilepatching -nologs";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();

            selectFile.Title = "Select arma3.exe";
            selectFile.Filter = "Executable File (.exe) | *.exe";
            selectFile.RestoreDirectory = true;

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                pathToArma3Client_textBox.Text = selectFile.FileName;

                ReadPresetFile();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.Description = "Select client mods folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                pathToArma3ClientMods_textBox.Text = chosenFolder.SelectedPath;

                ReadPresetFile();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Remove " + clientExcessFiles_listView.Items.Count + " excess files?", "", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                foreach (ListViewItem X in clientExcessFiles_listView.Items)
                {
                    File.Delete(pathToArma3ClientMods_textBox.Text + X.Text.Split(':')[0]);
                }

                MessageBox.Show("Done.");
            }
        }

        private void removeUncheckedMod_button_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in clientCustomMods_listView.Items)
            {
                if (!item.Checked)
                    item.Remove();
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/rebelvg/MurshunLauncher");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveXmlFile();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://community.bistudio.com/wiki/Arma_3_Startup_Parameters");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (defaultStartLine_textBox.Text.Contains("-showscripterrors"))
            {
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text.Replace(" -showscripterrors", "");
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text.Replace("-showscripterrors", "");
            }
            else
            {
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text + " -showscripterrors";
            }
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (defaultStartLine_textBox.Text.Contains("-nologs"))
            {
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text.Replace(" -nologs", "");
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text.Replace("-nologs", "");
            }
            else
            {
                defaultStartLine_textBox.Text = defaultStartLine_textBox.Text + " -nologs";
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ReadPresetFile();

            CheckSyncFolderSize();

            VerifyMods(false);
        }

        private void refreshClient_button_Click(object sender, EventArgs e)
        {
            ReadPresetFile();

            CheckSyncFolderSize();

            VerifyMods(false);
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            SaveXmlFile();
        }

        private void fullVerify_button_Click(object sender, EventArgs e)
        {
            Thread NewThread = new Thread(() => VerifyMods(true));
            NewThread.Start();
        }

        private void changePathToTeamSpeakFolder_button_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.Description = "Select teamspeak folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                teamSpeakFolder_textBox.Text = chosenFolder.SelectedPath;

                ReadPresetFile();
            }
        }
    }
}
