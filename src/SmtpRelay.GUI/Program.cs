using System;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // This wires up high-DPI, visual styles, etc.
            ApplicationConfiguration.Initialize();

            // Launch our MainForm
            Application.Run(new MainForm());
        }
    }
}
