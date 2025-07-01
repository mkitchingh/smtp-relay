using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmtpServer;
using SmtpServer.Protocol;
using SmtpServer.Storage;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        readonly Config  _cfg;
        readonly ILogger _log;

        public Worker(Config cfg, ILogger log)
        {
            _cfg = cfg;
            _log = log;
            _log.Information("Relay mode: {Mode}",
                cfg.AllowAllIPs ? "Allow ALL IPs" : $"Allow {cfg.AllowedIPs.Count} range(s)");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                .Port(25)
                .AllowUnsecureAuthentication(_cfg.UseStartTls)
                .Build();

            // set up DI for the message store
            var serviceProvider = new SmtpServer.ComponentModel.ServiceProvider();
            serviceProvider.Add(new MessageRelayStore(_cfg, _log));

            var server = new SmtpServer.SmtpServer(options, serviceProvider);
            return server.StartAsync(stoppingToken);
        }
    }
}
