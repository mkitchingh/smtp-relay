using System;
using System.ServiceProcess;

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
                    System.Configuration.Install.ManagedInstallerClass.InstallHelper(new[] { System.Reflection.Assembly.GetExecutingAssembly().Location });
                }
                else if (args.Length > 0 && args[0] == "--uninstall")
                {
                    System.Configuration.Install.ManagedInstallerClass.InstallHelper(new[] { "/u", System.Reflection.Assembly.GetExecutingAssembly().Location });
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
