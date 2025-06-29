using System;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using SmtpRelay;

namespace SmtpRelay.GUI
{
    public partial class MainForm : Form
    {
        TextBox host, port, user, pass, ips;
        RadioButton allowAll, allowListed;
        CheckBox starttls, enableLog;
        NumericUpDown keepDays;
        Button btnSave, btnClose;

        public MainForm() { InitializeComponent(); LoadCfg(); }

        Label L(int x, int y, string t) => new() { Left = x, Top = y, Text = t, AutoSize = true };

        void InitializeComponent()
        {
            Controls.Clear();
            Width = 940; Height = 560; Text = "SMTP Relay Config";
            Icon  = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            int lblX = 32, inX = 220, y = 32, h = 36, gap = 18;

            Controls.Add(L(lblX, y + 8, "SMTP Host:"));
            host = new() { Left = inX, Top = y, Width = 560 }; Controls.Add(host);

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "Port:"));
            port = new() { Left = inX, Top = y, Width = 90 }; Controls.Add(port);

            starttls = new() { Left = inX + 120, Top = y + 4, Text = "STARTTLS", AutoSize = true };
            starttls.CheckedChanged += (_, _) => ToggleAuthFields(); Controls.Add(starttls);

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "Username:"));
            user = new() { Left = inX, Top = y, Width = 260 }; Controls.Add(user);

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "Password:"));
            pass = new() { Left = inX, Top = y, Width = 260, UseSystemPasswordChar = true }; Controls.Add(pass);

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "Relay Restrictions:"));
            allowAll    = new() { Left = inX,       Top = y + 6, Text = "Allow All",       AutoSize = true };
            allowListed = new() { Left = inX + 150, Top = y + 6, Text = "Allow Specified", AutoSize = true };
            allowAll.CheckedChanged    += (_, _) => ToggleIPField();
            allowListed.CheckedChanged += (_, _) => ToggleIPField();
            Controls.AddRange(new Control[] { allowAll, allowListed });

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "IP Allowed List:"));
            ips = new() { Left = inX, Top = y, Width = 610 }; Controls.Add(ips);
            ToolTip(ips, "Example:\n  192.168.1.0/24\n  10.0.0.5\n  2001:db8::/32");

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "Logging:"));
            enableLog = new() { Left = inX, Top = y + 4, Text = "Enable Logging", AutoSize = true }; Controls.Add(enableLog);

            Controls.Add(L(inX + 200, y + 8, "Days kept:"));
            keepDays = new() { Left = inX + 300, Top = y, Width = 120, Minimum = 1, Maximum = 365, Value = 30 };
            Controls.Add(keepDays);

            y += h + gap * 2;
            btnSave = new()  { Left = inX,        Top = y, Width = 200, Height = 45, Text = "Save" };
            btnClose = new() { Left = inX + 240,  Top = y, Width = 200, Height = 45, Text = "Close" };
            btnSave.Click  += SaveCfg;
            btnClose.Click += (_, _) => Close();
            Controls.AddRange(new Control[] { btnSave, btnClose });

            /* right-side info */
            Controls.Add(L(Width - 260, Height - 140, "GUI version: 1.4"));
            var link = new LinkLabel
            {
                Left = Width - 260, Top = Height - 110,
                Text = "Project site", AutoSize = true
            };
            link.Links.Add(0, link.Text.Length, "https://github.com/mkitchingh/Smtp-Relay");
            link.LinkClicked += (_, e) => System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo(e.Link.LinkData.ToString()) { UseShellExecute = true });
            Controls.Add(link);
        }

        /* Helper to set ToolTip text */
        void ToolTip(Control c, string text)
        {
            var tt = new ToolTip { AutomaticDelay = 200 };
            tt.SetToolTip(c, text);
        }

        void ToggleAuthFields() { user.Enabled = pass.Enabled = starttls.Checked; }
        void ToggleIPField()    { ips.Enabled  = allowListed.Checked; }

        void LoadCfg()
        {
            var c = Config.Load();
            host.Text = c.SmartHost;
            port.Text = c.SmartHostPort.ToString();
            user.Text = c.Username;
            pass.Text = c.Password;
            allowAll.Checked = c.AllowAllIPs;
            allowListed.Checked = !c.AllowAllIPs;
            ips.Text = string.Join(",", c.AllowedIPs);
            starttls.Checked = c.UseStartTls;
            enableLog.Checked = c.EnableLogging;
            keepDays.Value = c.RetentionDays;
            ToggleAuthFields(); ToggleIPField();
        }

        void SaveCfg(object? s, EventArgs? e)
        {
            var cfg = new Config
            {
                SmartHost     = host.Text.Trim(),
                SmartHostPort = int.TryParse(port.Text, out var p) ? p : 25,
                Username      = user.Text.Trim(),
                Password      = pass.Text,
                AllowAllIPs   = allowAll.Checked,
                AllowedIPs    = ips.Text.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(x => x.Trim()).ToList(),
                UseStartTls   = starttls.Checked,
                EnableLogging = enableLog.Checked,
                RetentionDays = (int)keepDays.Value
            };
            cfg.Save();
            MessageBox.Show("Settings saved.\nService will continue to run.");
        }
    }
}
