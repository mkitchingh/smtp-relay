
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    public class MainForm : Form
    {
        private TextBox txtHost, txtPort, txtUsername, txtPassword, txtIPAllow;
        private CheckBox chkSsl;
        private Button btnSave;

        public MainForm()
        {
            Text = "SMTP Relay Config";
            Width = 500;
            Height = 400;
            Load += (s, e) => EnsureAdmin();
            InitializeComponents();
        }

        private void EnsureAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("This app needs to run as Administrator.", "Elevation Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Process.Start(new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    UseShellExecute = true,
                    Verb = "runas"
                });
                Environment.Exit(0);
            }
        }

        private void InitializeComponents()
        {
            Label lblHost = new Label { Text = "Smart Host", Top = 20, Left = 20, Width = 100 };
            txtHost = new TextBox { Top = 20, Left = 130, Width = 300 };

            Label lblPort = new Label { Text = "Port", Top = 60, Left = 20, Width = 100 };
            txtPort = new TextBox { Top = 60, Left = 130, Width = 100 };

            chkSsl = new CheckBox { Text = "Use SSL/TLS", Top = 100, Left = 130 };

            Label lblUser = new Label { Text = "Username", Top = 140, Left = 20, Width = 100 };
            txtUsername = new TextBox { Top = 140, Left = 130, Width = 300 };

            Label lblPass = new Label { Text = "Password", Top = 180, Left = 20, Width = 100 };
            txtPassword = new TextBox { Top = 180, Left = 130, Width = 300, UseSystemPasswordChar = true };

            Label lblIP = new Label { Text = "Allowlist IPs", Top = 220, Left = 20, Width = 100 };
            txtIPAllow = new TextBox { Top = 220, Left = 130, Width = 300 };

            btnSave = new Button { Text = "Save", Top = 270, Left = 130, Width = 100 };
            btnSave.Click += (s, e) => SaveSettings();

            Controls.AddRange(new Control[] {
                lblHost, txtHost, lblPort, txtPort, chkSsl, lblUser, txtUsername,
                lblPass, txtPassword, lblIP, txtIPAllow, btnSave
            });
        }

        private void SaveSettings()
        {
            string config = $"host={txtHost.Text}\nport={txtPort.Text}\nssl={chkSsl.Checked}\nuser={txtUsername.Text}\npass={txtPassword.Text}\nipallow={txtIPAllow.Text}";
            File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SMTPRelay", "config.txt"), config);
            RestartService();
            MessageBox.Show("Settings saved and service restarted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RestartService()
        {
            try
            {
                Process.Start(new ProcessStartInfo("sc.exe", "stop SmtpRelay") { CreateNoWindow = true, UseShellExecute = false });
                Process.Start(new ProcessStartInfo("sc.exe", "start SmtpRelay") { CreateNoWindow = true, UseShellExecute = false });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to restart service: " + ex.Message);
            }
        }
    }
}
