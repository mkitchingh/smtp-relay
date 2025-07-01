using System;
using System.Drawing;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Your existing save logic...
            UpdateServiceStatus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
