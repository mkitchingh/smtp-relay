using System;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // .NET 8 WinForms high-DPI and default styles
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
