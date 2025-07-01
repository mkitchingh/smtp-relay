using System;
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
                _cfg.AllowAllIPs
                    ? "Allow ALL IPs"
                    : $"Allow {_cfg.AllowedIPs.Count} range(s)");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                .Endpoint(e => e
                    .Port(25)                    // listen port
                    .AllowUnsecureAuthentication(_cfg.UseStartTls))
                .Build();

            var serviceProvider = new SmtpServer.ComponentModel.ServiceProvider();
            serviceProvider.Add(new MessageRelayStore(_cfg, _log));

            var server = new SmtpServer.SmtpServer(options, serviceProvider);
            return server.StartAsync(stoppingToken);
        }
    }
}
