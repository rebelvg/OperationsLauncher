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
                    MessageBox.Show("Repo Tool is already running.");
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
            }
            catch (Exception e)
            {
                MessageBox.Show("Tool crashed while initializing. Try running it as administrator.\n\n" + e.Message);
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

        private async void button10_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(pathToModsFolder_textBox.Text) || !Directory.Exists(pathToSyncFolder_textBox.Text))
            {
                MessageBox.Show("Server or Sync folder doesn't exist.");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Sync " + (compareExcessFiles_listView.Items.Count + compareMissingFiles_listView.Items.Count) + " files?", "", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = compareExcessFiles_listView.Items.Count + compareMissingFiles_listView.Items.Count;
                progressBar1.Value = 0;
                progressBar1.Step = 1;

                LockInterface("Copying...");

                foreach (ListViewItem item in compareExcessFiles_listView.Items)
                {
                    ChangeHeader("Deleting... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + item.Text);

                    await Task.Run(() =>
                    {
                        File.Delete(pathToSyncFolder_textBox.Text + item.Text.Split(':')[0]);
                    });

                    progressBar1.PerformStep();
                }

                foreach (ListViewItem item in compareMissingFiles_listView.Items)
                {
                    ChangeHeader("Copying... (" + progressBar1.Value + "/" + progressBar1.Maximum + ") - " + item.Text);

                    CheckPath(pathToSyncFolder_textBox.Text + item.Text.Split(':')[0]);

                    await Task.Run(() =>
                    {
                        File.Copy(pathToModsFolder_textBox.Text + item.Text.Split(':')[0], pathToSyncFolder_textBox.Text + item.Text.Split(':')[0], true);
                    });

                    progressBar1.PerformStep();
                }

                UnlockInterface();

                MessageBox.Show("Done.");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveXmlFile();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            refreshServer_button_Click(null, null);
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
            selectFile.InitialDirectory = Path.GetDirectoryName(repoConfigPath_textBox.Text);

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                repoConfigPath_textBox.Text = selectFile.FileName;

                refreshServer_button_Click(null, null);
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
