
using System.ServiceProcess;

namespace SmtpRelay
{
    internal static class Program
    {
        static void Main()
        {
            ServiceBase.Run(new RelayService());
        }
    }
}
