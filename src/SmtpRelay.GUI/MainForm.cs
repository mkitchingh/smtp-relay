
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "SMTP Relay Config";
            Width = 600;
            Height = 400;
            Load += (s, e) => EnsureAdmin();
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
    }
}
