using System.ServiceProcess;

namespace SmtpRelay
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun = new ServiceBase[]
            {
                new RelayService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
