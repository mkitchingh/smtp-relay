using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SmtpServer;
using SmtpServer.Storage;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        private readonly ILogger       _log;
        private readonly MessageRelayStore _store;

        public Worker(ILogger log, Config cfg)
        {
            _log   = log;
            _store = new MessageRelayStore(cfg, log);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var cfg = Config.Load();

            // build SMTP server
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                .Port(25, false)
                .MessageStore(_store)
                .Build();

            var serviceProvider = new ServiceProviderBuilder()
                .UseSerilog(_log)
                .BuildServiceProvider();

            var server = new SmtpServer.SmtpServer(options, serviceProvider);

            _log.Information("Relay mode: {Mode}", cfg.AllowAllIPs ? "Allow ALL IPs" : $"Allow {cfg.AllowedIPs.Count} range(s)");
            _log.Information("Application started. Hosting environment: Production");

            await server.StartAsync(stoppingToken);
        }
    }
}
