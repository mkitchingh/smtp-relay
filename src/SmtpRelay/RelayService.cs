using System.ServiceProcess;

namespace SmtpRelay
{
    public class RelayService : ServiceBase
    {
        public RelayService()
        {
            ServiceName = "SmtpRelay";
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add SMTP relay logic here
        }

        protected override void OnStop()
        {
            // TODO: Cleanup logic here
        }
    }
}