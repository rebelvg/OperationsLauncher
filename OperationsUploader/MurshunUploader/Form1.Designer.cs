namespace MurshunUploader
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
            this.button1 = new System.Windows.Forms.Button();
            this.password_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.directUpload_checkBox = new System.Windows.Forms.CheckBox();
            this.clearDependencies_checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(367, 293);
            this.button1.TabIndex = 1;
            this.button1.Text = "Select mission";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // password_textBox
            // 
            this.password_textBox.Location = new System.Drawing.Point(279, 311);
            this.password_textBox.Name = "password_textBox";
            this.password_textBox.Size = new System.Drawing.Size(100, 20);
            this.password_textBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(208, 314);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Password ->";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 363);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Version 0.9.9";
            // 
            // directUpload_checkBox
            // 
            this.directUpload_checkBox.AutoSize = true;
            this.directUpload_checkBox.Location = new System.Drawing.Point(15, 313);
            this.directUpload_checkBox.Name = "directUpload_checkBox";
            this.directUpload_checkBox.Size = new System.Drawing.Size(140, 17);
            this.directUpload_checkBox.TabIndex = 6;
            this.directUpload_checkBox.Text = "Direct Upload (Fallback)";
            this.directUpload_checkBox.UseVisualStyleBackColor = true;
            // 
            // clearDependencies_checkBox
            // 
            this.clearDependencies_checkBox.AutoSize = true;
            this.clearDependencies_checkBox.Location = new System.Drawing.Point(15, 336);
            this.clearDependencies_checkBox.Name = "clearDependencies_checkBox";
            this.clearDependencies_checkBox.Size = new System.Drawing.Size(122, 17);
            this.clearDependencies_checkBox.TabIndex = 5;
            this.clearDependencies_checkBox.Text = "Clear Dependencies";
            this.clearDependencies_checkBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 385);
            this.Controls.Add(this.directUpload_checkBox);
            this.Controls.Add(this.clearDependencies_checkBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.password_textBox);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operations Uploader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox password_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox directUpload_checkBox;
        private System.Windows.Forms.CheckBox clearDependencies_checkBox;
    }
}

