using System.ServiceProcess;
using System.Diagnostics;

namespace SmtpRelay
{
    public class RelayService : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("SmtpRelay", "Service started.");
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("SmtpRelay", "Service stopped.");
        }
    }
}
