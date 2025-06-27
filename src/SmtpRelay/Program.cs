using System.ServiceProcess;

namespace SmtpRelay
{
    static class Program
    {
        static void Main()
        {
            ServiceBase.Run(new RelayService());
        }
    }
}