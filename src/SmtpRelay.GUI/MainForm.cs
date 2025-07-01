// File: src/SmtpRelay.GUI/MainForm.cs

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    public partial class MainForm : Form
    {
        private readonly string _serviceName;

        public MainForm()
        {
            InitializeComponent();

            // --- Discover Windows‐service name by display name ---
            _serviceName = ServiceController
                .GetServices()
                .FirstOrDefault(s =>
                    s.DisplayName.Equals("SMTP Relay Service", StringComparison.OrdinalIgnoreCase))
                ?.ServiceName ?? "SmtpRelay";

            // --- Safely get version from the running EXE, even in single‐file publish ---
            var exeName = Assembly.GetExecutingAssembly().GetName().Name + ".exe";
            var exePath = Path.Combine(AppContext.BaseDirectory, exeName);
            string ver;
            try
            {
                ver = FileVersionInfo.GetVersionInfo(exePath).ProductVersion;
            }
            catch
            {
                ver = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
            }
            lblVersion.Text = $"Version: {ver}";

            // Wire up initial states
            chkStartTls_CheckedChanged(null, EventArgs.Empty);
            radioAllowRestrictions_CheckedChanged(null, EventArgs.Empty);
            UpdateServiceStatus();
        }

        private void UpdateServiceStatus()
        {
            try
            {
                using var sc = new ServiceController(_serviceName);
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    labelServiceStatus.Text       = "Service Running";
                    labelServiceStatus.ForeColor  = Color.Green;
                }
                else
                {
                    labelServiceStatus.Text       = "Service Stopped";
                    labelServiceStatus.ForeColor  = Color.Red;
                }
            }
            catch
            {
                labelServiceStatus.Text       = "Service Unknown";
                labelServiceStatus.ForeColor  = Color.Gray;
            }
        }

        private void chkStartTls_CheckedChanged(object sender, EventArgs e)
        {
            bool tls = chkStartTls.Checked;
            lblUsername.Enabled  =
            txtUsername.Enabled  =
            lblPassword.Enabled  =
            txtPassword.Enabled  = tls;

            // default ports
            numPort.Value = tls ? 587 : 25;
        }

        private void radioAllowRestrictions_CheckedChanged(object sender, EventArgs e)
        {
            txtIpList.Enabled = radioAllowList.Checked;
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            var logDir = Path.Combine(
                AppContext.BaseDirectory, "logs");
            if (Directory.Exists(logDir))
                Process.Start("explorer.exe", logDir);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // --- Your existing Config.Save(...) call goes here ---

            // Restart service
            try
            {
                using var sc = new ServiceController(_serviceName);
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                }
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));

                MessageBox.Show(
                    "Settings saved and service restarted.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to restart service:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            UpdateServiceStatus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkRepo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName        = linkRepo.Text,
                UseShellExecute = true
            });
        }
    }
}
