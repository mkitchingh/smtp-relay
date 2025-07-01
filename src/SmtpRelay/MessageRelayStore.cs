using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using SmtpServer.Protocol;
using SmtpServer.Storage;

namespace SmtpRelay
{
    public class MessageRelayStore : IMessageStore
    {
        private readonly Config _cfg;
        private readonly ILogger<MessageRelayStore> _log;

        public MessageRelayStore(Config cfg, ILogger<MessageRelayStore> log)
        {
            _cfg = cfg;
            _log = log;
        }

        public async Task<SmtpResponse> SaveAsync(
            ISessionContext context,
            IMessageTransaction transaction,
            ReadOnlySequence<byte> buffer,
            CancellationToken cancellationToken)
        {
            var remote = context.RemoteEndPoint?.Address;
            if (remote == null || !_cfg.IsAllowed(remote))
            {
                _log.LogWarning("DENIED {Remote} — not in allow-list", remote);
                return SmtpResponse.MailboxUnavailable; // 550
            }

            try
            {
                // parse incoming
                var data = buffer.ToArray();
                var message = MimeMessage.Load(new MemoryStream(data));

                // connect to smarthost
                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _cfg.SmartHost,
                    _cfg.SmartHostPort,
                    _cfg.UseStartTls
                        ? SecureSocketOptions.StartTls
                        : SecureSocketOptions.Auto,
                    cancellationToken);

                if (!string.IsNullOrEmpty(_cfg.Username))
                    await client.AuthenticateAsync(
                        _cfg.Username!,
                        _cfg.Password!,
                        cancellationToken);

                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                _log.LogInformation(
                    "Relayed from {Remote} From:{From} To:{To}",
                    remote,
                    string.Join(',', message.From),
                    string.Join(',', message.To));

                return SmtpResponse.Ok;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Relay failure from {Remote}", context.RemoteEndPoint?.Address);
                return SmtpResponse.TransactionFailed; // 554
            }
        }
    }
}
