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
            // Load config & log startup
            var cfg = Config.Load();
            Log.Information("Relay mode: {Mode}",
                cfg.AllowAllIPs
                  ? "Allow ALL IPs"
                  : $"Allow {cfg.AllowedIPs.Count} range(s)");

            // Build SMTP server options
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTPRelay")
                .Port(25, enableSsl: false);

            if (!cfg.AllowAllIPs)
            {
                options = options.Restrictions(r =>
                    r.AllowIp(cfg.AllowedIPs.ToArray()));
            }

            // Create & run
            var server = new SmtpServer.SmtpServer(options.Build(), new MessageRelayStore(cfg));
            return server.StartAsync(stoppingToken);
        }
    }
}
