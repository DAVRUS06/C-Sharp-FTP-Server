namespace FTPserver
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.sharedFoldertxtbox = new System.Windows.Forms.TextBox();
            this.openSharedFolderbtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.porttxtbox = new System.Windows.Forms.TextBox();
            this.userNametxtbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.passwordtxtbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.startServerbtn = new System.Windows.Forms.Button();
            this.openFolderToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.portToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userNameToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.passwordToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.startServerToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.serverStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.stopListeningbtn = new System.Windows.Forms.Button();
            this.stopServertooltip = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shared Folder:";
            this.openFolderToolTip.SetToolTip(this.label1, "Path of the shared folder on the local machine.");
            // 
            // sharedFoldertxtbox
            // 
            this.sharedFoldertxtbox.Location = new System.Drawing.Point(138, 9);
            this.sharedFoldertxtbox.Name = "sharedFoldertxtbox";
            this.sharedFoldertxtbox.Size = new System.Drawing.Size(198, 20);
            this.sharedFoldertxtbox.TabIndex = 1;
            this.sharedFoldertxtbox.Text = "\\";
            // 
            // openSharedFolderbtn
            // 
            this.openSharedFolderbtn.Location = new System.Drawing.Point(342, 9);
            this.openSharedFolderbtn.Name = "openSharedFolderbtn";
            this.openSharedFolderbtn.Size = new System.Drawing.Size(75, 23);
            this.openSharedFolderbtn.TabIndex = 2;
            this.openSharedFolderbtn.Text = "Open Folder";
            this.openFolderToolTip.SetToolTip(this.openSharedFolderbtn, "Select a folder to be used as the root of the server.");
            this.openSharedFolderbtn.UseVisualStyleBackColor = true;
            this.openSharedFolderbtn.Click += new System.EventHandler(this.openSharedFolderbtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(86, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port:";
            this.portToolTip.SetToolTip(this.label2, "Enter a port for the server to listen on. Default is 21.");
            // 
            // porttxtbox
            // 
            this.porttxtbox.Location = new System.Drawing.Point(138, 35);
            this.porttxtbox.Name = "porttxtbox";
            this.porttxtbox.Size = new System.Drawing.Size(198, 20);
            this.porttxtbox.TabIndex = 4;
            this.porttxtbox.Text = "21";
            // 
            // userNametxtbox
            // 
            this.userNametxtbox.Location = new System.Drawing.Point(138, 61);
            this.userNametxtbox.Name = "userNametxtbox";
            this.userNametxtbox.Size = new System.Drawing.Size(198, 20);
            this.userNametxtbox.TabIndex = 6;
            this.userNametxtbox.Text = "admin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(44, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "Username:";
            this.userNameToolTip.SetToolTip(this.label3, "Enter a username. This username will be used when connecting to the server. (Defa" +
        "ult: admin)");
            // 
            // passwordtxtbox
            // 
            this.passwordtxtbox.Location = new System.Drawing.Point(138, 87);
            this.passwordtxtbox.Name = "passwordtxtbox";
            this.passwordtxtbox.Size = new System.Drawing.Size(198, 20);
            this.passwordtxtbox.TabIndex = 7;
            this.passwordtxtbox.Text = "admin";
            this.passwordtxtbox.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(44, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 21);
            this.label4.TabIndex = 8;
            this.label4.Text = "Password:";
            this.passwordToolTip.SetToolTip(this.label4, "Enter a password. This password will be used when connecting to the server. (Defa" +
        "ult: admin)");
            // 
            // startServerbtn
            // 
            this.startServerbtn.Enabled = false;
            this.startServerbtn.Location = new System.Drawing.Point(12, 112);
            this.startServerbtn.Name = "startServerbtn";
            this.startServerbtn.Size = new System.Drawing.Size(200, 23);
            this.startServerbtn.TabIndex = 9;
            this.startServerbtn.Text = "Start Server";
            this.startServerToolTip.SetToolTip(this.startServerbtn, "Start running the server.");
            this.startServerbtn.UseVisualStyleBackColor = true;
            this.startServerbtn.Click += new System.EventHandler(this.startServer_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 139);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(434, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // serverStatus
            // 
            this.serverStatus.BackColor = System.Drawing.Color.Red;
            this.serverStatus.Name = "serverStatus";
            this.serverStatus.Size = new System.Drawing.Size(148, 17);
            this.serverStatus.Text = "Server Status: Not Running";
            // 
            // stopListeningbtn
            // 
            this.stopListeningbtn.Enabled = false;
            this.stopListeningbtn.Location = new System.Drawing.Point(222, 112);
            this.stopListeningbtn.Name = "stopListeningbtn";
            this.stopListeningbtn.Size = new System.Drawing.Size(200, 23);
            this.stopListeningbtn.TabIndex = 12;
            this.stopListeningbtn.Text = "Stop Listening";
            this.stopServertooltip.SetToolTip(this.stopListeningbtn, "Stops the server from listening for connections.");
            this.stopListeningbtn.UseVisualStyleBackColor = true;
            this.stopListeningbtn.Click += new System.EventHandler(this.stopListeningbtn_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(434, 161);
            this.Controls.Add(this.stopListeningbtn);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.startServerbtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.passwordtxtbox);
            this.Controls.Add(this.userNametxtbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.porttxtbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.openSharedFolderbtn);
            this.Controls.Add(this.sharedFoldertxtbox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(450, 200);
            this.MinimumSize = new System.Drawing.Size(450, 200);
            this.Name = "mainForm";
            this.Text = "FTP Server";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sharedFoldertxtbox;
        private System.Windows.Forms.Button openSharedFolderbtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox porttxtbox;
        private System.Windows.Forms.TextBox userNametxtbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox passwordtxtbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button startServerbtn;
        private System.Windows.Forms.ToolTip openFolderToolTip;
        private System.Windows.Forms.ToolTip portToolTip;
        private System.Windows.Forms.ToolTip userNameToolTip;
        private System.Windows.Forms.ToolTip passwordToolTip;
        private System.Windows.Forms.ToolTip startServerToolTip;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel serverStatus;
        private System.Windows.Forms.Button stopListeningbtn;
        private System.Windows.Forms.ToolTip stopServertooltip;
    }
}

