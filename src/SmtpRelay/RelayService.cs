using System;
using System.IO;
using System.ServiceProcess;

namespace SmtpRelay
{
    public class RelayService : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            File.AppendAllText("service.log", $"Service started at {DateTime.Now}\n");
        }

        protected override void OnStop()
        {
            File.AppendAllText("service.log", $"Service stopped at {DateTime.Now}\n");
        }
    }
}
