// src/SmtpRelay/Worker.cs
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmtpServer;
using SmtpServer.Storage;
using NetTools;                // for IPAddressRange

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        private readonly Config _cfg;
        private readonly ILogger _log;

        public Worker()
        {
            // Load your config.json
            _cfg = Config.Load();
            // Grab a Serilog logger for this type
            _log = Log.ForContext<Worker>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Log the relay mode
            _log.Information(
                "Relay mode: {Mode}",
                _cfg.AllowAllIPs
                    ? "Allow ALL IPs"
                    : $"Allow {_cfg.AllowedIPs.Count} IP range(s)");

            // Build the SMTP‐inbound server options:
            // • ServerName = “SMTP Relay”
// • Port 25, no SSL/TLS on the inbound side
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                .Port(25, allowUnsecure: true);

            // If not AllowAll, plug in each CIDR/IP
            if (!_cfg.AllowAllIPs)
            {
                foreach (var entry in _cfg.AllowedIPs)
                {
                    options.AllowedClientAddresses.Add(
                        IPAddressRange.Parse(entry));
                }
            }

            // Wire up your MessageRelayStore (it uses MailSender internally)
            var services = new ServiceProviderBuilder()
                .AddMessageStore(sp => new MessageRelayStore(_cfg, _log))
                .Build();

            // Spin up the server
            var server = new SmtpServer.SmtpServer(options.Build(), services);

            // This will run until the service is stopped
            await server.StartAsync(stoppingToken);
        }
    }
}
