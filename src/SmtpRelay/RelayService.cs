using System.ServiceProcess;

namespace SmtpRelay
{
    public class RelayService : ServiceBase
    {
        public RelayService()
        {
            ServiceName = "SMTPRelayService";
        }

        protected override void OnStart(string[] args)
        {
            // Service logic here
        }

        protected override void OnStop()
        {
            // Cleanup logic
        }
    }
}