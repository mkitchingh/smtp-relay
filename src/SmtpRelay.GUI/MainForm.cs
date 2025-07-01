using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using SmtpRelay;

namespace SmtpRelay.GUI
{
    public partial class MainForm : Form
    {
        private readonly Timer _statusTimer;

        public MainForm()
        {
            InitializeComponent();

            // load config into controls...
            var cfg = Config.Load();
            txtHost.Text = cfg.SmartHost;
            numPort.Value = cfg.SmartHostPort;
            chkStartTls.Checked = cfg.UseStartTls;
            txtUsername.Text = cfg.Username;
            txtPassword.Text = cfg.Password;
            radAllowAll.Checked = cfg.AllowAllIPs;
            radAllowSpecified.Checked = !cfg.AllowAllIPs;
            txtAllowedIPs.Lines = cfg.AllowedIPs.ToArray();
            chkEnableLogging.Checked = cfg.EnableLogging;
            numDays.Value = Math.Max(1, cfg.RetentionDays);

            // load icon
            var ico = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "smtp.ico");
            if (File.Exists(ico))
                Icon = new Icon(ico);

            // make form resizable
            MinimumSize = new Size(500, 400);

            // timer to refresh service status
            _statusTimer = new Timer { Interval = 5000 };
            _statusTimer.Tick += (s, e) => RefreshServiceStatus();
            _statusTimer.Start();
            RefreshServiceStatus();

            // hookup enable/disable behavior
            radAllowAll.CheckedChanged += (s, e) =>
                txtAllowedIPs.Enabled = radAllowSpecified.Checked;
            chkEnableLogging.CheckedChanged += (s, e) =>
                numDays.Enabled = chkEnableLogging.Checked;
        }

        private void RefreshServiceStatus()
        {
            try
            {
                using var sc = new ServiceController("SMTP Relay");
                var running = sc.Status == ServiceControllerStatus.Running;
                labelServiceStatus.Text = running
                    ? "Service Running"
                    : "Service Stopped";
                labelServiceStatus.ForeColor = running
                    ? Color.Green
                    : Color.Red;
            }
            catch
            {
                labelServiceStatus.Text = "Service Unknown";
                labelServiceStatus.ForeColor = Color.Gray;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var cfg = new Config
            {
                SmartHost      = txtHost.Text,
                SmartHostPort  = (int)numPort.Value,
                Username       = txtUsername.Text,
                Password       = txtPassword.Text,
                UseStartTls    = chkStartTls.Checked,
                AllowAllIPs    = radAllowAll.Checked,
                AllowedIPs     = radAllowAll.Checked
                    ? new List<string>()
                    : txtAllowedIPs.Lines.Where(l => !string.IsNullOrWhiteSpace(l)).ToList(),
                EnableLogging  = chkEnableLogging.Checked,
                RetentionDays  = (int)numDays.Value
            };

            try
            {
                cfg.Save();

                // restart the Windows service
                using var sc = new ServiceController("SMTP Relay");
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));

                MessageBox.Show(
                    "Settings saved and service restarted.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error saving settings:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            var dir = Program.GetServiceLogDirectory();
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            System.Diagnostics.Process.Start("explorer.exe", dir);
        }
    }
}
