using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTools;                         // IPAddressRange

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
                : _cfg.AllowedIPs.Select(IPAddressRange.Parse).ToArray();
        }

        protected override async Task ExecuteAsync(CancellationToken stop)
        {
            var listener = new TcpListener(IPAddress.Any, 25);
            listener.Start();
            _log.LogInformation("SMTP listener on port 25");

            while (!stop.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(stop);
                _ = Task.Run(() => HandleClient(client), stop);
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            var ip = (client.Client.RemoteEndPoint as IPEndPoint)?.Address;
            var allow = _cfg.AllowAllIPs
                         || (_ranges.Length == 0)          // fallback: empty list means allow
                         || _ranges.Any(r => r.Contains(ip));

            using var stream = client.GetStream();
            using var writer = new StreamWriter(stream) { AutoFlush = true };

            await writer.WriteLineAsync("220 SMTP Relay Ready");

            if (!allow)
            {
                _log.LogWarning("Rejected {ip}", ip);
                await writer.WriteLineAsync("554 Relay access denied");
                client.Close();
                return;
            }

            _log.LogInformation("Accepted {ip}", ip);
            // TODO: continue SMTP conversation
        }
    }
}
