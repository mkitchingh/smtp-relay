using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Management;

namespace SmtpRelay.GUI
{
    public partial class MainForm : Form
    {
        private readonly string _serviceName;

        public MainForm()
        {
            InitializeComponent();

            // discover the real Windows service name by display name
            _serviceName = ServiceController
                .GetServices()
                .FirstOrDefault(s =>
                    s.DisplayName.Equals("SMTP Relay Service", StringComparison.OrdinalIgnoreCase))
                ?.ServiceName ?? "SmtpRelay";

            // set version to the FileVersion (e.g. 1.4.0.0)
            var ver = FileVersionInfo
                .GetVersionInfo(Assembly.GetExecutingAssembly().Location)
                .ProductVersion;
            lblVersion.Text = $"Version: {ver}";

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
                    labelServiceStatus.Text = "Service Running";
                    labelServiceStatus.ForeColor = Color.Green;
                }
                else
                {
                    labelServiceStatus.Text = "Service Stopped";
                    labelServiceStatus.ForeColor = Color.Red;
                }
            }
            catch
            {
                labelServiceStatus.Text = "Service Unknown";
                labelServiceStatus.ForeColor = Color.Gray;
            }
        }

        private void chkStartTls_CheckedChanged(object sender, EventArgs e)
        {
            bool en = chkStartTls.Checked;
            lblUsername.Enabled = en;
            txtUsername.Enabled = en;
            lblPassword.Enabled = en;
            txtPassword.Enabled = en;
            numPort.Value = en ? 587 : 25;
        }

        private void radioAllowRestrictions_CheckedChanged(object sender, EventArgs e)
        {
            txtIpList.Enabled = radioAllowList.Checked;
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service", "logs");
            if (Directory.Exists(path))
                Process.Start("explorer.exe", path);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // existing SaveConfig() logic here...
            // then restart the Windows service:
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
                MessageBox.Show("Settings saved and service restarted.", 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to restart service:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateServiceStatus();
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();

        private void linkRepo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = linkRepo.Text,
                UseShellExecute = true
            });
        }
    }
}
