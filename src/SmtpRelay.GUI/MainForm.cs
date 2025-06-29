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
        Button btnSave, btnRestart;

        public MainForm()
        {
            InitializeComponent();
            LoadCfg();
        }

        Label L(int x, int y, string t) => new Label { Left = x, Top = y, Text = t, AutoSize = true };

        void InitializeComponent()
        {
            Controls.Clear();

            Width = 900;
            Height = 540;
            Text = "SMTP Relay Config";
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            int lblX = 32, inX = 200, y = 32, h = 36, gap = 18;

            Controls.Add(L(lblX, y + 8, "SMTP Host:"));
            host = new TextBox { Left = inX, Top = y, Width = 540 }; Controls.Add(host);

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "Port:"));
            port = new TextBox { Left = inX, Top = y, Width = 90 }; Controls.Add(port);
            starttls = new CheckBox { Left = inX + 120, Top = y + 4, Text = "STARTTLS", AutoSize = true };
            Controls.Add(starttls);

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "Username:"));
            user = new TextBox { Left = inX, Top = y, Width = 300 }; Controls.Add(user);

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "Password:"));
            pass = new TextBox { Left = inX, Top = y, Width = 300, UseSystemPasswordChar = true };
            Controls.Add(pass);

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "Relay Restrictions:"));
            allowAll    = new RadioButton { Left = inX,       Top = y + 6, Text = "Allow All",       AutoSize = true };
            allowListed = new RadioButton { Left = inX + 150, Top = y + 6, Text = "Allow Specified", AutoSize = true };
            Controls.AddRange(new Control[] { allowAll, allowListed });

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "IP Allowed List:"));
            ips = new TextBox { Left = inX, Top = y, Width = 600 }; Controls.Add(ips);

            y += h + gap;
            Controls.Add(L(lblX, y + 8, "Logging:"));
            enableLog = new CheckBox { Left = inX, Top = y + 4, Text = "Enable Logging", AutoSize = true };
            Controls.Add(enableLog);

            Controls.Add(L(inX + 200, y + 8, "Days kept:"));
            keepDays = new NumericUpDown
            {
                Left = inX + 320,   // shifted right to clear label
                Top  = y,
                Width = 120,
                Minimum = 1,
                Maximum = 365,
                Value = 30
            };
            Controls.Add(keepDays);

            y += h + gap * 2;
            btnSave    = new Button { Left = inX,         Top = y, Width = 200, Height = 45, Text = "Save" };
            btnRestart = new Button { Left = inX + 240,   Top = y, Width = 200, Height = 45, Text = "Restart" };
            Controls.AddRange(new Control[] { btnSave, btnRestart });

            btnSave.Click    += SaveCfg;
            btnRestart.Click += (s, e) => { if (RestartSvc()) MessageBox.Show("Service restarted."); };

            Controls.Add(L(Width - 140, Height - 80, "GUI v1.3"));
        }

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
                AllowedIPs    = ips.Text.Split(new[]{',',';'}, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s => s.Trim()).ToList(),
                UseStartTls   = starttls.Checked,
                EnableLogging = enableLog.Checked,
                RetentionDays = (int)keepDays.Value
            };
            cfg.Save();
            if (RestartSvc())
                MessageBox.Show("Settings saved and service restarted.");
        }

        bool RestartSvc()
        {
            try
            {
                using var sc = new ServiceController("SMTPRelayService");
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }
                sc.Start();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to restart service:\n" + ex.Message);
                return false;
            }
        }
    }
}
