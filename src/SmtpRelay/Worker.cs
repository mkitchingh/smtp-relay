using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmtpServer;
using SmtpServer.ComponentModel;
using NetTools;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        private readonly Config _cfg;
        private readonly ILogger _log;

        public Worker()
        {
            _cfg = Config.Load();
            _log = Log.Logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _log.Information("Relay mode: {Mode}",
                _cfg.AllowAllIPs
                    ? "Allow ALL IPs"
                    : $"Allow {_cfg.AllowedIPs.Count} range(s)");

            // Build SMTP server options
            var optionsBuilder = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                .Port(_cfg.SmartHostPort, _cfg.UseStartTls);

            if (!_cfg.AllowAllIPs)
            {
                var ep = optionsBuilder.Endpoint();
                foreach (var r in _cfg.AllowedIPs)
                {
                    ep.Allow(r);
                }
            }

            // Wire up our store
            var serviceProvider = new SmtpServerComponentBuilder()
                .UseMessageStore<MessageRelayStore>()
                .BuildServiceProvider();

            var server = new SmtpServer.SmtpServer(
                optionsBuilder.Build(),
                serviceProvider);

            await server.StartAsync(stoppingToken);
        }
    }
}
