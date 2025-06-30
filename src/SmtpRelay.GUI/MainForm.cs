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
        Button btnSave, btnClose, btnViewLogs;

        public MainForm() { InitializeComponent(); LoadCfg(); }

        Label L(int x,int y,string t)=>new(){Left=x,Top=y,Text=t,AutoSize=true};

        void InitializeComponent()
        {
            Controls.Clear();
            Width=960; Height=580; Text="SMTP Relay Config";
            Icon=System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            int lblX=32,inX=230,y=32,h=36,gap=18;

            Controls.Add(L(lblX,y+8,"SMTP Host:"));
            host=new(){Left=inX,Top=y,Width=560};Controls.Add(host);

            y+=h+gap;
            Controls.Add(L(lblX,y+8,"Port:"));
            port=new(){Left=inX,Top=y,Width=90};Controls.Add(port);
            starttls=new(){Left=inX+120,Top=y+4,Text="STARTTLS",AutoSize=true};
            starttls.CheckedChanged+=(_,_)=>ToggleAuth();Controls.Add(starttls);

            y+=h+gap;
            Controls.Add(L(lblX,y+8,"Username:"));
            user=new(){Left=inX,Top=y,Width=260};Controls.Add(user);

            y+=h+gap;
            Controls.Add(L(lblX,y+8,"Password:"));
            pass=new(){Left=inX,Top=y,Width=260,UseSystemPasswordChar=true};Controls.Add(pass);

            y+=h+gap;
            Controls.Add(L(lblX,y+8,"Relay Restrictions:"));
            allowAll=new(){Left=inX,Top=y+6,Text="Allow All",AutoSize=true};
            allowListed=new(){Left=inX+150,Top=y+6,Text="Allow Specified",AutoSize=true};
            allowAll.CheckedChanged+=(_,_)=>ToggleIP();allowListed.CheckedChanged+=(_,_)=>ToggleIP();
            Controls.AddRange(new Control[]{allowAll,allowListed});

            y+=h+gap;
            Controls.Add(L(lblX,y+8,"IP Allowed List:"));
            ips=new(){Left=inX,Top=y,Width=610};Controls.Add(ips);

            Controls.Add(L(inX,y+28,"Example: 192.168.1.0/24, 10.0.0.5, 2001:db8::/32"));

            y+=h+gap+22;
            Controls.Add(L(lblX,y+8,"Logging:"));
            enableLog=new(){Left=inX,Top=y+4,Text="Enable Logging",AutoSize=true};Controls.Add(enableLog);

            Controls.Add(L(inX+200,y+8,"Days kept:"));
            keepDays=new(){Left=inX+300,Top=y,Width=120,Minimum=1,Maximum=365,Value=30};Controls.Add(keepDays);

            /* bottom buttons */
            y+=h+gap*2;
            btnSave=new(){Left=inX,Top=y,Width=180,Height=45,Text="Save"};
            btnClose=new(){Left=inX+200,Top=y,Width=180,Height=45,Text="Close"};
            btnViewLogs=new(){Left=inX+400,Top=y,Width=180,Height=45,Text="View Logs"};
            btnSave.Click+=SaveCfg;
            btnClose.Click+=(_,_)=>Close();
            btnViewLogs.Click+=(_,_)=>OpenLogFolder();
            Controls.AddRange(new Control[]{btnSave,btnClose,btnViewLogs});

            Controls.Add(L(inX, y+55, "Service will remain running"));

            /* right-side info */
            Controls.Add(L(Width-260,Height-140,"Version: 1.4"));
            var link=new LinkLabel{Left=Width-260,Top=Height-110,Text="Project site",AutoSize=true};
            link.Links.Add(0,link.Text.Length,"https://github.com/mkitchingh/Smtp-Relay");
            link.LinkClicked+=(_,e)=>System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo(e.Link.LinkData.ToString()){UseShellExecute=true});
            Controls.Add(link);
        }

        void ToggleAuth(){user.Enabled=pass.Enabled=starttls.Checked;}
        void ToggleIP(){ips.Enabled=allowListed.Checked;}

        void LoadCfg()
        {
            var c=Config.Load();
            host.Text=c.SmartHost; port.Text=c.SmartHostPort.ToString();
            user.Text=c.Username;  pass.Text=c.Password;
            allowAll.Checked=c.AllowAllIPs; allowListed.Checked=!c.AllowAllIPs;
            ips.Text=string.Join(",",c.AllowedIPs);
            starttls.Checked=c.UseStartTls; enableLog.Checked=c.EnableLogging;
            keepDays.Value=c.RetentionDays; ToggleAuth(); ToggleIP();
        }

        void SaveCfg(object?_,EventArgs?__)
        {
            var cfg=new Config{
                SmartHost=host.Text.Trim(),
                SmartHostPort=int.TryParse(port.Text,out var p)?p:25,
                Username=user.Text.Trim(),Password=pass.Text,
                AllowAllIPs=allowAll.Checked,
                AllowedIPs=ips.Text.Split(new[]{',',';'},StringSplitOptions.RemoveEmptyEntries)
                                   .Select(s=>s.Trim()).ToList(),
                UseStartTls=starttls.Checked,
                EnableLogging=enableLog.Checked,
                RetentionDays=(int)keepDays.Value};
            cfg.Save(); RestartSvc();
            MessageBox.Show("Settings saved and service restarted.");
        }

        void RestartSvc()
        {
            try{
                using var sc=new ServiceController("SMTPRelayService");
                if(sc.Status!=ServiceControllerStatus.Stopped){
                    sc.Stop(); sc.WaitForStatus(ServiceControllerStatus.Stopped,TimeSpan.FromSeconds(10));
                }
                sc.Start();
            }catch(Exception ex){MessageBox.Show(ex.Message);}
        }

        void OpenLogFolder()
        {
            string logDir=System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"logs");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer",logDir){UseShellExecute=true});
        }
    }
}
