using System.ServiceProcess;

namespace SmtpRelay
{
    internal static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new RelayService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}