namespace MurshunLauncherServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.createVerifyFile_button = new System.Windows.Forms.Button();
            this.changeRepoConfigPath_button = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pathToSyncFolder_textBox = new System.Windows.Forms.TextBox();
            this.xmlPath_textBox = new System.Windows.Forms.TextBox();
            this.saveSettings_button = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.repoConfigPath_textBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.refresh_button = new System.Windows.Forms.Button();
            this.presetMods_listView = new System.Windows.Forms.ListView();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pathToModsFolder_textBox = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.createVerifyFile_button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.syncFolders_button = new System.Windows.Forms.Button();
            this.compareServerMods_textBox = new System.Windows.Forms.TextBox();
            this.compareFolders_button = new System.Windows.Forms.Button();
            this.compareClientMods_textBox = new System.Windows.Forms.TextBox();
            this.compareExcessFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.compareServerFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.compareMissingFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.compareClientFiles_listView = new System.Windows.Forms.ListView();
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.tabControl1.Size = new System.Drawing.Size(1054, 741);
            this.tabControl1.TabIndex = 8;
            this.tabControl1.Tag = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.createVerifyFile_button);
            this.tabPage3.Controls.Add(this.changeRepoConfigPath_button);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.pathToSyncFolder_textBox);
            this.tabPage3.Controls.Add(this.xmlPath_textBox);
            this.tabPage3.Controls.Add(this.saveSettings_button);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.repoConfigPath_textBox);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.refresh_button);
            this.tabPage3.Controls.Add(this.presetMods_listView);
            this.tabPage3.Controls.Add(this.pathToModsFolder_textBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1046, 712);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Repo";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // createVerifyFile_button
            // 
            this.createVerifyFile_button.Location = new System.Drawing.Point(907, 627);
            this.createVerifyFile_button.Name = "createVerifyFile_button";
            this.createVerifyFile_button.Size = new System.Drawing.Size(131, 78);
            this.createVerifyFile_button.TabIndex = 62;
            this.createVerifyFile_button.Text = "Create Verify File";
            this.createVerifyFile_button.UseVisualStyleBackColor = true;
            this.createVerifyFile_button.Click += new System.EventHandler(this.createVerifyFile_button_Click);
            // 
            // changeRepoConfigPath_button
            // 
            this.changeRepoConfigPath_button.Location = new System.Drawing.Point(387, 61);
            this.changeRepoConfigPath_button.Name = "changeRepoConfigPath_button";
            this.changeRepoConfigPath_button.Size = new System.Drawing.Size(26, 20);
            this.changeRepoConfigPath_button.TabIndex = 61;
            this.changeRepoConfigPath_button.Text = "...";
            this.changeRepoConfigPath_button.UseVisualStyleBackColor = true;
            this.changeRepoConfigPath_button.Click += new System.EventHandler(this.changeRepoConfigPath_button_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 59;
            this.label6.Text = "Sync Folder";
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
            // pathToSyncFolder_textBox
            // 
            this.pathToSyncFolder_textBox.Location = new System.Drawing.Point(7, 139);
            this.pathToSyncFolder_textBox.Name = "pathToSyncFolder_textBox";
            this.pathToSyncFolder_textBox.ReadOnly = true;
            this.pathToSyncFolder_textBox.Size = new System.Drawing.Size(406, 20);
            this.pathToSyncFolder_textBox.TabIndex = 54;
            // 
            // xmlPath_textBox
            // 
            this.xmlPath_textBox.Location = new System.Drawing.Point(7, 22);
            this.xmlPath_textBox.Name = "xmlPath_textBox";
            this.xmlPath_textBox.ReadOnly = true;
            this.xmlPath_textBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.xmlPath_textBox.Size = new System.Drawing.Size(406, 20);
            this.xmlPath_textBox.TabIndex = 52;
            // 
            // saveSettings_button
            // 
            this.saveSettings_button.Location = new System.Drawing.Point(808, 682);
            this.saveSettings_button.Name = "saveSettings_button";
            this.saveSettings_button.Size = new System.Drawing.Size(93, 23);
            this.saveSettings_button.TabIndex = 51;
            this.saveSettings_button.Text = "Save Settings";
            this.saveSettings_button.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 45);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(66, 13);
            this.label15.TabIndex = 50;
            this.label15.Text = "Repo Config";
            // 
            // repoConfigPath_textBox
            // 
            this.repoConfigPath_textBox.Location = new System.Drawing.Point(7, 61);
            this.repoConfigPath_textBox.Name = "repoConfigPath_textBox";
            this.repoConfigPath_textBox.ReadOnly = true;
            this.repoConfigPath_textBox.Size = new System.Drawing.Size(374, 20);
            this.repoConfigPath_textBox.TabIndex = 49;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 38;
            this.label8.Text = "Mods Folder";
            // 
            // refresh_button
            // 
            this.refresh_button.Location = new System.Drawing.Point(956, 556);
            this.refresh_button.Name = "refresh_button";
            this.refresh_button.Size = new System.Drawing.Size(82, 23);
            this.refresh_button.TabIndex = 36;
            this.refresh_button.Text = "Refresh";
            this.refresh_button.UseVisualStyleBackColor = true;
            this.refresh_button.Click += new System.EventHandler(this.refreshServer_button_Click);
            // 
            // presetMods_listView
            // 
            this.presetMods_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8});
            this.presetMods_listView.FullRowSelect = true;
            this.presetMods_listView.GridLines = true;
            this.presetMods_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.presetMods_listView.Location = new System.Drawing.Point(419, 6);
            this.presetMods_listView.MultiSelect = false;
            this.presetMods_listView.Name = "presetMods_listView";
            this.presetMods_listView.Size = new System.Drawing.Size(619, 544);
            this.presetMods_listView.TabIndex = 4;
            this.presetMods_listView.UseCompatibleStateImageBehavior = false;
            this.presetMods_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Preset Mods";
            this.columnHeader8.Width = 582;
            // 
            // pathToModsFolder_textBox
            // 
            this.pathToModsFolder_textBox.Location = new System.Drawing.Point(7, 100);
            this.pathToModsFolder_textBox.Name = "pathToModsFolder_textBox";
            this.pathToModsFolder_textBox.ReadOnly = true;
            this.pathToModsFolder_textBox.Size = new System.Drawing.Size(406, 20);
            this.pathToModsFolder_textBox.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.createVerifyFile_button2);
            this.tabPage4.Controls.Add(this.progressBar1);
            this.tabPage4.Controls.Add(this.syncFolders_button);
            this.tabPage4.Controls.Add(this.compareServerMods_textBox);
            this.tabPage4.Controls.Add(this.compareFolders_button);
            this.tabPage4.Controls.Add(this.compareClientMods_textBox);
            this.tabPage4.Controls.Add(this.compareExcessFiles_listView);
            this.tabPage4.Controls.Add(this.compareServerFiles_listView);
            this.tabPage4.Controls.Add(this.compareMissingFiles_listView);
            this.tabPage4.Controls.Add(this.compareClientFiles_listView);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1046, 712);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Repo Tools";
            // 
            // createVerifyFile_button2
            // 
            this.createVerifyFile_button2.Location = new System.Drawing.Point(381, 6);
            this.createVerifyFile_button2.Name = "createVerifyFile_button2";
            this.createVerifyFile_button2.Size = new System.Drawing.Size(180, 23);
            this.createVerifyFile_button2.TabIndex = 20;
            this.createVerifyFile_button2.Text = "Create Verify File";
            this.createVerifyFile_button2.UseVisualStyleBackColor = true;
            this.createVerifyFile_button2.Click += new System.EventHandler(this.createVerifyFile_button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(567, 6);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(471, 23);
            this.progressBar1.TabIndex = 19;
            // 
            // syncFolders_button
            // 
            this.syncFolders_button.Location = new System.Drawing.Point(195, 6);
            this.syncFolders_button.Name = "syncFolders_button";
            this.syncFolders_button.Size = new System.Drawing.Size(180, 23);
            this.syncFolders_button.TabIndex = 17;
            this.syncFolders_button.Text = "Sync Folders";
            this.syncFolders_button.UseVisualStyleBackColor = true;
            this.syncFolders_button.Click += new System.EventHandler(this.button10_Click);
            // 
            // compareServerMods_textBox
            // 
            this.compareServerMods_textBox.Location = new System.Drawing.Point(534, 36);
            this.compareServerMods_textBox.Name = "compareServerMods_textBox";
            this.compareServerMods_textBox.ReadOnly = true;
            this.compareServerMods_textBox.Size = new System.Drawing.Size(507, 20);
            this.compareServerMods_textBox.TabIndex = 16;
            this.compareServerMods_textBox.Text = "Sync Folder";
            // 
            // compareFolders_button
            // 
            this.compareFolders_button.Location = new System.Drawing.Point(9, 6);
            this.compareFolders_button.Name = "compareFolders_button";
            this.compareFolders_button.Size = new System.Drawing.Size(180, 23);
            this.compareFolders_button.TabIndex = 12;
            this.compareFolders_button.Text = "Compare";
            this.compareFolders_button.UseVisualStyleBackColor = true;
            this.compareFolders_button.Click += new System.EventHandler(this.button9_Click);
            // 
            // compareClientMods_textBox
            // 
            this.compareClientMods_textBox.Location = new System.Drawing.Point(9, 36);
            this.compareClientMods_textBox.Name = "compareClientMods_textBox";
            this.compareClientMods_textBox.ReadOnly = true;
            this.compareClientMods_textBox.Size = new System.Drawing.Size(507, 20);
            this.compareClientMods_textBox.TabIndex = 9;
            this.compareClientMods_textBox.Text = "Mods Folder";
            // 
            // compareExcessFiles_listView
            // 
            this.compareExcessFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader10});
            this.compareExcessFiles_listView.FullRowSelect = true;
            this.compareExcessFiles_listView.GridLines = true;
            this.compareExcessFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.compareExcessFiles_listView.Location = new System.Drawing.Point(534, 302);
            this.compareExcessFiles_listView.Name = "compareExcessFiles_listView";
            this.compareExcessFiles_listView.Size = new System.Drawing.Size(507, 406);
            this.compareExcessFiles_listView.TabIndex = 14;
            this.compareExcessFiles_listView.UseCompatibleStateImageBehavior = false;
            this.compareExcessFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Excess Files (Path:Size)";
            this.columnHeader10.Width = 503;
            // 
            // compareServerFiles_listView
            // 
            this.compareServerFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11});
            this.compareServerFiles_listView.FullRowSelect = true;
            this.compareServerFiles_listView.GridLines = true;
            this.compareServerFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.compareServerFiles_listView.Location = new System.Drawing.Point(534, 62);
            this.compareServerFiles_listView.Name = "compareServerFiles_listView";
            this.compareServerFiles_listView.Size = new System.Drawing.Size(507, 234);
            this.compareServerFiles_listView.TabIndex = 15;
            this.compareServerFiles_listView.UseCompatibleStateImageBehavior = false;
            this.compareServerFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Files (Path:Size)";
            this.columnHeader11.Width = 503;
            // 
            // compareMissingFiles_listView
            // 
            this.compareMissingFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader12});
            this.compareMissingFiles_listView.FullRowSelect = true;
            this.compareMissingFiles_listView.GridLines = true;
            this.compareMissingFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.compareMissingFiles_listView.Location = new System.Drawing.Point(9, 302);
            this.compareMissingFiles_listView.Name = "compareMissingFiles_listView";
            this.compareMissingFiles_listView.Size = new System.Drawing.Size(507, 406);
            this.compareMissingFiles_listView.TabIndex = 13;
            this.compareMissingFiles_listView.UseCompatibleStateImageBehavior = false;
            this.compareMissingFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Missing Files (Path:Size)";
            this.columnHeader12.Width = 503;
            // 
            // compareClientFiles_listView
            // 
            this.compareClientFiles_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader13});
            this.compareClientFiles_listView.FullRowSelect = true;
            this.compareClientFiles_listView.GridLines = true;
            this.compareClientFiles_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.compareClientFiles_listView.Location = new System.Drawing.Point(9, 62);
            this.compareClientFiles_listView.Name = "compareClientFiles_listView";
            this.compareClientFiles_listView.Size = new System.Drawing.Size(507, 234);
            this.compareClientFiles_listView.TabIndex = 11;
            this.compareClientFiles_listView.UseCompatibleStateImageBehavior = false;
            this.compareClientFiles_listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Files (Path:Size)";
            this.columnHeader13.Width = 503;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 748);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operations Repo Tool";
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
        private System.Windows.Forms.TextBox pathToModsFolder_textBox;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox compareServerMods_textBox;
        private System.Windows.Forms.Button compareFolders_button;
        private System.Windows.Forms.TextBox compareClientMods_textBox;
        private System.Windows.Forms.ListView compareExcessFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ListView compareServerFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ListView compareMissingFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ListView compareClientFiles_listView;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.Button syncFolders_button;
        private System.Windows.Forms.Button refresh_button;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox repoConfigPath_textBox;
        private System.Windows.Forms.Button saveSettings_button;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pathToSyncFolder_textBox;
        private System.Windows.Forms.TextBox xmlPath_textBox;
        private System.Windows.Forms.Button changeRepoConfigPath_button;
        private System.Windows.Forms.Button createVerifyFile_button;
        private System.Windows.Forms.Button createVerifyFile_button2;
    }
}

