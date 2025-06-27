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
            // TODO: Add start logic
        }

        protected override void OnStop()
        {
            // TODO: Add stop logic
        }
    }
}