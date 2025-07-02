using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmtpServer;
using SmtpServer.Storage;
using IPAddressRange;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var cfg = Config.Load();
            Log.Information("Relay mode: {Mode}",
                cfg.AllowAllIPs
                  ? "Allow ALL IPs"
                  : $"Allow {cfg.AllowedIPs.Count} range(s)");

            var builder = new SmtpServerOptionsBuilder()
                .ServerName("SMTPRelay")
                .Port(25, enableSsl: false);

            if (!cfg.AllowAllIPs)
            {
                builder = builder.Restrictions(r => 
                    r.AllowIp(cfg.AllowedIPs.ToArray()));
            }

            var server = new SmtpServer.SmtpServer(builder.Build(), new MessageRelayStore(cfg));
            return server.StartAsync(stoppingToken);
        }
    }
}
