using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmtpServer;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using SmtpServer.ComponentModel;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        private readonly Config _cfg = Config.Load();

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information("Starting SMTP Relay Service");

            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                .MessageStore(new MessageRelayStore(_cfg))
                .Endpoint(ep => ep
                    .Port(_cfg.UseStartTls ? _cfg.SmartHostPort : 25)
                    .AllowUnsecureAuthentication(!_cfg.UseStartTls))
                .Build();

            var serviceProvider = new ServiceProviderBuilder()
                .UseSessionContextFactory<DefaultSessionContextFactory>()
                .BuildServiceProvider();

            var server = new SmtpServer.SmtpServer(options, serviceProvider);
            return server.StartAsync(stoppingToken);
        }
    }
}
