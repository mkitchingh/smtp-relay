using System;
using System.IO;
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
        readonly ILogger       _log;
        readonly Config        _cfg;

        public Worker(ILogger log)
        {
            _log = log;
            _cfg = Config.Load();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Build shared paths
            var baseDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service");
            var logDir = Path.Combine(baseDir, "logs");
            Directory.CreateDirectory(logDir);

            // Log startup mode
            _log.Information("Relay mode: {Mode}",
                _cfg.AllowAllIPs
                    ? "Allow ALL IPs"
                    : $"Allow { _cfg.AllowedIPs.Count } range(s)");
            _log.Information("Application started. Content root: {Root}", baseDir);

            // Build SMTP server options
            var options = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay Service")
                .Endpoint(builder => builder
                    .Port(_cfg.ListenPort, Protocol.SmtpServerAuthentication.None))
                // Inbound store hands off to your MessageRelayStore
                .MessageStore(new MessageRelayStore(_cfg, _log))
                // **ONLY** this protocol logger, writing smtp-proto-YYYYMMDD.log
                .ProtocolLogger(new ProtocolLogger(
                    Path.Combine(logDir, $"smtp-proto-{DateTime.Now:yyyyMMdd}.log")))
                .Build();

            // Start the server
            var smtpServer = new SmtpServer.SmtpServer(options);
            await smtpServer.StartAsync(stoppingToken);
        }
    }
}
