using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SmtpServer;
using SmtpServer.Storage;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        private readonly Config _cfg;
        private readonly ILogger<Worker> _logger;

        public Worker(Config cfg, ILogger<Worker> logger)
        {
            _cfg    = cfg;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SMTP Relay Service starting…");

            // build the SMTP server options
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                // listen on port 25 unless STARTTLS==true then 587 (example)
                .Port(_cfg.UseStartTls ? 587 : 25)
                .Build();

            // wire up the custom message store
            var smtpServer = new SmtpServerBuilder()
                .Options(options)
                .MessageStore(new MessageRelayStore(_cfg, Log.Logger))
                .Build();

            // run until shutdown
            await smtpServer.StartAsync(stoppingToken);
        }
    }
}
