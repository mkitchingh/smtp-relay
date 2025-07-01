// File: src/SmtpRelay.GUI/Program.cs

using System;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                // Classic WinForms init (works in net8.0 and earlier)
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Run the main form
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                // If something fails before the form appears, show it
                MessageBox.Show(
                    $"Failed to launch GUI:\n\n{ex}",
                    "Startup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Environment.Exit(1);
            }
        }
    }
}
