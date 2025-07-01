using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SmtpRelay.GUI
{
    internal static class Program
    {
        /// <summary>
        /// Exposes the GUI assembly version (matches Installed Apps).
        /// </summary>
        public static string AppVersion =>
            Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";

        /// <summary>
        /// Returns the folder where the service writes logs.
        /// </summary>
        public static string GetServiceLogDirectory()
        {
            var baseDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service", "logs");
            return baseDir;
        }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
