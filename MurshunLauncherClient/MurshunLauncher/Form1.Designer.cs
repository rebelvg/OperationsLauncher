namespace MurshunLauncher
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.changePathToTeamSpeakFolder_button = new System.Windows.Forms.Button();
            this.teamSpeakFolder_textBox = new System.Windows.Forms.TextBox();
            this.saveSettings_button = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.refresh_button = new System.Windows.Forms.Button();
            this.linkLabel7 = new System.Windows.Forms.LinkLabel();
            this.linkLabel5 = new System.Windows.Forms.LinkLabel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.removeUncheckedMod_button = new System.Windows.Forms.Button();
            this.changePathToArma3ClientMods_button = new System.Windows.Forms.Button();
            this.changePathToArma3Client_button = new System.Windows.Forms.Button();
            this.pathToMods_textBox = new System.Windows.Forms.TextBox();
            this.pathToArma3_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.customMods_listView = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.presetMods_listView = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.advancedStartLine_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.defaultStartLine_textBox = new System.Windows.Forms.TextBox();
            this.joinTheServer_checkBox = new System.Windows.Forms.CheckBox();
            this.addCustomMod_button = new System.Windows.Forms.Button();
            this.xmlPath_textBox = new System.Windows.Forms.TextBox();
            this.launch_button = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.fullVerify_button = new System.Windows.Forms.Button();
            this.removeExcess_button = new System.Windows.Forms.Button();
            this.launcherFiles_textBox = new System.Windows.Forms.TextBox();
            this.verifyMods_button = new System.Windows.Forms.Button();
            this.modsFiles_textBox = new System.Windows.Forms.TextBox();
            this.excessFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.launcherFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.missingFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.modsFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1052, 740);
            this.tabControl1.TabIndex = 8;
            this.tabControl1.Tag = "";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.changePathToTeamSpeakFolder_button);
            this.tabPage1.Controls.Add(this.teamSpeakFolder_textBox);
            this.tabPage1.Controls.Add(this.saveSettings_button);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.refresh_button);
            this.tabPage1.Controls.Add(this.linkLabel7);
            this.tabPage1.Controls.Add(this.linkLabel5);
            this.tabPage1.Controls.Add(this.linkLabel4);
            this.tabPage1.Controls.Add(this.removeUncheckedMod_button);
            this.tabPage1.Controls.Add(this.changePathToArma3ClientMods_button);
            this.tabPage1.Controls.Add(this.changePathToArma3Client_button);
            this.tabPage1.Controls.Add(this.pathToMods_textBox);
            this.tabPage1.Controls.Add(this.pathToArma3_textBox);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.customMods_listView);
            this.tabPage1.Controls.Add(this.presetMods_listView);
            this.tabPage1.Controls.Add(this.advancedStartLine_textBox);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.defaultStartLine_textBox);
            this.tabPage1.Controls.Add(this.joinTheServer_checkBox);
            this.tabPage1.Controls.Add(this.addCustomMod_button);
            this.tabPage1.Controls.Add(this.xmlPath_textBox);
            this.tabPage1.Controls.Add(this.launch_button);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabPage1.Size = new System.Drawing.Size(1044, 711);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Client";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 13);
            this.label7.TabIndex = 44;
            this.label7.Text = "TeamSpeak AppData Folder";
            // 
            // changePathToTeamSpeakFolder_button
            // 
            this.changePathToTeamSpeakFolder_button.Location = new System.Drawing.Point(420, 139);
            this.changePathToTeamSpeakFolder_button.Name = "changePathToTeamSpeakFolder_button";
            this.changePathToTeamSpeakFolder_button.Size = new System.Drawing.Size(26, 20);
            this.changePathToTeamSpeakFolder_button.TabIndex = 43;
            this.changePathToTeamSpeakFolder_button.Text = "...";
            this.changePathToTeamSpeakFolder_button.UseVisualStyleBackColor = true;
            this.changePathToTeamSpeakFolder_button.Click += new System.EventHandler(this.changePathToTeamSpeakFolder_button_Click);
            // 
            // teamSpeakFolder_textBox
            // 
            this.teamSpeakFolder_textBox.Location = new System.Drawing.Point(7, 139);
            this.teamSpeakFolder_textBox.Name = "teamSpeakFolder_textBox";
            this.teamSpeakFolder_textBox.ReadOnly = true;
            this.teamSpeakFolder_textBox.Size = new System.Drawing.Size(406, 20);
            this.teamSpeakFolder_textBox.TabIndex = 42;
            // 
            // saveSettings_button
            // 
            this.saveSettings_button.Location = new System.Drawing.Point(803, 682);
            this.saveSettings_button.Name = "saveSettings_button";
            this.saveSettings_button.Size = new System.Drawing.Size(93, 23);
            this.saveSettings_button.TabIndex = 41;
            this.saveSettings_button.Text = "Save Settings";
            this.saveSettings_button.UseVisualStyleBackColor = true;
            this.saveSettings_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(146, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "Arma 3 Mods (BTsync Folder)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "Arma 3 (.exe)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Xml Path";
            // 
            // refresh_button
            // 
            this.refresh_button.Location = new System.Drawing.Point(660, 556);
            this.refresh_button.Name = "refresh_button";
            this.refresh_button.Size = new System.Drawing.Size(82, 23);
            this.refresh_button.TabIndex = 33;
            this.refresh_button.Text = "Verify";
            this.refresh_button.UseVisualStyleBackColor = true;
            this.refresh_button.Click += new System.EventHandler(this.refreshClient_button_Click);
            // 
            // linkLabel7
            // 
            this.linkLabel7.AutoSize = true;
            this.linkLabel7.Location = new System.Drawing.Point(401, 420);
            this.linkLabel7.Name = "linkLabel7";
            this.linkLabel7.Size = new System.Drawing.Size(45, 13);
            this.linkLabel7.TabIndex = 32;
            this.linkLabel7.TabStop = true;
            this.linkLabel7.Text = "-noLogs";
            this.linkLabel7.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel7_LinkClicked);
            // 
            // linkLabel5
            // 
            this.linkLabel5.AutoSize = true;
            this.linkLabel5.Location = new System.Drawing.Point(7, 483);
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.Size = new System.Drawing.Size(87, 13);
            this.linkLabel5.TabIndex = 30;
            this.linkLabel5.TabStop = true;
            this.linkLabel5.Text = "More Parameters";
            this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel5_LinkClicked);
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Location = new System.Drawing.Point(4, 674);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(40, 13);
            this.linkLabel4.TabIndex = 28;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "GitHub";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // removeUncheckedMod_button
            // 
            this.removeUncheckedMod_button.Location = new System.Drawing.Point(918, 556);
            this.removeUncheckedMod_button.Name = "removeUncheckedMod_button";
            this.removeUncheckedMod_button.Size = new System.Drawing.Size(120, 23);
            this.removeUncheckedMod_button.TabIndex = 27;
            this.removeUncheckedMod_button.Text = "Remove Unchecked";
            this.removeUncheckedMod_button.UseVisualStyleBackColor = true;
            this.removeUncheckedMod_button.Click += new System.EventHandler(this.removeUncheckedMod_button_Click);
            // 
            // changePathToArma3ClientMods_button
            // 
            this.changePathToArma3ClientMods_button.Location = new System.Drawing.Point(420, 100);
            this.changePathToArma3ClientMods_button.Name = "changePathToArma3ClientMods_button";
            this.changePathToArma3ClientMods_button.Size = new System.Drawing.Size(26, 20);
            this.changePathToArma3ClientMods_button.TabIndex = 26;
            this.changePathToArma3ClientMods_button.Text = "...";
            this.changePathToArma3ClientMods_button.UseVisualStyleBackColor = true;
            this.changePathToArma3ClientMods_button.Click += new System.EventHandler(this.button4_Click);
            // 
            // changePathToArma3Client_button
            // 
            this.changePathToArma3Client_button.Location = new System.Drawing.Point(420, 61);
            this.changePathToArma3Client_button.Name = "changePathToArma3Client_button";
            this.changePathToArma3Client_button.Size = new System.Drawing.Size(26, 20);
            this.changePathToArma3Client_button.TabIndex = 25;
            this.changePathToArma3Client_button.Text = "...";
            this.changePathToArma3Client_button.UseVisualStyleBackColor = true;
            this.changePathToArma3Client_button.Click += new System.EventHandler(this.button2_Click);
            // 
            // pathToMods_textBox
            // 
            this.pathToMods_textBox.Location = new System.Drawing.Point(7, 100);
            this.pathToMods_textBox.Name = "pathToMods_textBox";
            this.pathToMods_textBox.ReadOnly = true;
            this.pathToMods_textBox.Size = new System.Drawing.Size(406, 20);
            this.pathToMods_textBox.TabIndex = 24;
            // 
            // pathToArma3_textBox
            // 
            this.pathToArma3_textBox.Location = new System.Drawing.Point(7, 61);
            this.pathToArma3_textBox.Name = "pathToArma3_textBox";
            this.pathToArma3_textBox.ReadOnly = true;
            this.pathToArma3_textBox.Size = new System.Drawing.Size(406, 20);
            this.pathToArma3_textBox.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 695);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Version 0.999";
            // 
            // customMods_listView
            // 
            this.customMods_listView.CheckBoxes = true;
            this.customMods_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7});
            this.customMods_listView.FullRowSelect = true;
            this.customMods_listView.GridLines = true;
            this.customMods_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.customMods_listView.Location = new System.Drawing.Point(748, 6);
            this.customMods_listView.Name = "customMods_listView";
            this.customMods_listView.Size = new System.Drawing.Size(290, 544);
            this.customMods_listView.TabIndex = 16;
            this.customMods_listView.UseCompatibleStateImageBehavior = false;
            this.customMods_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Custom Mods";
            this.columnHeader7.Width = 286;
            // 
            // presetMods_listView
            // 
            this.presetMods_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6});
            this.presetMods_listView.FullRowSelect = true;
            this.presetMods_listView.GridLines = true;
            this.presetMods_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.presetMods_listView.Location = new System.Drawing.Point(452, 6);
            this.presetMods_listView.Name = "presetMods_listView";
            this.presetMods_listView.Size = new System.Drawing.Size(290, 544);
            this.presetMods_listView.TabIndex = 15;
            this.presetMods_listView.UseCompatibleStateImageBehavior = false;
            this.presetMods_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Preset Mods";
            this.columnHeader6.Width = 265;
            // 
            // advancedStartLine_textBox
            // 
            this.advancedStartLine_textBox.Location = new System.Drawing.Point(7, 460);
            this.advancedStartLine_textBox.Name = "advancedStartLine_textBox";
            this.advancedStartLine_textBox.Size = new System.Drawing.Size(439, 20);
            this.advancedStartLine_textBox.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 444);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Advanced Startup Line";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 381);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Default Startup Line";
            // 
            // defaultStartLine_textBox
            // 
            this.defaultStartLine_textBox.Location = new System.Drawing.Point(7, 397);
            this.defaultStartLine_textBox.Name = "defaultStartLine_textBox";
            this.defaultStartLine_textBox.ReadOnly = true;
            this.defaultStartLine_textBox.Size = new System.Drawing.Size(439, 20);
            this.defaultStartLine_textBox.TabIndex = 11;
            this.defaultStartLine_textBox.Text = "-world=empty -nosplash -skipintro -nofilepatching -showscripterrors -nologs";
            // 
            // joinTheServer_checkBox
            // 
            this.joinTheServer_checkBox.AutoSize = true;
            this.joinTheServer_checkBox.Location = new System.Drawing.Point(797, 659);
            this.joinTheServer_checkBox.Name = "joinTheServer_checkBox";
            this.joinTheServer_checkBox.Size = new System.Drawing.Size(99, 17);
            this.joinTheServer_checkBox.TabIndex = 9;
            this.joinTheServer_checkBox.Text = "Join on Launch";
            this.joinTheServer_checkBox.UseVisualStyleBackColor = true;
            // 
            // addCustomMod_button
            // 
            this.addCustomMod_button.Location = new System.Drawing.Point(804, 556);
            this.addCustomMod_button.Name = "addCustomMod_button";
            this.addCustomMod_button.Size = new System.Drawing.Size(108, 23);
            this.addCustomMod_button.TabIndex = 7;
            this.addCustomMod_button.Text = "Add Custom Mod";
            this.addCustomMod_button.UseVisualStyleBackColor = true;
            this.addCustomMod_button.Click += new System.EventHandler(this.button6_Click);
            // 
            // xmlPath_textBox
            // 
            this.xmlPath_textBox.Location = new System.Drawing.Point(7, 22);
            this.xmlPath_textBox.Name = "xmlPath_textBox";
            this.xmlPath_textBox.ReadOnly = true;
            this.xmlPath_textBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.xmlPath_textBox.Size = new System.Drawing.Size(439, 20);
            this.xmlPath_textBox.TabIndex = 1;
            // 
            // launch_button
            // 
            this.launch_button.Location = new System.Drawing.Point(902, 650);
            this.launch_button.Name = "launch_button";
            this.launch_button.Size = new System.Drawing.Size(136, 55);
            this.launch_button.TabIndex = 0;
            this.launch_button.Text = "LAUNCH CLIENT";
            this.launch_button.UseVisualStyleBackColor = true;
            this.launch_button.Click += new System.EventHandler(this.launch_button_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.progressBar1);
            this.tabPage2.Controls.Add(this.fullVerify_button);
            this.tabPage2.Controls.Add(this.removeExcess_button);
            this.tabPage2.Controls.Add(this.launcherFiles_textBox);
            this.tabPage2.Controls.Add(this.verifyMods_button);
            this.tabPage2.Controls.Add(this.modsFiles_textBox);
            this.tabPage2.Controls.Add(this.excessFiles_listView);
            this.tabPage2.Controls.Add(this.launcherFiles_listView);
            this.tabPage2.Controls.Add(this.missingFiles_listView);
            this.tabPage2.Controls.Add(this.modsFiles_listView);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1044, 711);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Client Verify";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(567, 6);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(471, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // fullVerify_button
            // 
            this.fullVerify_button.Location = new System.Drawing.Point(381, 6);
            this.fullVerify_button.Name = "fullVerify_button";
            this.fullVerify_button.Size = new System.Drawing.Size(180, 23);
            this.fullVerify_button.TabIndex = 10;
            this.fullVerify_button.Text = "Full Verify";
            this.fullVerify_button.UseVisualStyleBackColor = true;
            this.fullVerify_button.Click += new System.EventHandler(this.fullVerify_button_Click);
            // 
            // removeExcess_button
            // 
            this.removeExcess_button.Location = new System.Drawing.Point(195, 6);
            this.removeExcess_button.Name = "removeExcess_button";
            this.removeExcess_button.Size = new System.Drawing.Size(180, 23);
            this.removeExcess_button.TabIndex = 9;
            this.removeExcess_button.Text = "Remove Excess";
            this.removeExcess_button.UseVisualStyleBackColor = true;
            this.removeExcess_button.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // launcherFiles_textBox
            // 
            this.launcherFiles_textBox.Location = new System.Drawing.Point(534, 36);
            this.launcherFiles_textBox.Name = "launcherFiles_textBox";
            this.launcherFiles_textBox.ReadOnly = true;
            this.launcherFiles_textBox.Size = new System.Drawing.Size(507, 20);
            this.launcherFiles_textBox.TabIndex = 8;
            this.launcherFiles_textBox.Text = "MurshunLauncherFiles.json";
            // 
            // verifyMods_button
            // 
            this.verifyMods_button.Location = new System.Drawing.Point(9, 6);
            this.verifyMods_button.Name = "verifyMods_button";
            this.verifyMods_button.Size = new System.Drawing.Size(180, 23);
            this.verifyMods_button.TabIndex = 4;
            this.verifyMods_button.Text = "Verify";
            this.verifyMods_button.UseVisualStyleBackColor = true;
            this.verifyMods_button.Click += new System.EventHandler(this.button3_Click);
            // 
            // modsFiles_textBox
            // 
            this.modsFiles_textBox.Location = new System.Drawing.Point(9, 36);
            this.modsFiles_textBox.Name = "modsFiles_textBox";
            this.modsFiles_textBox.ReadOnly = true;
            this.modsFiles_textBox.Size = new System.Drawing.Size(507, 20);
            this.modsFiles_textBox.TabIndex = 0;
            this.modsFiles_textBox.Text = "Client Mods";
            // 
            // excessFiles_listView
            // 
            this.excessFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.excessFiles_listView.FullRowSelect = true;
            this.excessFiles_listView.GridLines = true;
            this.excessFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.excessFiles_listView.Location = new System.Drawing.Point(534, 302);
            this.excessFiles_listView.Name = "excessFiles_listView";
            this.excessFiles_listView.Size = new System.Drawing.Size(507, 406);
            this.excessFiles_listView.TabIndex = 6;
            this.excessFiles_listView.UseCompatibleStateImageBehavior = false;
            this.excessFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Excess Files (Path:Size)";
            this.columnHeader3.Width = 503;
            // 
            // launcherFiles_listView
            // 
            this.launcherFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this.launcherFiles_listView.FullRowSelect = true;
            this.launcherFiles_listView.GridLines = true;
            this.launcherFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.launcherFiles_listView.Location = new System.Drawing.Point(534, 62);
            this.launcherFiles_listView.Name = "launcherFiles_listView";
            this.launcherFiles_listView.Size = new System.Drawing.Size(507, 234);
            this.launcherFiles_listView.TabIndex = 7;
            this.launcherFiles_listView.UseCompatibleStateImageBehavior = false;
            this.launcherFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Files (Path:Size)";
            this.columnHeader4.Width = 503;
            // 
            // missingFiles_listView
            // 
            this.missingFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.missingFiles_listView.FullRowSelect = true;
            this.missingFiles_listView.GridLines = true;
            this.missingFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.missingFiles_listView.Location = new System.Drawing.Point(9, 302);
            this.missingFiles_listView.Name = "missingFiles_listView";
            this.missingFiles_listView.Size = new System.Drawing.Size(507, 406);
            this.missingFiles_listView.TabIndex = 5;
            this.missingFiles_listView.UseCompatibleStateImageBehavior = false;
            this.missingFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Missing Files (Path:Size)";
            this.columnHeader2.Width = 503;
            // 
            // modsFiles_listView
            // 
            this.modsFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.modsFiles_listView.FullRowSelect = true;
            this.modsFiles_listView.GridLines = true;
            this.modsFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.modsFiles_listView.Location = new System.Drawing.Point(9, 62);
            this.modsFiles_listView.Name = "modsFiles_listView";
            this.modsFiles_listView.Size = new System.Drawing.Size(507, 234);
            this.modsFiles_listView.TabIndex = 2;
            this.modsFiles_listView.UseCompatibleStateImageBehavior = false;
            this.modsFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Files (Path:Size)";
            this.columnHeader1.Width = 503;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 744);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Murshun Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button launch_button;
        private System.Windows.Forms.TextBox xmlPath_textBox;
        private System.Windows.Forms.Button addCustomMod_button;
        private System.Windows.Forms.CheckBox joinTheServer_checkBox;
        private System.Windows.Forms.TextBox defaultStartLine_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox advancedStartLine_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView customMods_listView;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ListView presetMods_listView;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button changePathToArma3ClientMods_button;
        private System.Windows.Forms.Button changePathToArma3Client_button;
        private System.Windows.Forms.TextBox pathToMods_textBox;
        private System.Windows.Forms.TextBox pathToArma3_textBox;
        private System.Windows.Forms.TextBox launcherFiles_textBox;
        private System.Windows.Forms.Button verifyMods_button;
        private System.Windows.Forms.ListView missingFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView launcherFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox modsFiles_textBox;
        private System.Windows.Forms.ListView modsFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView excessFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button removeExcess_button;
        private System.Windows.Forms.Button removeUncheckedMod_button;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.LinkLabel linkLabel5;
        private System.Windows.Forms.LinkLabel linkLabel7;
        private System.Windows.Forms.Button refresh_button;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button saveSettings_button;
        private System.Windows.Forms.Button fullVerify_button;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button changePathToTeamSpeakFolder_button;
        private System.Windows.Forms.TextBox teamSpeakFolder_textBox;
        private System.Windows.Forms.Timer timer1;
    }
}

