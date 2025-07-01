using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmtpServer;
using SmtpServer.Protocol;
using SmtpServer.Storage;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker>       _log;
        private readonly MessageRelayStore     _store;
        private readonly Config                _cfg;

        public Worker(
            ILogger<Worker> log,
            Config cfg,
            MessageRelayStore store)
        {
            _log   = log;
            _cfg   = cfg;
            _store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Build SMTP server options
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                .Port(25, false)
                .MessageStore(_store)
                .Build();

            var builder = new ServiceProviderBuilder();
            builder.UseLoggingProvider(_log);

            var server = new SmtpServer.SmtpServer(options, builder.BuildServiceProvider());

            _log.LogInformation("Relay mode: {Mode}",
                _cfg.AllowAllIPs
                    ? "Allow ALL IPs"
                    : $"Allow {_cfg.AllowedIPs.Count} range(s)");
            _log.LogInformation("Application started");

            await server.StartAsync(stoppingToken);
        }
    }
}
