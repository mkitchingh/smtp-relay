using System;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "SMTP Relay Config";
            this.Width = 400;
            this.Height = 300;
        }
    }
}