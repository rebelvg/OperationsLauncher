namespace OperationsLauncherServer
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
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.xmlPath_textBox = new System.Windows.Forms.TextBox();
            this.saveSettings_button = new System.Windows.Forms.Button();
            this.linkLabel8 = new System.Windows.Forms.LinkLabel();
            this.label13 = new System.Windows.Forms.Label();
            this.defaultStartLineServer_textBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.refresh_button = new System.Windows.Forms.Button();
            this.closeServer_button = new System.Windows.Forms.Button();
            this.hideWindow_checkBox = new System.Windows.Forms.CheckBox();
            this.removeUncheckedServerMod_button = new System.Windows.Forms.Button();
            this.addCustomServerMod = new System.Windows.Forms.Button();
            this.serverProfileName_textBox = new System.Windows.Forms.TextBox();
            this.changeServerProfiles_button = new System.Windows.Forms.Button();
            this.changeServerCfg_button = new System.Windows.Forms.Button();
            this.changeServerConfig_button = new System.Windows.Forms.Button();
            this.changePathToArma3ServerMods_button = new System.Windows.Forms.Button();
            this.changePathToArma3Server_button = new System.Windows.Forms.Button();
            this.launch_button = new System.Windows.Forms.Button();
            this.serverProfiles_textBox = new System.Windows.Forms.TextBox();
            this.customMods_listView = new System.Windows.Forms.ListView();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.presetMods_listView = new System.Windows.Forms.ListView();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.serverCfg_textBox = new System.Windows.Forms.TextBox();
            this.serverConfig_textBox = new System.Windows.Forms.TextBox();
            this.pathToMods_textBox = new System.Windows.Forms.TextBox();
            this.pathToArma3_textBox = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.fullVerify_button = new System.Windows.Forms.Button();
            this.removeExcess_button = new System.Windows.Forms.Button();
            this.launcherFiles_textBox = new System.Windows.Forms.TextBox();
            this.verify_button = new System.Windows.Forms.Button();
            this.modsFiles_textBox = new System.Windows.Forms.TextBox();
            this.excessFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.launcherFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.missingFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.modsFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.steamWorkshopFolderFindButton = new System.Windows.Forms.Button();
            this.steamWorkshopFolderTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1052, 740);
            this.tabControl1.TabIndex = 8;
            this.tabControl1.Tag = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.steamWorkshopFolderFindButton);
            this.tabPage3.Controls.Add(this.steamWorkshopFolderTextBox);
            this.tabPage3.Controls.Add(this.linkLabel4);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.xmlPath_textBox);
            this.tabPage3.Controls.Add(this.saveSettings_button);
            this.tabPage3.Controls.Add(this.linkLabel8);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.defaultStartLineServer_textBox);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.refresh_button);
            this.tabPage3.Controls.Add(this.closeServer_button);
            this.tabPage3.Controls.Add(this.hideWindow_checkBox);
            this.tabPage3.Controls.Add(this.removeUncheckedServerMod_button);
            this.tabPage3.Controls.Add(this.addCustomServerMod);
            this.tabPage3.Controls.Add(this.serverProfileName_textBox);
            this.tabPage3.Controls.Add(this.changeServerProfiles_button);
            this.tabPage3.Controls.Add(this.changeServerCfg_button);
            this.tabPage3.Controls.Add(this.changeServerConfig_button);
            this.tabPage3.Controls.Add(this.changePathToArma3ServerMods_button);
            this.tabPage3.Controls.Add(this.changePathToArma3Server_button);
            this.tabPage3.Controls.Add(this.launch_button);
            this.tabPage3.Controls.Add(this.serverProfiles_textBox);
            this.tabPage3.Controls.Add(this.customMods_listView);
            this.tabPage3.Controls.Add(this.presetMods_listView);
            this.tabPage3.Controls.Add(this.serverCfg_textBox);
            this.tabPage3.Controls.Add(this.serverConfig_textBox);
            this.tabPage3.Controls.Add(this.pathToMods_textBox);
            this.tabPage3.Controls.Add(this.pathToArma3_textBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1044, 711);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Server";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Location = new System.Drawing.Point(4, 674);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(40, 13);
            this.linkLabel4.TabIndex = 59;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "GitHub";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 695);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 58;
            this.label3.Text = "Version 0.0.00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 57;
            this.label4.Text = "Xml Path";
            // 
            // xmlPath_textBox
            // 
            this.xmlPath_textBox.Location = new System.Drawing.Point(7, 22);
            this.xmlPath_textBox.Name = "xmlPath_textBox";
            this.xmlPath_textBox.ReadOnly = true;
            this.xmlPath_textBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.xmlPath_textBox.Size = new System.Drawing.Size(439, 20);
            this.xmlPath_textBox.TabIndex = 52;
            // 
            // saveSettings_button
            // 
            this.saveSettings_button.Location = new System.Drawing.Point(803, 682);
            this.saveSettings_button.Name = "saveSettings_button";
            this.saveSettings_button.Size = new System.Drawing.Size(93, 23);
            this.saveSettings_button.TabIndex = 51;
            this.saveSettings_button.Text = "Save Settings";
            this.saveSettings_button.UseVisualStyleBackColor = true;
            // 
            // linkLabel8
            // 
            this.linkLabel8.AutoSize = true;
            this.linkLabel8.Location = new System.Drawing.Point(401, 420);
            this.linkLabel8.Name = "linkLabel8";
            this.linkLabel8.Size = new System.Drawing.Size(45, 13);
            this.linkLabel8.TabIndex = 45;
            this.linkLabel8.TabStop = true;
            this.linkLabel8.Text = "-noLogs";
            this.linkLabel8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel8_LinkClicked);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 381);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 13);
            this.label13.TabIndex = 44;
            this.label13.Text = "Default Startup Line";
            // 
            // defaultStartLineServer_textBox
            // 
            this.defaultStartLineServer_textBox.Location = new System.Drawing.Point(7, 397);
            this.defaultStartLineServer_textBox.Name = "defaultStartLineServer_textBox";
            this.defaultStartLineServer_textBox.ReadOnly = true;
            this.defaultStartLineServer_textBox.Size = new System.Drawing.Size(439, 20);
            this.defaultStartLineServer_textBox.TabIndex = 43;
            this.defaultStartLineServer_textBox.Text = "-port=2302 -nofilepatching -nologs";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 279);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 42;
            this.label12.Text = "Profile Name";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 240);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 13);
            this.label11.TabIndex = 41;
            this.label11.Text = "Profiles Folder";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 201);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 13);
            this.label10.TabIndex = 40;
            this.label10.Text = "Basic Config (.cfg)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 162);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 13);
            this.label9.TabIndex = 39;
            this.label9.Text = "Server Config (.cfg)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 38;
            this.label8.Text = "Server Mods";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "Server (.exe)";
            // 
            // refresh_button
            // 
            this.refresh_button.Location = new System.Drawing.Point(660, 556);
            this.refresh_button.Name = "refresh_button";
            this.refresh_button.Size = new System.Drawing.Size(82, 23);
            this.refresh_button.TabIndex = 36;
            this.refresh_button.Text = "Verify";
            this.refresh_button.UseVisualStyleBackColor = true;
            this.refresh_button.Click += new System.EventHandler(this.refreshServer_button_Click);
            // 
            // closeServer_button
            // 
            this.closeServer_button.Location = new System.Drawing.Point(902, 619);
            this.closeServer_button.Name = "closeServer_button";
            this.closeServer_button.Size = new System.Drawing.Size(136, 25);
            this.closeServer_button.TabIndex = 35;
            this.closeServer_button.Text = "CLOSE SERVER";
            this.closeServer_button.UseVisualStyleBackColor = true;
            this.closeServer_button.Click += new System.EventHandler(this.closeServer_button_Click);
            // 
            // hideWindow_checkBox
            // 
            this.hideWindow_checkBox.AutoSize = true;
            this.hideWindow_checkBox.Location = new System.Drawing.Point(803, 659);
            this.hideWindow_checkBox.Name = "hideWindow_checkBox";
            this.hideWindow_checkBox.Size = new System.Drawing.Size(90, 17);
            this.hideWindow_checkBox.TabIndex = 34;
            this.hideWindow_checkBox.Text = "Hide Window";
            this.hideWindow_checkBox.UseVisualStyleBackColor = true;
            // 
            // removeUncheckedServerMod_button
            // 
            this.removeUncheckedServerMod_button.Location = new System.Drawing.Point(918, 556);
            this.removeUncheckedServerMod_button.Name = "removeUncheckedServerMod_button";
            this.removeUncheckedServerMod_button.Size = new System.Drawing.Size(120, 23);
            this.removeUncheckedServerMod_button.TabIndex = 33;
            this.removeUncheckedServerMod_button.Text = "Remove Unchecked";
            this.removeUncheckedServerMod_button.UseVisualStyleBackColor = true;
            this.removeUncheckedServerMod_button.Click += new System.EventHandler(this.removeUncheckedServerMod_button_Click);
            // 
            // addCustomServerMod
            // 
            this.addCustomServerMod.Location = new System.Drawing.Point(804, 556);
            this.addCustomServerMod.Name = "addCustomServerMod";
            this.addCustomServerMod.Size = new System.Drawing.Size(108, 23);
            this.addCustomServerMod.TabIndex = 32;
            this.addCustomServerMod.Text = "Add Custom Mod";
            this.addCustomServerMod.UseVisualStyleBackColor = true;
            this.addCustomServerMod.Click += new System.EventHandler(this.addCustomServerMod_Click);
            // 
            // serverProfileName_textBox
            // 
            this.serverProfileName_textBox.Location = new System.Drawing.Point(7, 295);
            this.serverProfileName_textBox.Name = "serverProfileName_textBox";
            this.serverProfileName_textBox.Size = new System.Drawing.Size(439, 20);
            this.serverProfileName_textBox.TabIndex = 31;
            // 
            // changeServerProfiles_button
            // 
            this.changeServerProfiles_button.Location = new System.Drawing.Point(420, 256);
            this.changeServerProfiles_button.Name = "changeServerProfiles_button";
            this.changeServerProfiles_button.Size = new System.Drawing.Size(26, 20);
            this.changeServerProfiles_button.TabIndex = 30;
            this.changeServerProfiles_button.Text = "...";
            this.changeServerProfiles_button.UseVisualStyleBackColor = true;
            this.changeServerProfiles_button.Click += new System.EventHandler(this.changeServerProfiles_button_Click);
            // 
            // changeServerCfg_button
            // 
            this.changeServerCfg_button.Location = new System.Drawing.Point(420, 217);
            this.changeServerCfg_button.Name = "changeServerCfg_button";
            this.changeServerCfg_button.Size = new System.Drawing.Size(26, 20);
            this.changeServerCfg_button.TabIndex = 29;
            this.changeServerCfg_button.Text = "...";
            this.changeServerCfg_button.UseVisualStyleBackColor = true;
            this.changeServerCfg_button.Click += new System.EventHandler(this.changeServerCfg_button_Click);
            // 
            // changeServerConfig_button
            // 
            this.changeServerConfig_button.Location = new System.Drawing.Point(420, 178);
            this.changeServerConfig_button.Name = "changeServerConfig_button";
            this.changeServerConfig_button.Size = new System.Drawing.Size(26, 20);
            this.changeServerConfig_button.TabIndex = 28;
            this.changeServerConfig_button.Text = "...";
            this.changeServerConfig_button.UseVisualStyleBackColor = true;
            this.changeServerConfig_button.Click += new System.EventHandler(this.changeServerConfig_button_Click);
            // 
            // changePathToArma3ServerMods_button
            // 
            this.changePathToArma3ServerMods_button.Location = new System.Drawing.Point(420, 100);
            this.changePathToArma3ServerMods_button.Name = "changePathToArma3ServerMods_button";
            this.changePathToArma3ServerMods_button.Size = new System.Drawing.Size(26, 20);
            this.changePathToArma3ServerMods_button.TabIndex = 27;
            this.changePathToArma3ServerMods_button.Text = "...";
            this.changePathToArma3ServerMods_button.UseVisualStyleBackColor = true;
            this.changePathToArma3ServerMods_button.Click += new System.EventHandler(this.changePathToArma3ServerMods_button_Click);
            // 
            // changePathToArma3Server_button
            // 
            this.changePathToArma3Server_button.Location = new System.Drawing.Point(420, 61);
            this.changePathToArma3Server_button.Name = "changePathToArma3Server_button";
            this.changePathToArma3Server_button.Size = new System.Drawing.Size(26, 20);
            this.changePathToArma3Server_button.TabIndex = 26;
            this.changePathToArma3Server_button.Text = "...";
            this.changePathToArma3Server_button.UseVisualStyleBackColor = true;
            this.changePathToArma3Server_button.Click += new System.EventHandler(this.changePathToArma3Server_button_Click);
            // 
            // launch_button
            // 
            this.launch_button.Location = new System.Drawing.Point(902, 650);
            this.launch_button.Name = "launch_button";
            this.launch_button.Size = new System.Drawing.Size(136, 55);
            this.launch_button.TabIndex = 12;
            this.launch_button.Text = "LAUNCH SERVER";
            this.launch_button.UseVisualStyleBackColor = true;
            this.launch_button.Click += new System.EventHandler(this.button8_Click);
            // 
            // serverProfiles_textBox
            // 
            this.serverProfiles_textBox.Location = new System.Drawing.Point(7, 256);
            this.serverProfiles_textBox.Name = "serverProfiles_textBox";
            this.serverProfiles_textBox.ReadOnly = true;
            this.serverProfiles_textBox.Size = new System.Drawing.Size(406, 20);
            this.serverProfiles_textBox.TabIndex = 11;
            // 
            // customMods_listView
            // 
            this.customMods_listView.CheckBoxes = true;
            this.customMods_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9});
            this.customMods_listView.FullRowSelect = true;
            this.customMods_listView.GridLines = true;
            this.customMods_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.customMods_listView.HideSelection = false;
            this.customMods_listView.Location = new System.Drawing.Point(748, 6);
            this.customMods_listView.MultiSelect = false;
            this.customMods_listView.Name = "customMods_listView";
            this.customMods_listView.Size = new System.Drawing.Size(290, 544);
            this.customMods_listView.TabIndex = 5;
            this.customMods_listView.UseCompatibleStateImageBehavior = false;
            this.customMods_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Custom Mods";
            this.columnHeader9.Width = 286;
            // 
            // presetMods_listView
            // 
            this.presetMods_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8});
            this.presetMods_listView.FullRowSelect = true;
            this.presetMods_listView.GridLines = true;
            this.presetMods_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.presetMods_listView.HideSelection = false;
            this.presetMods_listView.Location = new System.Drawing.Point(452, 6);
            this.presetMods_listView.MultiSelect = false;
            this.presetMods_listView.Name = "presetMods_listView";
            this.presetMods_listView.Size = new System.Drawing.Size(290, 544);
            this.presetMods_listView.TabIndex = 4;
            this.presetMods_listView.UseCompatibleStateImageBehavior = false;
            this.presetMods_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Preset Mods";
            this.columnHeader8.Width = 265;
            // 
            // serverCfg_textBox
            // 
            this.serverCfg_textBox.Location = new System.Drawing.Point(7, 217);
            this.serverCfg_textBox.Name = "serverCfg_textBox";
            this.serverCfg_textBox.ReadOnly = true;
            this.serverCfg_textBox.Size = new System.Drawing.Size(406, 20);
            this.serverCfg_textBox.TabIndex = 3;
            // 
            // serverConfig_textBox
            // 
            this.serverConfig_textBox.Location = new System.Drawing.Point(7, 178);
            this.serverConfig_textBox.Name = "serverConfig_textBox";
            this.serverConfig_textBox.ReadOnly = true;
            this.serverConfig_textBox.Size = new System.Drawing.Size(406, 20);
            this.serverConfig_textBox.TabIndex = 2;
            // 
            // pathToMods_textBox
            // 
            this.pathToMods_textBox.Location = new System.Drawing.Point(7, 100);
            this.pathToMods_textBox.Name = "pathToMods_textBox";
            this.pathToMods_textBox.ReadOnly = true;
            this.pathToMods_textBox.Size = new System.Drawing.Size(406, 20);
            this.pathToMods_textBox.TabIndex = 1;
            // 
            // pathToArma3_textBox
            // 
            this.pathToArma3_textBox.Location = new System.Drawing.Point(7, 61);
            this.pathToArma3_textBox.Name = "pathToArma3_textBox";
            this.pathToArma3_textBox.ReadOnly = true;
            this.pathToArma3_textBox.Size = new System.Drawing.Size(406, 20);
            this.pathToArma3_textBox.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.progressBar1);
            this.tabPage4.Controls.Add(this.fullVerify_button);
            this.tabPage4.Controls.Add(this.removeExcess_button);
            this.tabPage4.Controls.Add(this.launcherFiles_textBox);
            this.tabPage4.Controls.Add(this.verify_button);
            this.tabPage4.Controls.Add(this.modsFiles_textBox);
            this.tabPage4.Controls.Add(this.excessFiles_listView);
            this.tabPage4.Controls.Add(this.launcherFiles_listView);
            this.tabPage4.Controls.Add(this.missingFiles_listView);
            this.tabPage4.Controls.Add(this.modsFiles_listView);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1044, 711);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Server Verify";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(567, 6);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(471, 23);
            this.progressBar1.TabIndex = 19;
            // 
            // fullVerify_button
            // 
            this.fullVerify_button.Location = new System.Drawing.Point(381, 6);
            this.fullVerify_button.Name = "fullVerify_button";
            this.fullVerify_button.Size = new System.Drawing.Size(180, 23);
            this.fullVerify_button.TabIndex = 18;
            this.fullVerify_button.Text = "Full Verify";
            this.fullVerify_button.UseVisualStyleBackColor = true;
            this.fullVerify_button.Click += new System.EventHandler(this.createVerifyFile_button_Click);
            // 
            // removeExcess_button
            // 
            this.removeExcess_button.Location = new System.Drawing.Point(195, 6);
            this.removeExcess_button.Name = "removeExcess_button";
            this.removeExcess_button.Size = new System.Drawing.Size(180, 23);
            this.removeExcess_button.TabIndex = 17;
            this.removeExcess_button.Text = "Remove Excess";
            this.removeExcess_button.UseVisualStyleBackColor = true;
            this.removeExcess_button.Click += new System.EventHandler(this.button10_Click);
            // 
            // launcherFiles_textBox
            // 
            this.launcherFiles_textBox.Location = new System.Drawing.Point(534, 36);
            this.launcherFiles_textBox.Name = "launcherFiles_textBox";
            this.launcherFiles_textBox.ReadOnly = true;
            this.launcherFiles_textBox.Size = new System.Drawing.Size(507, 20);
            this.launcherFiles_textBox.TabIndex = 16;
            this.launcherFiles_textBox.Text = "OperationsLauncherFiles.json";
            // 
            // verify_button
            // 
            this.verify_button.Location = new System.Drawing.Point(9, 6);
            this.verify_button.Name = "verify_button";
            this.verify_button.Size = new System.Drawing.Size(180, 23);
            this.verify_button.TabIndex = 12;
            this.verify_button.Text = "Verify";
            this.verify_button.UseVisualStyleBackColor = true;
            this.verify_button.Click += new System.EventHandler(this.button9_Click);
            // 
            // modsFiles_textBox
            // 
            this.modsFiles_textBox.Location = new System.Drawing.Point(9, 36);
            this.modsFiles_textBox.Name = "modsFiles_textBox";
            this.modsFiles_textBox.ReadOnly = true;
            this.modsFiles_textBox.Size = new System.Drawing.Size(507, 20);
            this.modsFiles_textBox.TabIndex = 9;
            this.modsFiles_textBox.Text = "Server Mods";
            // 
            // excessFiles_listView
            // 
            this.excessFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader10});
            this.excessFiles_listView.FullRowSelect = true;
            this.excessFiles_listView.GridLines = true;
            this.excessFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.excessFiles_listView.HideSelection = false;
            this.excessFiles_listView.Location = new System.Drawing.Point(534, 302);
            this.excessFiles_listView.Name = "excessFiles_listView";
            this.excessFiles_listView.Size = new System.Drawing.Size(507, 406);
            this.excessFiles_listView.TabIndex = 14;
            this.excessFiles_listView.UseCompatibleStateImageBehavior = false;
            this.excessFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Excess Files (Path:Size)";
            this.columnHeader10.Width = 503;
            // 
            // launcherFiles_listView
            // 
            this.launcherFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11});
            this.launcherFiles_listView.FullRowSelect = true;
            this.launcherFiles_listView.GridLines = true;
            this.launcherFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.launcherFiles_listView.HideSelection = false;
            this.launcherFiles_listView.Location = new System.Drawing.Point(534, 62);
            this.launcherFiles_listView.Name = "launcherFiles_listView";
            this.launcherFiles_listView.Size = new System.Drawing.Size(507, 234);
            this.launcherFiles_listView.TabIndex = 15;
            this.launcherFiles_listView.UseCompatibleStateImageBehavior = false;
            this.launcherFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Files (Path:Size)";
            this.columnHeader11.Width = 503;
            // 
            // missingFiles_listView
            // 
            this.missingFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader12});
            this.missingFiles_listView.FullRowSelect = true;
            this.missingFiles_listView.GridLines = true;
            this.missingFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.missingFiles_listView.HideSelection = false;
            this.missingFiles_listView.Location = new System.Drawing.Point(9, 302);
            this.missingFiles_listView.Name = "missingFiles_listView";
            this.missingFiles_listView.Size = new System.Drawing.Size(507, 406);
            this.missingFiles_listView.TabIndex = 13;
            this.missingFiles_listView.UseCompatibleStateImageBehavior = false;
            this.missingFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Missing Files (Path:Size)";
            this.columnHeader12.Width = 503;
            // 
            // modsFiles_listView
            // 
            this.modsFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader13});
            this.modsFiles_listView.FullRowSelect = true;
            this.modsFiles_listView.GridLines = true;
            this.modsFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.modsFiles_listView.HideSelection = false;
            this.modsFiles_listView.Location = new System.Drawing.Point(9, 62);
            this.modsFiles_listView.Name = "modsFiles_listView";
            this.modsFiles_listView.Size = new System.Drawing.Size(507, 234);
            this.modsFiles_listView.TabIndex = 11;
            this.modsFiles_listView.UseCompatibleStateImageBehavior = false;
            this.modsFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Files (Path:Size)";
            this.columnHeader13.Width = 503;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "Steam Workshop Folder";
            // 
            // steamWorkshopFolderFindButton
            // 
            this.steamWorkshopFolderFindButton.Location = new System.Drawing.Point(420, 139);
            this.steamWorkshopFolderFindButton.Name = "steamWorkshopFolderFindButton";
            this.steamWorkshopFolderFindButton.Size = new System.Drawing.Size(26, 20);
            this.steamWorkshopFolderFindButton.TabIndex = 61;
            this.steamWorkshopFolderFindButton.Text = "...";
            this.steamWorkshopFolderFindButton.UseVisualStyleBackColor = true;
            this.steamWorkshopFolderFindButton.Click += new System.EventHandler(this.steamWorkshopFolderFindButton_Click);
            // 
            // steamWorkshopFolderTextBox
            // 
            this.steamWorkshopFolderTextBox.Location = new System.Drawing.Point(7, 139);
            this.steamWorkshopFolderTextBox.Name = "steamWorkshopFolderTextBox";
            this.steamWorkshopFolderTextBox.ReadOnly = true;
            this.steamWorkshopFolderTextBox.Size = new System.Drawing.Size(406, 20);
            this.steamWorkshopFolderTextBox.TabIndex = 60;
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
            this.Text = "Operations Launcher Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView presetMods_listView;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.TextBox serverCfg_textBox;
        private System.Windows.Forms.TextBox serverConfig_textBox;
        private System.Windows.Forms.TextBox pathToMods_textBox;
        private System.Windows.Forms.TextBox pathToArma3_textBox;
        private System.Windows.Forms.ListView customMods_listView;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.TextBox serverProfiles_textBox;
        private System.Windows.Forms.Button launch_button;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox launcherFiles_textBox;
        private System.Windows.Forms.Button verify_button;
        private System.Windows.Forms.TextBox modsFiles_textBox;
        private System.Windows.Forms.ListView excessFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ListView launcherFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ListView missingFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ListView modsFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.Button removeExcess_button;
        private System.Windows.Forms.Button changeServerProfiles_button;
        private System.Windows.Forms.Button changeServerCfg_button;
        private System.Windows.Forms.Button changeServerConfig_button;
        private System.Windows.Forms.Button changePathToArma3ServerMods_button;
        private System.Windows.Forms.Button changePathToArma3Server_button;
        private System.Windows.Forms.TextBox serverProfileName_textBox;
        private System.Windows.Forms.Button addCustomServerMod;
        private System.Windows.Forms.Button removeUncheckedServerMod_button;
        private System.Windows.Forms.CheckBox hideWindow_checkBox;
        private System.Windows.Forms.Button closeServer_button;
        private System.Windows.Forms.Button refresh_button;
        private System.Windows.Forms.Button fullVerify_button;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox defaultStartLineServer_textBox;
        private System.Windows.Forms.LinkLabel linkLabel8;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button saveSettings_button;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox xmlPath_textBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button steamWorkshopFolderFindButton;
        private System.Windows.Forms.TextBox steamWorkshopFolderTextBox;
    }
}
