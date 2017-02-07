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
            }
            catch (Exception e)
            {
                PrintMessage("Tool crashed while initializing. Try running it as administrator.\n\n" + e.Message);
                System.Environment.Exit(1);
            }
        }

        public class MurshunLauncherXmlSettings
        {
            public string modListLink = Directory.GetCurrentDirectory() + "\\MurshunRepoToolConfig.json";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CompareFolders();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(pathToModsFolder_textBox.Text) || !Directory.Exists(pathToSyncFolder_textBox.Text))
            {
                MessageBox.Show("Server or Sync folder doesn't exist.");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Sync " + (compareExcessFiles_listView.Items.Count + compareMissingFiles_listView.Items.Count) + " files?", "", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                progressBar2.Minimum = 0;
                progressBar2.Maximum = compareExcessFiles_listView.Items.Count + compareMissingFiles_listView.Items.Count;
                progressBar2.Value = 0;
                progressBar2.Step = 1;

                foreach (ListViewItem item in compareExcessFiles_listView.Items)
                {
                    File.Delete(pathToSyncFolder_textBox.Text + item.Text.Split(':')[0]);

                    progressBar2.PerformStep();
                }

                foreach (ListViewItem item in compareMissingFiles_listView.Items)
                {
                    CheckPath(pathToSyncFolder_textBox.Text + item.Text.Split(':')[0]);

                    File.Copy(pathToModsFolder_textBox.Text + item.Text.Split(':')[0], pathToSyncFolder_textBox.Text + item.Text.Split(':')[0], true);

                    progressBar2.PerformStep();
                }

                PrintMessage("Done.");
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

        private void createVerifyFile_button2_Click(object sender, EventArgs e)
        {
            CreateVerifyFile();
        }
    }
}
