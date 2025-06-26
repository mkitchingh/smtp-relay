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
                Console.WriteLine("Run the installer to register the service.");
            }
            else
            {
                ServiceBase.Run(new RelayService());
            }
        }
    }
}
