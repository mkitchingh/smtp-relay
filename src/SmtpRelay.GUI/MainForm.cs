using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.ServiceProcess;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    public class MainForm : Form
    {
        private TextBox ipTextBox;
        private Label formatLabel;
        private Button saveButton;

        public MainForm()
        {
            this.Text = "SMTP Relay Config";
            this.Width = 400;
            this.Height = 200;

            formatLabel = new Label
            {
                Text = "Format: 192.168.1.10 or 10.0.0.0/24",
                Top = 20,
                Left = 20,
                AutoSize = true
            };
            this.Controls.Add(formatLabel);

            ipTextBox = new TextBox
            {
                Top = 50,
                Left = 20,
                Width = 340
            };
            this.Controls.Add(ipTextBox);

            saveButton = new Button
            {
                Text = "Save and Restart Service",
                Top = 90,
                Left = 20,
                Width = 200
            };
            saveButton.Click += SaveButton_Click;
            this.Controls.Add(saveButton);

            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!IsRunningAsAdministrator())
            {
                MessageBox.Show("This application must be run as administrator.", "Permission Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(1);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string configPath = @"C:\ProgramData\SMTPRelay";
                Directory.CreateDirectory(configPath);
                File.WriteAllText(Path.Combine(configPath, "config.txt"), ipTextBox.Text);
                RestartService("SmtpRelay");
                MessageBox.Show("Configuration saved and service restarted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save configuration: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestartService(string serviceName)
        {
            try
            {
                ServiceController sc = new ServiceController(serviceName);
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error restarting service: " + ex.Message, "Service Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool IsRunningAsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}