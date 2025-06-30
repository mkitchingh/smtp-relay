using System;
using System.Buffers;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTools;
using SmtpServer;
using SmtpServer.ComponentModel;
using SmtpServer.Protocol;
using SmtpServer.Storage;

namespace SmtpRelay
{
    public class Worker : BackgroundService
    {
        readonly ILogger<Worker> _log;
        readonly Config _cfg;
        readonly IPAddressRange[] _ranges;

        public Worker(ILogger<Worker> log)
        {
            _log = log;
            _cfg = Config.Load();
            _ranges = _cfg.AllowAllIPs
                ? Array.Empty<IPAddressRange>()
                : _cfg.AllowedIPs.Select(IPAddressRange.Parse).ToArray();
        }

        protected override Task ExecuteAsync(CancellationToken token)
        {
            var opts = new SmtpServerOptionsBuilder()
                .ServerName("SMTP Relay")
                .Port(25)
                .Build();

            var provider = new ServiceProvider();
            provider.Add(new RelayStore(_cfg, _ranges, _log));

            var server = new SmtpServer.SmtpServer(opts, provider);
            _log.LogInformation("SMTP Relay listening on port 25");
            return server.StartAsync(token);
        }

        /* ── Message store that enforces allow-list and forwards via MailKit ── */
        sealed class RelayStore : MessageStore
        {
            readonly Config _cfg;
            readonly IPAddressRange[] _ranges;
            readonly ILogger _log;
            public RelayStore(Config cfg, IPAddressRange[] ranges, ILogger log)
            { _cfg = cfg; _ranges = ranges; _log = log; }

            public override async Task<SmtpResponse> SaveAsync(
                ISessionContext ctx, IMessageTransaction txn,
                ReadOnlySequence<byte> data, CancellationToken ct)
            {
                /* remote IP from session properties */
                IPAddress? ip = null;
                if (ctx.Properties.TryGetValue("SessionRemoteEndPoint", out var epObj) &&
                    epObj is IPEndPoint iep) ip = iep.Address;

                bool allowed = _cfg.AllowAllIPs ||
                               _ranges.Length == 0 ||
                               (ip != null && _ranges.Any(r => r.Contains(ip)));

                if (!allowed)
                {
                    _log.LogWarning("DENIED {ip} — not in allow-list", ip);
                    return SmtpResponse.MailboxUnavailable;   // 550
                }

                try
                {
                    await MailSender.SendAsync(_cfg, txn, data, ct);
                    _log.LogInformation("Relayed mail from {ip}", ip);
                    return SmtpResponse.Ok;
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "Relay failure from {ip}", ip);
                    return SmtpResponse.TransactionFailed;
                }
            }
        }
    }
}
