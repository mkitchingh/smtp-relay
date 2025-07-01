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

        public Worker(Config cfg) => _cfg = cfg;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information("Starting SMTP Relay Service");

            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                .Port(25, !_cfg.UseStartTls)          // incoming
                .Port(25,  _cfg.UseStartTls)          // same port, different SecureSocketOptions
                .Build();

            var services = new ServiceProviderBuilder()
                .AddMessageStore(() =>
                    new MessageRelayStore(_cfg, Log.Logger))
                .Build();

            var server = new SmtpServer.SmtpServer(options, services);

            await server.StartAsync(stoppingToken);
        }
    }
}
