// File: src/SmtpRelay.GUI/MainForm.Designer.cs
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
        private System.Windows.Forms.Label       labelUsername;
        private System.Windows.Forms.TextBox     txtUsername;
        private System.Windows.Forms.Label       labelPassword;
        private System.Windows.Forms.TextBox     txtPassword;
        private System.Windows.Forms.Label       labelRelayRestrictions;
        private System.Windows.Forms.RadioButton radioAllowAll;
        private System.Windows.Forms.RadioButton radioAllowList;
        private System.Windows.Forms.TextBox     txtIpList;
        private System.Windows.Forms.Label       labelIpExample;
        private System.Windows.Forms.Label       labelLogging;
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

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components                = new System.ComponentModel.Container();
            this.labelHost                 = new System.Windows.Forms.Label();
            this.txtHost                   = new System.Windows.Forms.TextBox();
            this.labelPort                 = new System.Windows.Forms.Label();
            this.numPort                   = new System.Windows.Forms.NumericUpDown();
            this.chkStartTls               = new System.Windows.Forms.CheckBox();
            this.labelUsername             = new System.Windows.Forms.Label();
            this.txtUsername               = new System.Windows.Forms.TextBox();
            this.labelPassword             = new System.Windows.Forms.Label();
            this.txtPassword               = new System.Windows.Forms.TextBox();
            this.labelRelayRestrictions    = new System.Windows.Forms.Label();
            this.radioAllowAll             = new System.Windows.Forms.RadioButton();
            this.radioAllowList            = new System.Windows.Forms.RadioButton();
            this.txtIpList                 = new System.Windows.Forms.TextBox();
            this.labelIpExample            = new System.Windows.Forms.Label();
            this.labelLogging              = new System.Windows.Forms.Label();
            this.chkEnableLogging          = new System.Windows.Forms.CheckBox();
            this.labelDaysKept             = new System.Windows.Forms.Label();
            this.numRetentionDays          = new System.Windows.Forms.NumericUpDown();
            this.btnViewLogs               = new System.Windows.Forms.Button();
            this.btnSave                   = new System.Windows.Forms.Button();
            this.btnClose                  = new System.Windows.Forms.Button();
            this.labelServiceStatusCaption = new System.Windows.Forms.Label();
            this.labelServiceStatus        = new System.Windows.Forms.Label();
            this.lblVersion                = new System.Windows.Forms.Label();
            this.linkRepo                  = new System.Windows.Forms.LinkLabel();

            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetentionDays)).BeginInit();
            this.SuspendLayout();

            // 
            // MainForm
            // 
            this.AutoScaleDimensions      = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode            = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize               = new System.Drawing.Size(450, 475);
            this.FormBorderStyle          = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox              = false;
            this.StartPosition            = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text                     = "SMTP Relay Configuration";

            // 
            // labelHost
            // 
            this.labelHost.AutoSize       = true;
            this.labelHost.Location       = new System.Drawing.Point(20, 20);
            this.labelHost.Name           = "labelHost";
            this.labelHost.Size           = new System.Drawing.Size(81, 17);
            this.labelHost.Text           = "SMTP Host:";

            // 
            // txtHost
            // 
            this.txtHost.Location         = new System.Drawing.Point(120, 17);
            this.txtHost.Name             = "txtHost";
            this.txtHost.Size             = new System.Drawing.Size(300, 22);

            // 
            // labelPort
            // 
            this.labelPort.AutoSize       = true;
            this.labelPort.Location       = new System.Drawing.Point(20, 60);
            this.labelPort.Name           = "labelPort";
            this.labelPort.Size           = new System.Drawing.Size(38, 17);
            this.labelPort.Text           = "Port:";

            // 
            // numPort
            // 
            this.numPort.Location         = new System.Drawing.Point(120, 58);
            this.numPort.Maximum          = new decimal(new int[] { 65535, 0, 0, 0 });
            this.numPort.Minimum          = new decimal(new int[] { 1, 0, 0, 0 });
            this.numPort.Name             = "numPort";
            this.numPort.Size             = new System.Drawing.Size(60, 22);
            this.numPort.Value            = new decimal(new int[] { 25, 0, 0, 0 });

            // 
            // chkStartTls
            // 
            this.chkStartTls.AutoSize     = true;
            this.chkStartTls.Location     = new System.Drawing.Point(200, 60);
            this.chkStartTls.Name         = "chkStartTls";
            this.chkStartTls.Size         = new System.Drawing.Size(80, 21);
            this.chkStartTls.Text         = "STARTTLS";
            this.chkStartTls.CheckedChanged += new System.EventHandler(this.chkStartTls_CheckedChanged);

            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize   = true;
            this.labelUsername.Location   = new System.Drawing.Point(20, 100);
            this.labelUsername.Name       = "labelUsername";
            this.labelUsername.Size       = new System.Drawing.Size(77, 17);
            this.labelUsername.Text       = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location     = new System.Drawing.Point(120, 97);
            this.txtUsername.Name         = "txtUsername";
            this.txtUsername.Size         = new System.Drawing.Size(300, 22);
            this.txtUsername.Enabled      = false;

            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize   = true;
            this.labelPassword.Location   = new System.Drawing.Point(20, 140);
            this.labelPassword.Name       = "labelPassword";
            this.labelPassword.Size       = new System.Drawing.Size(73, 17);
            this.labelPassword.Text       = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location     = new System.Drawing.Point(120, 137);
            this.txtPassword.Name         = "txtPassword";
            this.txtPassword.Size         = new System.Drawing.Size(300, 22);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Enabled      = false;

            // 
            // labelRelayRestrictions
            // 
            this.labelRelayRestrictions.AutoSize = true;
            this.labelRelayRestrictions.Location = new System.Drawing.Point(20, 180);
            this.labelRelayRestrictions.Name     = "labelRelayRestrictions";
            this.labelRelayRestrictions.Size     = new System.Drawing.Size(130, 17);
            this.labelRelayRestrictions.Text     = "Relay Restrictions:";

            // 
            // radioAllowAll
            // 
            this.radioAllowAll.AutoSize    = true;
            this.radioAllowAll.Location    = new System.Drawing.Point(160, 178);
            this.radioAllowAll.Name        = "radioAllowAll";
            this.radioAllowAll.Size        = new System.Drawing.Size(82, 21);
            this.radioAllowAll.Text        = "Allow All";
            this.radioAllowAll.Checked     = true;
            this.radioAllowAll.CheckedChanged += new System.EventHandler(this.radioAllowRestrictions_CheckedChanged);

            // 
            // radioAllowList
            // 
            this.radioAllowList.AutoSize   = true;
            this.radioAllowList.Location   = new System.Drawing.Point(260, 178);
            this.radioAllowList.Name       = "radioAllowList";
            this.radioAllowList.Size       = new System.Drawing.Size(131, 21);
            this.radioAllowList.Text       = "Allow Specified";
            this.radioAllowList.CheckedChanged += new System.EventHandler(this.radioAllowRestrictions_CheckedChanged);

            // 
            // txtIpList
            // 
            this.txtIpList.Location        = new System.Drawing.Point(120, 205);
            this.txtIpList.Multiline       = true;
            this.txtIpList.Name            = "txtIpList";
            this.txtIpList.ScrollBars      = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIpList.Size            = new System.Drawing.Size(300, 80);
            this.txtIpList.Enabled         = false;

            // 
            // labelIpExample
            // 
            this.labelIpExample.AutoSize   = true;
            this.labelIpExample.Location   = new System.Drawing.Point(120, 290);
            this.labelIpExample.Name       = "labelIpExample";
            this.labelIpExample.Size       = new System.Drawing.Size(212, 17);
            this.labelIpExample.ForeColor  = System.Drawing.SystemColors.GrayText;
            this.labelIpExample.Text       = "e.g. 127.0.0.1, 10.0.0.0/24";

            // 
            // labelLogging
            // 
            this.labelLogging.AutoSize     = true;
            this.labelLogging.Location     = new System.Drawing.Point(20, 330);
            this.labelLogging.Name         = "labelLogging";
            this.labelLogging.Size         = new System.Drawing.Size(65, 17);
            this.labelLogging.Text         = "Logging:";

            // 
            // chkEnableLogging
            // 
            this.chkEnableLogging.AutoSize = true;
            this.chkEnableLogging.Location = new System.Drawing.Point(120, 328);
            this.chkEnableLogging.Name     = "chkEnableLogging";
            this.chkEnableLogging.Size     = new System.Drawing.Size(74, 21);
            this.chkEnableLogging.Text     = "Enable";
            this.chkEnableLogging.CheckedChanged += new System.EventHandler(this.chkEnableLogging_CheckedChanged);

            // 
            // labelDaysKept
            // 
            this.labelDaysKept.AutoSize    = true;
            this.labelDaysKept.Location    = new System.Drawing.Point(220, 330);
            this.labelDaysKept.Name        = "labelDaysKept";
            this.labelDaysKept.Size        = new System.Drawing.Size(80, 17);
            this.labelDaysKept.Text        = "Days Kept:";

            // 
            // numRetentionDays
            // 
            this.numRetentionDays.Location  = new System.Drawing.Point(305, 328);
            this.numRetentionDays.Maximum   = new decimal(new int[] {365, 0, 0, 0});
            this.numRetentionDays.Minimum   = new decimal(new int[] {1, 0, 0, 0});
            this.numRetentionDays.Name      = "numRetentionDays";
            this.numRetentionDays.Size      = new System.Drawing.Size(60, 22);
            this.numRetentionDays.Value     = new decimal(new int[] {30, 0, 0, 0});

            // 
            // btnViewLogs
            // 
            this.btnViewLogs.Location      = new System.Drawing.Point(380, 325);
            this.btnViewLogs.Name          = "btnViewLogs";
            this.btnViewLogs.Size          = new System.Drawing.Size(80, 27);
            this.btnViewLogs.Text          = "View Logs";
            this.btnViewLogs.Click        += new System.EventHandler(this.btnViewLogs_Click);

            // 
            // btnSave
            // 
            this.btnSave.Location          = new System.Drawing.Point(120, 370);
            this.btnSave.Name              = "btnSave";
            this.btnSave.Size              = new System.Drawing.Size(100, 30);
            this.btnSave.Text              = "Save and Restart";
            this.btnSave.Click            += new System.EventHandler(this.btnSave_Click);

            // 
            // btnClose
            // 
            this.btnClose.Location         = new System.Drawing.Point(240, 370);
            this.btnClose.Name             = "btnClose";
            this.btnClose.Size             = new System.Drawing.Size(100, 30);
            this.btnClose.Text             = "Close";
            this.btnClose.Click           += new System.EventHandler(this.btnClose_Click);

            // 
            // labelServiceStatusCaption
            // 
            this.labelServiceStatusCaption.AutoSize = true;
            this.labelServiceStatusCaption.Font      = new Font(System.Drawing.FontFamily.GenericSansSerif,  nineF:9.75F, System.Drawing.FontStyle.Bold);
            this.labelServiceStatusCaption.Location  = new System.Drawing.Point(20, 420);
            this.labelServiceStatusCaption.Name      = "labelServiceStatusCaption";
            this.labelServiceStatusCaption.Size      = new System.Drawing.Size(115, 17);
            this.labelServiceStatusCaption.Text      = "Service Status:";

            // 
            // labelServiceStatus
            // 
            this.labelServiceStatus.AutoSize   = true;
            this.labelServiceStatus.Font      = new Font(System.Drawing.FontFamily.GenericSansSerif,  nineF:9.75F, System.Drawing.FontStyle.Bold);
            this.labelServiceStatus.Location  = new System.Drawing.Point(140, 420);
            this.labelServiceStatus.Name      = "labelServiceStatus";
            this.labelServiceStatus.Size      = new System.Drawing.Size(100, 17);
            this.labelServiceStatus.Text      = "Unknown";

            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize   = true;
            this.lblVersion.Location   = new System.Drawing.Point(20, 450);
            this.lblVersion.Name       = "lblVersion";
            this.lblVersion.Size       = new System.Drawing.Size(60, 17);
            this.lblVersion.Text       = "Version:";

            // 
            // linkRepo
            // 
            this.linkRepo.AutoSize      = true;
            this.linkRepo.Location      = new System.Drawing.Point(120, 450);
            this.linkRepo.Name          = "linkRepo";
            this.linkRepo.Size          = new System.Drawing.Size(230, 17);
            this.linkRepo.Text          = "https://github.com/mkitchingh/Smtp-Relay";
            this.linkRepo.LinkClicked  += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRepo_LinkClicked);

            // 
            // Add controls to form
            // 
            this.Controls.Add(this.labelHost);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.chkStartTls);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.labelRelayRestrictions);
            this.Controls.Add(this.radioAllowAll);
            this.Controls.Add(this.radioAllowList);
            this.Controls.Add(this.txtIpList);
            this.Controls.Add(this.labelIpExample);
            this.Controls.Add(this.labelLogging);
            this.Controls.Add(this.chkEnableLogging);
            this.Controls.Add(this.labelDaysKept);
            this.Controls.Add(this.numRetentionDays);
            this.Controls.Add(this.btnViewLogs);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.labelServiceStatusCaption);
            this.Controls.Add(this.labelServiceStatus);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.linkRepo);

            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetentionDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
