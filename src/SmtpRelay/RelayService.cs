using System;
using System.ServiceProcess;
using System.Diagnostics;

namespace SmtpRelay
{
    public class RelayService : ServiceBase
    {
        public RelayService()
        {
            ServiceName = "SMTPRelay";
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("SMTP Relay Service Started.");
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("SMTP Relay Service Stopped.");
        }
    }
}