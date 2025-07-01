using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            // set version from assembly
            lblVersion.Text = $"Version: {Assembly.GetExecutingAssembly().GetName().Version}";
            chkStartTls_CheckedChanged(this, EventArgs.Empty);
            radioAllowRestrictions_CheckedChanged(this, EventArgs.Empty);
            UpdateServiceStatus();
        }

        private void UpdateServiceStatus()
        {
            try
            {
                using var sc = new ServiceController("SmtpRelay");
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
            // enable creds only when STARTTLS is checked
            bool enabled = chkStartTls.Checked;
            lblUsername.Enabled = enabled;
            txtUsername.Enabled = enabled;
            lblPassword.Enabled = enabled;
            txtPassword.Enabled = enabled;

            // default port
            numPort.Value = enabled ? 587 : 25;
        }

        private void radioAllowRestrictions_CheckedChanged(object sender, EventArgs e)
        {
            // ip list only when "Allow Specified"
            txtIpList.Enabled = radioAllowList.Checked;
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            var baseDir = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service", "logs");
            Process.Start("explorer.exe", baseDir);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // your existing save & restart logic...
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
                FileName = linkRepo.Text,
                UseShellExecute = true
            });
        }
    }
}
