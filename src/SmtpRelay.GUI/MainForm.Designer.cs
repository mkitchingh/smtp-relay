namespace SmtpRelay.GUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label       labelHost;
        private System.Windows.Forms.TextBox     txtHost;
        private System.Windows.Forms.Label       labelPort;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.CheckBox    chkStartTls;
        private System.Windows.Forms.Label       lblUsername;
        private System.Windows.Forms.TextBox     txtUsername;
        private System.Windows.Forms.Label       lblPassword;
        private System.Windows.Forms.TextBox     txtPassword;
        private System.Windows.Forms.Label       labelRelayRestrictions;
        private System.Windows.Forms.RadioButton radioAllowAll;
        private System.Windows.Forms.RadioButton radioAllowList;
        private System.Windows.Forms.TextBox     txtIpList;
        private System.Windows.Forms.Label       labelIpExample;
        private System.Windows.Forms.Label       lblLogging;
        private System.Windows.Forms.CheckBox    chkEnableLogging;
        private System.Windows.Forms.Label       labelDaysKept;
        private System.Windows.Forms.NumericUpDown numRetentionDays;
        private System.Windows.Forms.Button      btnViewLogs;
        private System.Windows.Forms.Button      btnSave;
        private System.Windows.Forms.Button      btnClose;
        private System.Windows.Forms.Label       labelServiceStatusCaption;
        private System.Windows.Forms.Label       labelServiceStatus;
        private System.Windows.Forms.Label       lblVersion;
        private System.Windows.Forms.LinkLabel   linkRepo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components                    = new System.ComponentModel.Container();
            this.labelHost                     = new System.Windows.Forms.Label();
            this.txtHost                       = new System.Windows.Forms.TextBox();
            this.labelPort                     = new System.Windows.Forms.Label();
            this.numPort                       = new System.Windows.Forms.NumericUpDown();
            this.chkStartTls                   = new System.Windows.Forms.CheckBox();
            this.lblUsername                   = new System.Windows.Forms.Label();
            this.txtUsername                   = new System.Windows.Forms.TextBox();
            this.lblPassword                   = new System.Windows.Forms.Label();
            this.txtPassword                   = new System.Windows.Forms.TextBox();
            this.labelRelayRestrictions        = new System.Windows.Forms.Label();
            this.radioAllowAll                 = new System.Windows.Forms.RadioButton();
            this.radioAllowList                = new System.Windows.Forms.RadioButton();
            this.txtIpList                     = new System.Windows.Forms.TextBox();
            this.labelIpExample                = new System.Windows.Forms.Label();
            this.lblLogging                    = new System.Windows.Forms.Label();
            this.chkEnableLogging              = new System.Windows.Forms.CheckBox();
            this.labelDaysKept                 = new System.Windows.Forms.Label();
            this.numRetentionDays              = new System.Windows.Forms.NumericUpDown();
            this.btnViewLogs                   = new System.Windows.Forms.Button();
            this.btnSave                       = new System.Windows.Forms.Button();
            this.btnClose                      = new System.Windows.Forms.Button();
            this.labelServiceStatusCaption     = new System.Windows.Forms.Label();
            this.labelServiceStatus            = new System.Windows.Forms.Label();
            this.lblVersion                    = new System.Windows.Forms.Label();
            this.linkRepo                      = new System.Windows.Forms.LinkLabel();

            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetentionDays)).BeginInit();
            this.SuspendLayout();

            // MainForm
            this.ClientSize               = new System.Drawing.Size(480, 520);
            this.FormBorderStyle          = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox              = false;
            this.StartPosition            = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text                     = "SMTP Relay Configuration";
            this.Font                     = new System.Drawing.Font("Segoe UI",  nineF:9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            // labelHost
            this.labelHost.AutoSize       = true;
            this.labelHost.Location       = new System.Drawing.Point(20, 20);
            this.labelHost.Name           = "labelHost";
            this.labelHost.Size           = new System.Drawing.Size(77, 15);
            this.labelHost.Text           = "SMTP Host:";

            // txtHost
            this.txtHost.Location         = new System.Drawing.Point(120, 17);
            this.txtHost.Size             = new System.Drawing.Size(330, 23);
            this.txtHost.Name             = "txtHost";

            // labelPort
            this.labelPort.AutoSize       = true;
            this.labelPort.Location       = new System.Drawing.Point(20, 60);
            this.labelPort.Name           = "labelPort";
            this.labelPort.Size           = new System.Drawing.Size(34, 15);
            this.labelPort.Text           = "Port:";

            // numPort
            this.numPort.Location         = new System.Drawing.Point(120, 57);
            this.numPort.Maximum          = new decimal(new int[] {65535,0,0,0});
            this.numPort.Minimum          = new decimal(new int[] {1,0,0,0});
            this.numPort.Value            = new decimal(new int[] {25,0,0,0});
            this.numPort.Name             = "numPort";
            this.numPort.Size             = new System.Drawing.Size(60, 23);

            // chkStartTls
            this.chkStartTls.AutoSize     = true;
            this.chkStartTls.Location     = new System.Drawing.Point(200, 59);
            this.chkStartTls.Name         = "chkStartTls";
            this.chkStartTls.Size         = new System.Drawing.Size(70, 19);
            this.chkStartTls.Text         = "STARTTLS";
            this.chkStartTls.CheckedChanged += this.chkStartTls_CheckedChanged;

            // lblUsername
            this.lblUsername.AutoSize     = true;
            this.lblUsername.Location     = new System.Drawing.Point(20, 100);
            this.lblUsername.Name         = "lblUsername";
            this.lblUsername.Size         = new System.Drawing.Size(60, 15);
            this.lblUsername.Text         = "Username:";
            
            // txtUsername
            this.txtUsername.Location     = new System.Drawing.Point(120, 97);
            this.txtUsername.Size         = new System.Drawing.Size(330, 23);
            this.txtUsername.Name         = "txtUsername";
            this.txtUsername.Enabled      = false;

            // lblPassword
            this.lblPassword.AutoSize     = true;
            this.lblPassword.Location     = new System.Drawing.Point(20, 140);
            this.lblPassword.Name         = "lblPassword";
            this.lblPassword.Size         = new System.Drawing.Size(59, 15);
            this.lblPassword.Text         = "Password:";
            
            // txtPassword
            this.txtPassword.Location     = new System.Drawing.Point(120, 137);
            this.txtPassword.Size         = new System.Drawing.Size(330, 23);
            this.txtPassword.Name         = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Enabled      = false;

            // labelRelayRestrictions
            this.labelRelayRestrictions.AutoSize = true;
            this.labelRelayRestrictions.Location = new System.Drawing.Point(20, 180);
            this.labelRelayRestrictions.Name     = "labelRelayRestrictions";
            this.labelRelayRestrictions.Size     = new System.Drawing.Size(119, 15);
            this.labelRelayRestrictions.Text     = "Relay Restrictions:";

            // radioAllowAll
            this.radioAllowAll.AutoSize    = true;
            this.radioAllowAll.Location    = new System.Drawing.Point(160, 178);
            this.radioAllowAll.Name        = "radioAllowAll";
            this.radioAllowAll.Size        = new System.Drawing.Size(69, 19);
            this.radioAllowAll.Text        = "Allow All";
            this.radioAllowAll.Checked     = true;
            this.radioAllowAll.CheckedChanged += this.radioAllowRestrictions_CheckedChanged;

            // radioAllowList
            this.radioAllowList.AutoSize   = true;
            this.radioAllowList.Location   = new System.Drawing.Point(250, 178);
            this.radioAllowList.Name       = "radioAllowList";
            this.radioAllowList.Size       = new System.Drawing.Size(118, 19);
            this.radioAllowList.Text       = "Allow Specified";
            this.radioAllowList.CheckedChanged += this.radioAllowRestrictions_CheckedChanged;

            // txtIpList
            this.txtIpList.Location        = new System.Drawing.Point(120, 205);
            this.txtIpList.Multiline       = true;
            this.txtIpList.Name            = "txtIpList";
            this.txtIpList.Size            = new System.Drawing.Size(330, 60);
            this.txtIpList.Enabled         = false;
            this.txtIpList.ScrollBars      = System.Windows.Forms.ScrollBars.Vertical;

            // labelIpExample
            this.labelIpExample.AutoSize   = true;
            this.labelIpExample.ForeColor  = System.Drawing.Color.Gray;
            this.labelIpExample.Location   = new System.Drawing.Point(120, 270);
            this.labelIpExample.Name       = "labelIpExample";
            this.labelIpExample.Size       = new System.Drawing.Size(208, 15);
            this.labelIpExample.Text       = "e.g. 127.0.0.1, 10.0.0.0/24, ::1";

            // lblLogging
            this.lblLogging.AutoSize       = true;
            this.lblLogging.Location       = new System.Drawing.Point(20, 310);
            this.lblLogging.Name           = "lblLogging";
            this.lblLogging.Size           = new System.Drawing.Size(52, 15);
            this.lblLogging.Text           = "Logging:";

            // chkEnableLogging
            this.chkEnableLogging.AutoSize = true;
            this.chkEnableLogging.Location = new System.Drawing.Point(120, 308);
            this.chkEnableLogging.Name     = "chkEnableLogging";
            this.chkEnableLogging.Size     = new System.Drawing.Size(66, 19);
            this.chkEnableLogging.Text     = "Enable";
            this.chkEnableLogging.CheckedChanged += this.chkEnableLogging_CheckedChanged;

            // labelDaysKept
            this.labelDaysKept.AutoSize    = true;
            this.labelDaysKept.Location    = new System.Drawing.Point(220, 310);
            this.labelDaysKept.Name        = "labelDaysKept";
            this.labelDaysKept.Size        = new System.Drawing.Size(68, 15);
            this.labelDaysKept.Text        = "Days Kept:";

            // numRetentionDays
            this.numRetentionDays.Location  = new System.Drawing.Point(300, 307);
            this.numRetentionDays.Maximum   = new decimal(new int[]{ 365,0,0,0 });
            this.numRetentionDays.Minimum   = new decimal(new int[]{ 1,0,0,0 });
            this.numRetentionDays.Value     = new decimal(new int[]{ 30,0,0,0 });
            this.numRetentionDays.Name      = "numRetentionDays";
            this.numRetentionDays.Size      = new System.Drawing.Size(60, 23);

            // btnViewLogs
            this.btnViewLogs.Location      = new System.Drawing.Point(380, 304);
            this.btnViewLogs.Name          = "btnViewLogs";
            this.btnViewLogs.Size          = new System.Drawing.Size(70, 27);
            this.btnViewLogs.Text          = "View Logs";
            this.btnViewLogs.Click         += this.btnViewLogs_Click;

            // btnSave
            this.btnSave.Location          = new System.Drawing.Point(120, 350);
            this.btnSave.Name              = "btnSave";
            this.btnSave.Size              = new System.Drawing.Size(120, 30);
            this.btnSave.Text              = "Save and Restart";
            this.btnSave.Click             += this.btnSave_Click;

            // btnClose
            this.btnClose.Location         = new System.Drawing.Point(260, 350);
            this.btnClose.Name             = "btnClose";
            this.btnClose.Size             = new System.Drawing.Size(120, 30);
            this.btnClose.Text             = "Close";
            this.btnClose.Click            += this.btnClose_Click;

            // labelServiceStatusCaption
            this.labelServiceStatusCaption.AutoSize = true;
            this.labelServiceStatusCaption.Font      = new System.Drawing.Font("Segoe UI",  nineF:9F, System.Drawing.FontStyle.Bold);
            this.labelServiceStatusCaption.Location  = new System.Drawing.Point(20, 400);
            this.labelServiceStatusCaption.Name      = "labelServiceStatusCaption";
            this.labelServiceStatusCaption.Size      = new System.Drawing.Size(92, 15);
            this.labelServiceStatusCaption.Text      = "Service Status:";

            // labelServiceStatus
            this.labelServiceStatus.AutoSize   = true;
            this.labelServiceStatus.Font      = new System.Drawing.Font("Segoe UI",  nineF:9F, System.Drawing.FontStyle.Bold);
            this.labelServiceStatus.Location  = new System.Drawing.Point(120, 400);
            this.labelServiceStatus.Name      = "labelServiceStatus";
            this.labelServiceStatus.Size      = new System.Drawing.Size(56, 15);
            this.labelServiceStatus.Text      = "Unknown";

            // lblVersion
            this.lblVersion.AutoSize         = true;
            this.lblVersion.Location         = new System.Drawing.Point(20, 430);
            this.lblVersion.Name             = "lblVersion";
            this.lblVersion.Size             = new System.Drawing.Size(48, 15);
            this.lblVersion.Text             = "Version:";

            // linkRepo
            this.linkRepo.AutoSize           = true;
            this.linkRepo.Location           = new System.Drawing.Point(120, 430);
            this.linkRepo.Name               = "linkRepo";
            this.linkRepo.Size               = new System.Drawing.Size(260, 15);
            this.linkRepo.Text               = "https://github.com/mkitchingh/Smtp-Relay";
            this.linkRepo.LinkClicked       += this.linkRepo_LinkClicked;

            // add controls
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.labelHost, this.txtHost,
                this.labelPort, this.numPort, this.chkStartTls,
                this.lblUsername, this.txtUsername,
                this.lblPassword, this.txtPassword,
                this.labelRelayRestrictions, this.radioAllowAll, this.radioAllowList,
                this.txtIpList, this.labelIpExample,
                this.lblLogging, this.chkEnableLogging, this.labelDaysKept, this.numRetentionDays, this.btnViewLogs,
                this.btnSave, this.btnClose,
                this.labelServiceStatusCaption, this.labelServiceStatus,
                this.lblVersion, this.linkRepo
            });

            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetentionDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
