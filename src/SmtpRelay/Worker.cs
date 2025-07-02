using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmtpServer;
using SmtpServer.Storage;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        private readonly Config _cfg;
        private readonly ILogger _log;
        private readonly MessageRelayStore _store;

        public Worker()
        {
            _cfg   = Config.Load();
            _log   = Log.Logger;
            _store = new MessageRelayStore(_cfg, _log);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _log.Information("Relay mode: {Mode}", 
                _cfg.AllowAllIPs 
                    ? "Allow ALL IPs" 
                    : $"Allow {_cfg.AllowedIPs.Count} range(s)");

            // configure and start the inbound SMTP server
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                // default listen on port 25
                .Port(25)
                .MessageStore(_store)
                .Build();

            var server = new SmtpServer.SmtpServer(options);
            await server.StartAsync(stoppingToken);
        }
    }
}
