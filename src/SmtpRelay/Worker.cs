using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmtpServer;
using SmtpServer.Protocol;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Config _cfg;
        private readonly MessageRelayStore _store;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _cfg = Config.Load();
            _store = new MessageRelayStore(_cfg, logger);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay Service")
                .Endpoint(ep => ep
                    .Port(_cfg.SmartHostPort)
                    .AllowUnsecureAuthentication(false))
                .Build();

            var smtpServer = new SmtpServer.SmtpServer(options, _store);

            _logger.LogInformation("Starting SMTP server on port {Port}", _cfg.SmartHostPort);
            await smtpServer.StartAsync(stoppingToken);
        }
    }
}
