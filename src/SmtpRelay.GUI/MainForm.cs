// File: src/SmtpRelay.GUI/MainForm.cs

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            // Opens the actual log folder under Program Files
            var baseDir = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service", "logs");
            Process.Start("explorer.exe", baseDir);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Your existing save logic goes here...
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
