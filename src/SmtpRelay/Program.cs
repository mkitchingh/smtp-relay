using System;
using System.ServiceProcess;
using System.Configuration.Install;

namespace SmtpRelay
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                if (args.Length > 0 && args[0] == "--install")
                {
                    ManagedInstallerClass.InstallHelper(new[] { System.Reflection.Assembly.GetExecutingAssembly().Location });
                }
                else if (args.Length > 0 && args[0] == "--uninstall")
                {
                    ManagedInstallerClass.InstallHelper(new[] { "/u", System.Reflection.Assembly.GetExecutingAssembly().Location });
                }
                else
                {
                    Console.WriteLine("Running in console mode.");
                }
            }
            else
            {
                ServiceBase.Run(new RelayService());
            }
        }
    }
}
