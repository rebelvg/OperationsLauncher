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

namespace OperationsLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();

            if (!args.Contains("--debug"))
            {
                debugMode = false;
            }
            else
            {
                debugMode = true;
            }

            try
            {
                if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName)).Length > 1)
                {
                    MessageBox.Show("Launcher is already running.");
                    System.Environment.Exit(1);
                }

                string iniDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OperationsLauncher";

                xmlPath_textBox.Text = iniDirectoryPath + "\\OperationsLauncher.json";

                if (!Directory.Exists(iniDirectoryPath))
                {
                    try
                    {
                        Directory.CreateDirectory(iniDirectoryPath);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Couldn't create a folder at " + iniDirectoryPath, error.Message);
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
                        var LauncherSettingsJson = new LauncherSettingsJson();

                        string json = JsonConvert.SerializeObject(LauncherSettingsJson, Formatting.Indented);

                        File.WriteAllText(xmlPath_textBox.Text, json);

                        ReadXmlFile();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Saving settings failed. " + error.Message);
                    }
                }

                label3.Text = "Version " + launcherVersion;
            }
            catch (Exception e)
            {
                MessageBox.Show("Launcher crashed while initializing. Try running it as administrator.\n\n" + e.Message);
                System.Environment.Exit(1);
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await VerifyMods(false);
        }

        private async void launch_button_Click(object sender, EventArgs e)
        {
            if (!await VerifyMods(false))
            {
                if (debugMode)
                {
                    DialogResult dialogResult = MessageBox.Show("Verify returned errors.", "Launch anyway?", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Verify returned errors. Launch canceled.");
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

            foreach (ListViewItem X in presetMods_listView.Items)
            {
                modLine = modLine + pathToMods_textBox.Text + "\\" + X.Text + ";";
            }

            foreach (ListViewItem X in customMods_listView.CheckedItems)
            {
                modLine = modLine + X.Text + ";";
            }

            modLine = modLine + "\"";

            if (joinTheServer_checkBox.Checked)
            {
                if (repoConfigJson.serverHost != "")
                {
                    modLine = modLine + " -connect=" + repoConfigJson.serverHost;

                    if (repoConfigJson.serverPassword != "")
                        modLine = modLine + " -password=" + repoConfigJson.serverPassword;
                }
            }

            if (Environment.Is64BitOperatingSystem)
            {
                if (!pathToArma3_textBox.Text.Contains("x64"))
                {
                    DialogResult dialogResult = MessageBox.Show("You're trying to launch x86 executable on a x64 operating system.", "Launch anyway?", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            if (File.Exists(pathToArma3_textBox.Text))
            {
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = pathToArma3_textBox.Text;
                myProcess.StartInfo.Arguments = modLine;
                myProcess.Start();

                launch_button.Enabled = false;
            }
            else
            {
                MessageBox.Show(Path.GetFileName(pathToArma3_textBox.Text) + " not found.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.Description = "Select custom mod folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                customMods_listView.Items.Add(chosenFolder.SelectedPath);

                refreshClient_button_Click(null, null);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();

            selectFile.Title = "Select arma3.exe";
            selectFile.Filter = "Executable File (.exe) | *.exe";
            selectFile.InitialDirectory = Path.GetDirectoryName(pathToArma3_textBox.Text);

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                pathToArma3_textBox.Text = selectFile.FileName;

                refreshClient_button_Click(null, null);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.UseDescriptionForTitle = true;
            chosenFolder.Description = "Select repo folder.";
            chosenFolder.SelectedPath = pathToMods_textBox.Text;

            if (chosenFolder.ShowDialog().Value)
            {
                pathToMods_textBox.Text = chosenFolder.SelectedPath;

                refreshClient_button_Click(null, null);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Remove " + excessFiles_listView.Items.Count + " excess files?", "", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                foreach (ListViewItem X in excessFiles_listView.Items)
                {
                    File.Delete(pathToMods_textBox.Text + X.Text.Split(':')[0]);
                }

                MessageBox.Show("Done.");
            }
        }

        private void removeUncheckedMod_button_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in customMods_listView.Items)
            {
                if (!item.Checked)
                    item.Remove();
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/rebelvg/OperationsLauncher");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveXmlFile();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://community.bistudio.com/wiki/Arma_3_Startup_Parameters");
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
            refreshClient_button_Click(null, null);
        }

        private async void refreshClient_button_Click(object sender, EventArgs e)
        {
            CheckSyncFolderSize();

            await VerifyMods(false);
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            SaveXmlFile();
        }

        private async void fullVerify_button_Click(object sender, EventArgs e)
        {
            await VerifyMods(true);
        }

        private void changePathToTeamSpeakFolder_button_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.UseDescriptionForTitle = true;
            chosenFolder.Description = "Select teamspeak folder.";
            chosenFolder.UseDescriptionForTitle = true;

            if (chosenFolder.ShowDialog().Value)
            {
                if (Directory.Exists(chosenFolder.SelectedPath + @"\plugins"))
                {
                    teamSpeakFolder_textBox.Text = chosenFolder.SelectedPath;

                    refreshClient_button_Click(null, null);
                }
                else
                {
                    MessageBox.Show("This folder doesn't have plugins folder in it.");
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(pathToArma3_textBox.Text));

            if (processes.Count() > 0)
                launch_button.Enabled = false;
            else
                launch_button.Enabled = true;
        }

        private void steamWorkshopFolderFindButton_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog chosenFolder = new VistaFolderBrowserDialog();
            chosenFolder.UseDescriptionForTitle = true;
            chosenFolder.Description = "Select !Workshop folder.";
            chosenFolder.SelectedPath = steamWorkshopFolderTextBox.Text;

            if (chosenFolder.ShowDialog().Value)
            {
                steamWorkshopFolderTextBox.Text = chosenFolder.SelectedPath;

                refreshClient_button_Click(null, null);
            }
        }
    }
}
