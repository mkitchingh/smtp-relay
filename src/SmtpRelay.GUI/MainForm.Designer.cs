namespace SmtpRelay.GUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel tableLayout;
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
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.tableLayout     = new System.Windows.Forms.TableLayoutPanel();
            this.lblSmtpHost     = new System.Windows.Forms.Label();
            this.txtSmtpHost     = new System.Windows.Forms.TextBox();
            this.lblPort         = new System.Windows.Forms.Label();
            this.numPort         = new System.Windows.Forms.NumericUpDown();
            this.chkStartTls     = new System.Windows.Forms.CheckBox();
            this.lblUsername     = new System.Windows.Forms.Label();
            this.txtUsername     = new System.Windows.Forms.TextBox();
            this.lblPassword     = new System.Windows.Forms.Label();
            this.txtPassword     = new System.Windows.Forms.TextBox();
            this.lblRestrictions = new System.Windows.Forms.Label();
            this.radioAllowAll   = new System.Windows.Forms.RadioButton();
            this.radioAllowList  = new System.Windows.Forms.RadioButton();
            this.txtIpList       = new System.Windows.Forms.TextBox();
            this.lblIpSample     = new System.Windows.Forms.Label();
            this.lblLogging      = new System.Windows.Forms.Label();
            this.chkEnableLogging= new System.Windows.Forms.CheckBox();
            this.lblDaysKept     = new System.Windows.Forms.Label();
            this.numDaysKept     = new System.Windows.Forms.NumericUpDown();
            this.btnViewLogs     = new System.Windows.Forms.Button();
            this.btnSave         = new System.Windows.Forms.Button();
            this.btnClose        = new System.Windows.Forms.Button();
            this.lblVersion      = new System.Windows.Forms.Label();
            this.linkRepo        = new System.Windows.Forms.LinkLabel();
            this.labelServiceStatus = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDaysKept)).BeginInit();
            this.SuspendLayout();

            // 
            // tableLayout
            // 
            this.tableLayout.ColumnCount = 2;
            this.tableLayout.RowCount    = 12;
            this.tableLayout.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.AutoSize    = true;
            this.tableLayout.AutoSizeMode= System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            for (int i = 0; i < 12; i++)
                this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // 
            // Row 0: SMTP Host
            // 
            this.lblSmtpHost.Text = "SMTP Host:";
            this.lblSmtpHost.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayout.Controls.Add(this.lblSmtpHost, 0, 0);
            this.txtSmtpHost.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtSmtpHost.Width  = 400;
            this.tableLayout.Controls.Add(this.txtSmtpHost, 1, 0);

            // 
            // Row 1: Port + STARTTLS
            // 
            this.lblPort.Text = "Port:";
            this.tableLayout.Controls.Add(this.lblPort, 0, 1);
            var flpPort = new System.Windows.Forms.FlowLayoutPanel
            {
                AutoSize     = true,
                Dock         = System.Windows.Forms.DockStyle.Fill,
                FlowDirection= System.Windows.Forms.FlowDirection.LeftToRight
            };
            this.numPort.Minimum = 1;
            this.numPort.Maximum = 65535;
            this.numPort.Value   = 25;
            flpPort.Controls.Add(this.numPort);
            this.chkStartTls.Text = "STARTTLS";
            flpPort.Controls.Add(this.chkStartTls);
            this.tableLayout.Controls.Add(flpPort, 1, 1);

            // 
            // Row 2: Username
            // 
            this.lblUsername.Text = "Username:";
            this.tableLayout.Controls.Add(this.lblUsername, 0, 2);
            this.txtUsername.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.tableLayout.Controls.Add(this.txtUsername, 1, 2);

            // 
            // Row 3: Password
            // 
            this.lblPassword.Text = "Password:";
            this.tableLayout.Controls.Add(this.lblPassword, 0, 3);
            this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtPassword.PasswordChar = '*';
            this.tableLayout.Controls.Add(this.txtPassword, 1, 3);

            // 
            // Row 4: Relay Restrictions
            // 
            this.lblRestrictions.Text = "Relay Restrictions:";
            this.tableLayout.Controls.Add(this.lblRestrictions, 0, 4);
            var flpRel = new System.Windows.Forms.FlowLayoutPanel
            {
                AutoSize      = true,
                Dock          = System.Windows.Forms.DockStyle.Fill,
                FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            };
            this.radioAllowAll.Text  = "Allow All";
            this.radioAllowList.Text = "Allow Specified";
            flpRel.Controls.Add(this.radioAllowAll);
            flpRel.Controls.Add(this.radioAllowList);
            this.tableLayout.Controls.Add(flpRel, 1, 4);

            // 
            // Row 5: IP List
            // 
            this.txtIpList.Multiline = true;
            this.txtIpList.Height    = 80;
            this.txtIpList.ScrollBars= System.Windows.Forms.ScrollBars.Vertical;
            this.tableLayout.Controls.Add(this.txtIpList, 1, 5);

            // 
            // Row 6: IP Sample
            // 
            this.lblIpSample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblIpSample.Text      = "e.g. 192.168.1.0/24, 10.0.0.1, 2001:db8::/32";
            this.tableLayout.Controls.Add(this.lblIpSample, 1, 6);

            // 
            // Row 7: Logging
            // 
            this.lblLogging.Text = "Logging:";
            this.tableLayout.Controls.Add(this.lblLogging, 0, 7);
            var flpLog = new System.Windows.Forms.FlowLayoutPanel
            {
                AutoSize      = true,
                Dock          = System.Windows.Forms.DockStyle.Fill,
                FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            };
            this.chkEnableLogging.Text = "Enable Logging";
            flpLog.Controls.Add(this.chkEnableLogging);
            flpLog.Controls.Add(this.lblDaysKept);
            this.lblDaysKept.Text = "Days Kept:";
            flpLog.Controls.Add(this.numDaysKept);
            this.numDaysKept.Minimum=1;
            this.numDaysKept.Value  =30;
            this.btnViewLogs.Text   = "View Logs";
            flpLog.Controls.Add(this.btnViewLogs);
            this.tableLayout.Controls.Add(flpLog, 1, 7);

            // 
            // Row 8: Buttons
            // 
            var flpBtns = new System.Windows.Forms.FlowLayoutPanel
            {
                AutoSize      = true,
                Dock          = System.Windows.Forms.DockStyle.Fill,
                FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            };
            this.btnSave.Text  = "Save and Restart";
            this.btnClose.Text = "Close";
            flpBtns.Controls.Add(this.btnSave);
            flpBtns.Controls.Add(this.btnClose);
            this.tableLayout.Controls.Add(flpBtns, 1, 8);

            // 
            // Row 9: Service Status
            // 
            this.labelServiceStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.tableLayout.Controls.Add(this.labelServiceStatus, 1, 9);

            // 
            // Row 10: Version
            // 
            this.tableLayout.Controls.Add(this.lblVersion, 0, 10);
            this.tableLayout.Controls.Add(this.linkRepo, 1, 10);

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tableLayout);
            this.Text = "SMTP Relay Configuration";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(600, 500);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDaysKept)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
