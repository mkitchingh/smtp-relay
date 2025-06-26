using System;
using System.ServiceProcess;
using System.IO;

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
            Directory.CreateDirectory("C:/ProgramData/SmtpRelay");
            File.AppendAllText("C:/ProgramData/SmtpRelay/log.txt", $"Service started at {DateTime.Now}\n");
        }

        protected override void OnStop()
        {
            File.AppendAllText("C:/ProgramData/SmtpRelay/log.txt", $"Service stopped at {DateTime.Now}\n");
        }
    }
}
