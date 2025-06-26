
using System.ServiceProcess;

namespace SmtpRelay
{
    public class RelayService : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            // Load config and start relay logic
        }

        protected override void OnStop()
        {
            // Cleanup
        }
    }
}
