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

namespace MurshunLauncherServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            try
            {
                if (Process.GetProcessesByName("MurshunRepoTool").Length > 1)
                {
                    PrintMessage("Tool is already running.");
                    System.Environment.Exit(1);
                }

                string iniDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MurshunLauncher";

                xmlPath_textBox.Text = iniDirectoryPath + "\\MurshunRepoTool.xml";

                if (!Directory.Exists(iniDirectoryPath))
                {
                    try
                    {
                        Directory.CreateDirectory(iniDirectoryPath);
                    }
                    catch
                    {
                        PrintMessage("Couldn't create a folder at " + iniDirectoryPath);
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
                        PrintMessage("Saving xml settings failed.");
                    }
                }

                label3.Text = "Version " + launcherVersion;
            }
            catch (Exception e)
            {
                PrintMessage("Tool crashed while initializing. Try running it as administrator.\n\n" + e);
                System.Environment.Exit(1);
            }
        }

        public class MurshunLauncherXmlSettings
        {
            public string pathToArma3ClientMods_textBox = Directory.GetCurrentDirectory();
            public string pathToArma3ServerMods_textBox = Directory.GetCurrentDirectory();
            public string modListLink = Directory.GetCurrentDirectory() + "\\MurshunRepoToolConfig.json";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CompareFolders();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sync " + (compareExcessFiles_listView.Items.Count + compareMissingFiles_listView.Items.Count) + " files?", "", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                progressBar2.Minimum = 0;
                progressBar2.Maximum = compareExcessFiles_listView.Items.Count + compareMissingFiles_listView.Items.Count;
                progressBar2.Value = 0;
                progressBar2.Step = 1;

                foreach (ListViewItem item in compareExcessFiles_listView.Items)
                {
                    File.Delete(pathToArma3ServerMods_textBox.Text + item.Text.Split(':')[0]);

                    progressBar2.PerformStep();
                }

                foreach (ListViewItem item in compareMissingFiles_listView.Items)
                {
                    CheckPath(pathToArma3ServerMods_textBox.Text + item.Text.Split(':')[0]);

                    File.Copy(pathToArma3ClientMods_textBox.Text + item.Text.Split(':')[0], pathToArma3ServerMods_textBox.Text + item.Text.Split(':')[0], true);

                    progressBar2.PerformStep();
                }

                PrintMessage("Done.");
            }
        }

        private void changePathToArma3ServerMods_button_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.Description = "Select server mods folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                pathToArma3ServerMods_textBox.Text = chosenFolder.SelectedPath;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveXmlFile();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ReadPresetFile();

            if (Environment.CommandLine.ToLower().IndexOf("-createverifyfile") >= 0)
            {
                runSilent = true;
                CreateVerifyFile();
                System.Environment.Exit(1);
            }
        }

        private void refreshServer_button_Click(object sender, EventArgs e)
        {
            ReadPresetFile();
        }

        private void changePathToArma3ClientMods_button_Click(object sender, EventArgs e)
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

        private void changeRepoConfigPath_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();

            selectFile.Title = "Select repo config.";
            selectFile.Filter = "Repo Config (.json) | *.json";
            selectFile.RestoreDirectory = true;

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                repoConfigPath_textBox.Text = selectFile.FileName;

                ReadPresetFile();
            }
        }

        private void createVerifyFile_button_Click(object sender, EventArgs e)
        {
            CreateVerifyFile();
        }
    }
}
