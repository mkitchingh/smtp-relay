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

        public Worker()
        {
            _cfg = Config.Load();
            _log = Log.Logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // configure inbound SMTP server
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay Service")
                .Endpoint(builder => builder
                    .Port(25)
                    .AllowUnsecureAuthentication()
                    .Build())
                .Build();

            // plug in our relay store
            var serviceProvider = new ServiceProviderBuilder()
                .MessageStore(new MessageRelayStore(_cfg, _log))
                .Build();

            var smtpServer = new SmtpServer.SmtpServer(options, serviceProvider);

            _log.Information(
                "Application started. Content root path: {Path}", AppContext.BaseDirectory);

            return smtpServer.StartAsync(stoppingToken);
        }
    }
}
