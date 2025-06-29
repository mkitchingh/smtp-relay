using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTools;                 // ← correct namespace for IPAddressRange
using System.Collections.Generic;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _log;
        private readonly Config _cfg;
        private readonly IReadOnlyList<IPAddressRange> _ranges;

        public Worker(ILogger<Worker> log)
        {
            _log = log;
            _cfg = Config.Load();

            _ranges = _cfg.AllowAllIPs
                ? Array.Empty<IPAddressRange>()
                : _cfg.AllowedIPs
                      .Select(IPAddressRange.Parse)   // IPv4 & IPv6 CIDR / single IP
                      .ToList();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var listener = new TcpListener(IPAddress.Any, 25);
            listener.Start();
            _log.LogInformation("SMTP listener started on port 25");

            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(stoppingToken);
                _ = Task.Run(() => HandleClient(client), stoppingToken);
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            var remoteIP = (client.Client.RemoteEndPoint as IPEndPoint)?.Address;
            var allowed  = _cfg.AllowAllIPs || _ranges.Any(r => r.Contains(remoteIP));

            using var stream = client.GetStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };

            // RFC 5321 greeting
            await writer.WriteLineAsync("220 SMTP Relay Ready");

            if (!allowed)
            {
                _log.LogWarning("Rejected {ip} (not in allow-list)", remoteIP);
                await writer.WriteLineAsync("554 Relay access denied");
                client.Close();
                return;
            }

            _log.LogInformation("Accepted {ip}", remoteIP);
            // TODO: handle SMTP session (omitted for brevity)
        }
    }
}
