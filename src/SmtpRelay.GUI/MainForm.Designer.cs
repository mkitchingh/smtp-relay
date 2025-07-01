namespace SmtpRelay.GUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblSmtpHost;
        private System.Windows.Forms.TextBox txtSmtpHost;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.CheckBox chkStartTls;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblRestrictions;
        private System.Windows.Forms.RadioButton radioAllowAll;
        private System.Windows.Forms.RadioButton radioAllowList;
        private System.Windows.Forms.Label lblIpList;
        private System.Windows.Forms.TextBox txtIpList;
        private System.Windows.Forms.Label lblIpSample;
        private System.Windows.Forms.Label lblLogging;
        private System.Windows.Forms.CheckBox chkEnableLogging;
        private System.Windows.Forms.Label lblDaysKept;
        private System.Windows.Forms.NumericUpDown numDaysKept;
        private System.Windows.Forms.Button btnViewLogs;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.LinkLabel linkRepo;
        private System.Windows.Forms.Label labelServiceStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblSmtpHost = new System.Windows.Forms.Label();
            this.txtSmtpHost = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.chkStartTls = new System.Windows.Forms.CheckBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblRestrictions = new System.Windows.Forms.Label();
            this.radioAllowAll = new System.Windows.Forms.RadioButton();
            this.radioAllowList = new System.Windows.Forms.RadioButton();
            this.lblIpList = new System.Windows.Forms.Label();
            this.txtIpList = new System.Windows.Forms.TextBox();
            this.lblIpSample = new System.Windows.Forms.Label();
            this.lblLogging = new System.Windows.Forms.Label();
            this.chkEnableLogging = new System.Windows.Forms.CheckBox();
            this.lblDaysKept = new System.Windows.Forms.Label();
            this.numDaysKept = new System.Windows.Forms.NumericUpDown();
            this.btnViewLogs = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.linkRepo = new System.Windows.Forms.LinkLabel();
            this.labelServiceStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDaysKept)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSmtpHost
            // 
            this.lblSmtpHost.AutoSize = true;
            this.lblSmtpHost.Location = new System.Drawing.Point(20, 20);
            this.lblSmtpHost.Name = "lblSmtpHost";
            this.lblSmtpHost.Size = new System.Drawing.Size(65, 13);
            this.lblSmtpHost.TabIndex = 0;
            this.lblSmtpHost.Text = "SMTP Host:";
            // 
            // txtSmtpHost
            // 
            this.txtSmtpHost.Location = new System.Drawing.Point(150, 17);
            this.txtSmtpHost.Name = "txtSmtpHost";
            this.txtSmtpHost.Size = new System.Drawing.Size(400, 20);
            this.txtSmtpHost.TabIndex = 1;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(20, 60);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port:";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(150, 58);
            this.numPort.Maximum = new decimal(new int[] {65535,0,0,0});
            this.numPort.Minimum = new decimal(new int[] {1,0,0,0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(80, 20);
            this.numPort.TabIndex = 3;
            this.numPort.Value = new decimal(new int[] {25,0,0,0});
            // 
            // chkStartTls
            // 
            this.chkStartTls.AutoSize = true;
            this.chkStartTls.Location = new System.Drawing.Point(260, 59);
            this.chkStartTls.Name = "chkStartTls";
            this.chkStartTls.Size = new System.Drawing.Size(70, 17);
            this.chkStartTls.TabIndex = 4;
            this.chkStartTls.Text = "STARTTLS";
            this.chkStartTls.UseVisualStyleBackColor = true;
            this.chkStartTls.CheckedChanged += new System.EventHandler(this.chkStartTls_CheckedChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(20, 100);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(58, 13);
            this.lblUsername.TabIndex = 5;
            this.lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(150, 97);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(400, 20);
            this.txtUsername.TabIndex = 6;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(20, 140);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(150, 137);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(400, 20);
            this.txtPassword.TabIndex = 8;
            // 
            // lblRestrictions
            // 
            this.lblRestrictions.AutoSize = true;
            this.lblRestrictions.Location = new System.Drawing.Point(20, 180);
            this.lblRestrictions.Name = "lblRestrictions";
            this.lblRestrictions.Size = new System.Drawing.Size(100, 13);
            this.lblRestrictions.TabIndex = 9;
            this.lblRestrictions.Text = "Relay Restrictions:";
            // 
            // radioAllowAll
            // 
            this.radioAllowAll.AutoSize = true;
            this.radioAllowAll.Location = new System.Drawing.Point(150, 178);
            this.radioAllowAll.Name = "radioAllowAll";
            this.radioAllowAll.Size = new System.Drawing.Size(67, 17);
            this.radioAllowAll.TabIndex = 10;
            this.radioAllowAll.TabStop = true;
            this.radioAllowAll.Text = "Allow All";
            this.radioAllowAll.UseVisualStyleBackColor = true;
            this.radioAllowAll.CheckedChanged += new System.EventHandler(this.radioAllowRestrictions_CheckedChanged);
            // 
            // radioAllowList
            // 
            this.radioAllowList.AutoSize = true;
            this.radioAllowList.Location = new System.Drawing.Point(260, 178);
            this.radioAllowList.Name = "radioAllowList";
            this.radioAllowList.Size = new System.Drawing.Size(106, 17);
            this.radioAllowList.TabIndex = 11;
            this.radioAllowList.TabStop = true;
            this.radioAllowList.Text = "Allow Specified";
            this.radioAllowList.UseVisualStyleBackColor = true;
            this.radioAllowList.CheckedChanged += new System.EventHandler(this.radioAllowRestrictions_CheckedChanged);
            // 
            // lblIpList
            // 
            this.lblIpList.AutoSize = true;
            this.lblIpList.Location = new System.Drawing.Point(20, 220);
            this.lblIpList.Name = "lblIpList";
            this.lblIpList.Size = new System.Drawing.Size(80, 13);
            this.lblIpList.TabIndex = 12;
            this.lblIpList.Text = "IP Allowed List:";
            // 
            // txtIpList
            // 
            this.txtIpList.Location = new System.Drawing.Point(150, 217);
            this.txtIpList.Multiline = true;
            this.txtIpList.Name = "txtIpList";
            this.txtIpList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIpList.Size = new System.Drawing.Size(400, 80);
            this.txtIpList.TabIndex = 13;
            // 
            // lblIpSample
            // 
            this.lblIpSample.AutoSize = true;
            this.lblIpSample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblIpSample.Location = new System.Drawing.Point(150, 305);
            this.lblIpSample.Name = "lblIpSample";
            this.lblIpSample.Size = new System.Drawing.Size(280, 13);
            this.lblIpSample.TabIndex = 14;
            this.lblIpSample.Text = "e.g. 192.168.1.0/24, 10.0.0.1, 2001:db8::/32 (comma-separated)";
            // 
            // lblLogging
            // 
            this.lblLogging.AutoSize = true;
            this.lblLogging.Location = new System.Drawing.Point(20, 340);
            this.lblLogging.Name = "lblLogging";
            this.lblLogging.Size = new System.Drawing.Size(47, 13);
            this.lblLogging.TabIndex = 15;
            this.lblLogging.Text = "Logging:";
            // 
            // chkEnableLogging
            // 
            this.chkEnableLogging.AutoSize = true;
            this.chkEnableLogging.Location = new System.Drawing.Point(150, 338);
            this.chkEnableLogging.Name = "chkEnableLogging";
            this.chkEnableLogging.Size = new System.Drawing.Size(102, 17);
            this.chkEnableLogging.TabIndex = 16;
            this.chkEnableLogging.Text = "Enable Logging";
            this.chkEnableLogging.UseVisualStyleBackColor = true;
            // 
            // lblDaysKept
            // 
            this.lblDaysKept.AutoSize = true;
            this.lblDaysKept.Location = new System.Drawing.Point(280, 340);
            this.lblDaysKept.Name = "lblDaysKept";
            this.lblDaysKept.Size = new System.Drawing.Size(61, 13);
            this.lblDaysKept.TabIndex = 17;
            this.lblDaysKept.Text = "Days Kept:";
            // 
            // numDaysKept
            // 
            this.numDaysKept.Location = new System.Drawing.Point(360, 338);
            this.numDaysKept.Minimum = new decimal(new int[] {1,0,0,0});
            this.numDaysKept.Name = "numDaysKept";
            this.numDaysKept.Size = new System.Drawing.Size(60, 20);
            this.numDaysKept.TabIndex = 18;
            this.numDaysKept.Value = new decimal(new int[] {30,0,0,0});
            // 
            // btnViewLogs
            // 
            this.btnViewLogs.Location = new System.Drawing.Point(450, 335);
            this.btnViewLogs.Name = "btnViewLogs";
            this.btnViewLogs.Size = new System.Drawing.Size(100, 25);
            this.btnViewLogs.TabIndex = 19;
            this.btnViewLogs.Text = "View Logs";
            this.btnViewLogs.UseVisualStyleBackColor = true;
            this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(150, 380);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 40);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save && Restart";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(300, 380);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 40);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblVersion.Location = new System.Drawing.Point(20, 750);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(54, 13);
            this.lblVersion.TabIndex = 22;
            this.lblVersion.Text = "Version: ?";
            // 
            // linkRepo
            // 
            this.linkRepo.AutoSize = true;
            this.linkRepo.Location = new System.Drawing.Point(150, 750);
            this.linkRepo.Name = "linkRepo";
            this.linkRepo.Size = new System.Drawing.Size(200, 13);
            this.linkRepo.TabIndex = 23;
            this.linkRepo.TabStop = true;
            this.linkRepo.Text = "https://github.com/mkitchingh/Smtp-Relay";
            this.linkRepo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRepo_LinkClicked);
            // 
            // labelServiceStatus
            // 
            this.labelServiceStatus.AutoSize = true;
            this.labelServiceStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelServiceStatus.Location = new System.Drawing.Point(20, 720);
            this.labelServiceStatus.Name = "labelServiceStatus";
            this.labelServiceStatus.Size = new System.Drawing.Size(119, 19);
            this.labelServiceStatus.TabIndex = 24;
            this.labelServiceStatus.Text = "Service Status...";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 800);
            this.Controls.Add(this.labelServiceStatus);
            this.Controls.Add(this.linkRepo);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnViewLogs);
            this.Controls.Add(this.numDaysKept);
            this.Controls.Add(this.lblDaysKept);
            this.Controls.Add(this.chkEnableLogging);
            this.Controls.Add(this.lblLogging);
            this.Controls.Add(this.lblIpSample);
            this.Controls.Add(this.txtIpList);
            this.Controls.Add(this.lblIpList);
            this.Controls.Add(this.radioAllowList);
            this.Controls.Add(this.radioAllowAll);
            this.Controls.Add(this.lblRestrictions);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.chkStartTls);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtSmtpHost);
            this.Controls.Add(this.lblSmtpHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "SMTP Relay Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDaysKept)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
