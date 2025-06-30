using System;
using System.IO;                             // ← added for IOException & Path
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using System.Windows.Forms;
using SmtpRelay;

namespace SmtpRelay.GUI
{
    public partial class MainForm : Form
    {
        // ── Controls ────────────────────────────────────────────
        TextBox host, port, user, pass, ips;
        RadioButton allowAll, allowListed;
        CheckBox starttls, enableLog;
        NumericUpDown keepDays;
        Button btnSave, btnClose, btnLogs;
        Label L(int x,int y,string t) => new() { Left = x, Top = y, Text = t, AutoSize = true };

        public MainForm()
        {
            // Require elevation
            if (!IsAdministrator())
            {
                MessageBox.Show(
                    "You must run SMTP Relay Config as Administrator to view or change settings.",
                    "Elevation required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(0);
            }

            InitializeComponent();
            LoadCfg();
        }

        static bool IsAdministrator()
        {
            using var id = System.Security.Principal.WindowsIdentity.GetCurrent();
            return new System.Security.Principal.WindowsPrincipal(id)
                   .IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        // ── Build UI ────────────────────────────────────────────
        void InitializeComponent()
        {
            Controls.Clear();
            Width = 1020; Height = 720;
            Text = "SMTP Relay Config";
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            int lblX = 40, inX = 260, y = 32, h = 38, gap = 20;

            // SMTP Host
            Controls.Add(L(lblX, y+8, "SMTP Host:"));
            host = new() { Left = inX, Top = y, Width = 600 };
            Controls.Add(host);

            // Port + STARTTLS
            y += h + gap;
            Controls.Add(L(lblX, y+8, "Port:"));
            port = new() { Left = inX, Top = y, Width = 90 };
            Controls.Add(port);
            starttls = new() { Left = inX+120, Top = y+4, Text = "STARTTLS", AutoSize = true };
            starttls.CheckedChanged += StarttlsChanged;
            Controls.Add(starttls);

            // Username
            y += h + gap;
            Controls.Add(L(lblX, y+8, "Username:"));
            user = new() { Left = inX, Top = y, Width = 280 };
            Controls.Add(user);

            // Password
            y += h + gap;
            Controls.Add(L(lblX, y+8, "Password:"));
            pass = new() { Left = inX, Top = y, Width = 280, UseSystemPasswordChar = true };
            Controls.Add(pass);

            // Relay Restrictions
            y += h + gap;
            Controls.Add(L(lblX, y+8, "Relay Restrictions:"));
            allowAll = new() { Left = inX,       Top = y+6, Text = "Allow All",       AutoSize = true };
            allowListed = new() { Left = inX+170, Top = y+6, Text = "Allow Specified", AutoSize = true };
            allowAll.CheckedChanged    += (_,_) => ToggleIP();
            allowListed.CheckedChanged += (_,_) => ToggleIP();
            Controls.AddRange(new[] { allowAll, allowListed });

            // IP Allowed List
            y += h + gap;
            Controls.Add(L(lblX, y+8, "IP Allowed List:"));
            ips = new() { Left = inX, Top = y, Width = 650 };
            Controls.Add(ips);
            Controls.Add(L(inX, y+28,
                "Example: 192.168.1.0/24 , 10.0.0.5 , 2001:db8::/32"));

            // Logging
            y += h + gap + 26;
            Controls.Add(L(lblX, y+8, "Logging:"));
            enableLog = new() { Left = inX, Top = y+4, Text = "Enable Logging", AutoSize = true };
            Controls.Add(enableLog);
            Controls.Add(L(inX+200, y+8, "Days kept:"));
            keepDays = new() { Left = inX+300, Top = y, Width = 120, Minimum = 1, Maximum = 365, Value = 30 };
            Controls.Add(keepDays);

            // View Logs button
            btnLogs = new() { Left = inX, Top = y+40, Width = 180, Height = 40, Text = "View Logs" };
            btnLogs.Click += (_,_) => OpenLogs();
            Controls.Add(btnLogs);

            // Save / Close (shifted far left)
            btnSave  = new() { Left = lblX,      Top = y+120, Width = 200, Height = 48, Text = "Save"  };
            btnClose = new() { Left = lblX + 220, Top = y+120, Width = 200, Height = 48, Text = "Close" };
            btnSave.Click  += SaveCfg;
            btnClose.Click += (_,_) => Close();
            Controls.AddRange(new[] { btnSave, btnClose });

            // Under Close: service remains running
            Controls.Add(L(btnClose.Left, btnClose.Bottom+6, "Service will\nremain running"));

            // Right-side info
            Controls.Add(L(Width-260, Height-160, "Version: 1.4"));
            var link = new LinkLabel
            {
                Left = Width-260,
                Top = Height-130,
                Text = "Project site",
                AutoSize = true
            };
            link.Links.Add(0, link.Text.Length, "https://github.com/mkitchingh/Smtp-Relay");
            link.LinkClicked += (_, e) => System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo(e.Link.LinkData.ToString())
                { UseShellExecute = true });
            Controls.Add(link);
        }

        // ── Helpers ──────────────────────────────────────────
        void ToggleAuth() => user.Enabled = pass.Enabled = starttls.Checked;
        void ToggleIP()   => ips.Enabled  = allowListed.Checked;

        void StarttlsChanged(object? _, EventArgs? __)
        {
            if (int.TryParse(port.Text, out var p))
            {
                if (starttls.Checked && (p == 25 || p == 465 || p == 2525))
                    port.Text = "587";
                else if (!starttls.Checked && (p == 587 || p == 465 || p == 2525))
                    port.Text = "25";
            }
            ToggleAuth();
        }

        void LoadCfg()
        {
            var c = Config.Load();
            host.Text      = c.SmartHost;
            port.Text      = c.SmartHostPort.ToString();
            user.Text      = c.Username;
            pass.Text      = c.Password;
            allowAll.Checked    = c.AllowAllIPs;
            allowListed.Checked = !c.AllowAllIPs;
            ips.Text       = string.Join(",", c.AllowedIPs);
            starttls.Checked   = c.UseStartTls;
            enableLog.Checked  = c.EnableLogging;
            keepDays.Value     = c.RetentionDays;
            ToggleAuth(); ToggleIP();
        }

        void SaveCfg(object? _, EventArgs? __)
        {
            var cfg = new Config
            {
                SmartHost     = host.Text.Trim(),
                SmartHostPort = int.TryParse(port.Text, out var p) ? p : 25,
                Username      = user.Text.Trim(),
                Password      = pass.Text,
                AllowAllIPs   = allowAll.Checked,
                AllowedIPs    = ips.Text
                                   .Split(new[]{',',';'}, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(s => s.Trim()).ToList(),
                UseStartTls   = starttls.Checked,
                EnableLogging = enableLog.Checked,
                RetentionDays = (int)keepDays.Value
            };

            try
            {
                cfg.Save();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Validation error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "I/O error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RestartSvc();
            MessageBox.Show("Settings saved and service restarted.");
        }

        void RestartSvc()
        {
            try
            {
                using var sc = new ServiceController("SMTPRelayService");
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped,
                        TimeSpan.FromSeconds(10));
                }
                sc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Service error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void OpenLogs()
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay","service","logs");
            System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo("explorer", dir)
                { UseShellExecute = true });
        }
    }
}
