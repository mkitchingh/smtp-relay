using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IPAddressRange;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _log;
        private readonly Config _cfg;
        private readonly IPAddressRange[] _ranges;

        public Worker(ILogger<Worker> log)
        {
            _log = log;
            _cfg = Config.Load();

            _ranges = _cfg.AllowAllIPs
                ? Array.Empty<IPAddressRange>()
                : _cfg.AllowedIPs
                      .Select(IPAddressRange.Parse)   // supports IPv4 & IPv6 CIDR
                      .ToArray();
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
            var remote = (client.Client.RemoteEndPoint as IPEndPoint)?.Address;
            var allow  = _cfg.AllowAllIPs || _ranges.Any(r => r.Contains(remote));

            using var stream = client.GetStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };

            // always send greeting so RFC 5321 clients know why connection closes
            await writer.WriteLineAsync("220 SMTP Relay Ready");

            if (!allow)
            {
                _log.LogWarning("Rejected {ip} (not in allow-list)", remote);
                await writer.WriteLineAsync("554 Relay access denied");
                client.Close();
                return;
            }

            _log.LogInformation("Accepted {ip}", remote);
            // … continue handling SMTP commands (omitted for brevity) …
        }
    }
}
