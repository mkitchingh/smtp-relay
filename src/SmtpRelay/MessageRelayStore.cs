using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Serilog;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using SmtpResponse = SmtpServer.Protocol.SmtpResponse;
using ILogger = Serilog.ILogger;

namespace SmtpRelay
{
    public class MessageRelayStore : IMessageStore
    {
        private readonly Config _cfg;
        private readonly ILogger _log;

        public MessageRelayStore(Config cfg, ILogger log)
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
            try
            {
                // Parse incoming message
                var data = buffer.ToArray();
                var mime = MimeMessage.Load(new MemoryStream(data));

                // Relay
                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _cfg.SmartHost,
                    _cfg.SmartHostPort,
                    _cfg.UseStartTls
                        ? SecureSocketOptions.StartTls
                        : SecureSocketOptions.Auto,
                    cancellationToken);

                if (!string.IsNullOrEmpty(_cfg.Username))
                {
                    await client.AuthenticateAsync(
                        _cfg.Username,
                        _cfg.Password ?? "",
                        cancellationToken);
                }

                await client.SendAsync(mime, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                return SmtpResponse.Ok;
            }
            catch (Exception ex)
            {
                var remote = context.RemoteEndPoint?.ToString() ?? "Unknown";
                _log.Error(ex, "Relay failure from {Remote}", remote);
                return SmtpResponse.TransactionFailed;
            }
        }
    }
}
