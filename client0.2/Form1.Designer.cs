namespace lokunclient
{
    partial class lokunclientform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(lokunclientform));
            this.btnCheckConnection = new System.Windows.Forms.Button();
            this.lblCheckConnection = new System.Windows.Forms.Label();
            this.chkAutostart = new System.Windows.Forms.CheckBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.rdTunnelEverything = new System.Windows.Forms.RadioButton();
            this.rdExcludeIcelandic = new System.Windows.Forms.RadioButton();
            this.rdOnlyIcelandic = new System.Windows.Forms.RadioButton();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnkRegister = new System.Windows.Forms.LinkLabel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ststrpLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCheckConnection
            // 
            this.btnCheckConnection.Location = new System.Drawing.Point(13, 117);
            this.btnCheckConnection.Name = "btnCheckConnection";
            this.btnCheckConnection.Size = new System.Drawing.Size(109, 23);
            this.btnCheckConnection.TabIndex = 0;
            this.btnCheckConnection.Text = "Check connection";
            this.btnCheckConnection.UseVisualStyleBackColor = true;
            this.btnCheckConnection.Click += new System.EventHandler(this.btnCheckConnection_Click);
            // 
            // lblCheckConnection
            // 
            this.lblCheckConnection.AutoSize = true;
            this.lblCheckConnection.Location = new System.Drawing.Point(163, 122);
            this.lblCheckConnection.Name = "lblCheckConnection";
            this.lblCheckConnection.Size = new System.Drawing.Size(73, 13);
            this.lblCheckConnection.TabIndex = 1;
            this.lblCheckConnection.Text = "Disconnected";
            // 
            // chkAutostart
            // 
            this.chkAutostart.AutoSize = true;
            this.chkAutostart.Location = new System.Drawing.Point(14, 213);
            this.chkAutostart.Name = "chkAutostart";
            this.chkAutostart.Size = new System.Drawing.Size(68, 17);
            this.chkAutostart.TabIndex = 2;
            this.chkAutostart.Text = "Autostart";
            this.chkAutostart.UseVisualStyleBackColor = true;
            this.chkAutostart.CheckedChanged += new System.EventHandler(this.chkAutostart_CheckedChanged);
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(13, 163);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 4;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // rdTunnelEverything
            // 
            this.rdTunnelEverything.AutoSize = true;
            this.rdTunnelEverything.Location = new System.Drawing.Point(111, 146);
            this.rdTunnelEverything.Name = "rdTunnelEverything";
            this.rdTunnelEverything.Size = new System.Drawing.Size(116, 17);
            this.rdTunnelEverything.TabIndex = 5;
            this.rdTunnelEverything.TabStop = true;
            this.rdTunnelEverything.Text = "Tunnnel everything";
            this.rdTunnelEverything.UseVisualStyleBackColor = true;
            // 
            // rdExcludeIcelandic
            // 
            this.rdExcludeIcelandic.AutoSize = true;
            this.rdExcludeIcelandic.Location = new System.Drawing.Point(111, 169);
            this.rdExcludeIcelandic.Name = "rdExcludeIcelandic";
            this.rdExcludeIcelandic.Size = new System.Drawing.Size(155, 17);
            this.rdExcludeIcelandic.TabIndex = 6;
            this.rdExcludeIcelandic.TabStop = true;
            this.rdExcludeIcelandic.Text = "Exclude Icelandic networks";
            this.rdExcludeIcelandic.UseVisualStyleBackColor = true;
            // 
            // rdOnlyIcelandic
            // 
            this.rdOnlyIcelandic.AutoSize = true;
            this.rdOnlyIcelandic.Location = new System.Drawing.Point(111, 192);
            this.rdOnlyIcelandic.Name = "rdOnlyIcelandic";
            this.rdOnlyIcelandic.Size = new System.Drawing.Size(138, 17);
            this.rdOnlyIcelandic.TabIndex = 7;
            this.rdOnlyIcelandic.TabStop = true;
            this.rdOnlyIcelandic.Text = "Only Icelandic networks";
            this.rdOnlyIcelandic.UseVisualStyleBackColor = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(6, 19);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(138, 20);
            this.txtUsername.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lnkRegister);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.btnDownload);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Location = new System.Drawing.Point(13, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 95);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Download config";
            // 
            // lnkRegister
            // 
            this.lnkRegister.AutoSize = true;
            this.lnkRegister.Location = new System.Drawing.Point(6, 68);
            this.lnkRegister.Name = "lnkRegister";
            this.lnkRegister.Size = new System.Drawing.Size(111, 13);
            this.lnkRegister.TabIndex = 12;
            this.lnkRegister.TabStop = true;
            this.lnkRegister.Text = "Register new account";
            this.lnkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRegister_LinkClicked);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(7, 45);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(137, 20);
            this.txtPassword.TabIndex = 10;
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(150, 45);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(44, 23);
            this.btnDownload.TabIndex = 11;
            this.btnDownload.Text = "OK";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ststrpLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 240);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(284, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "stsStrip";
            // 
            // ststrpLabel
            // 
            this.ststrpLabel.Name = "ststrpLabel";
            this.ststrpLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // lokunclientform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.rdOnlyIcelandic);
            this.Controls.Add(this.rdExcludeIcelandic);
            this.Controls.Add(this.rdTunnelEverything);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.chkAutostart);
            this.Controls.Add(this.lblCheckConnection);
            this.Controls.Add(this.btnCheckConnection);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "lokunclientform";
            this.Text = "Lokun";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCheckConnection;
        private System.Windows.Forms.Label lblCheckConnection;
        private System.Windows.Forms.CheckBox chkAutostart;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.RadioButton rdTunnelEverything;
        private System.Windows.Forms.RadioButton rdExcludeIcelandic;
        private System.Windows.Forms.RadioButton rdOnlyIcelandic;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ststrpLabel;
        private System.Windows.Forms.LinkLabel lnkRegister;
    }
}

