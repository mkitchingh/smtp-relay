using System;
using System.IO;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Serilog;
using SmtpServer.Protocol;
using SmtpServer.Storage;

namespace SmtpRelay
{
    public class MessageRelayStore : IMessageStore
    {
        private readonly Config _cfg;

        public MessageRelayStore(Config cfg)
        {
            _cfg = cfg;
        }

        public async Task<SmtpResponse> SaveAsync(
            ISessionContext      context,
            IMessageTransaction  transaction,
            ReadOnlySequence<byte> buffer,
            CancellationToken    cancellationToken)
        {
            try
            {
                // Load incoming message
                var data = buffer.ToArray();
                var message = MimeMessage.Load(new MemoryStream(data));

                // Relay via MailKit
                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _cfg.SmartHost,
                    _cfg.SmartHostPort,
                    _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto,
                    cancellationToken);

                if (!string.IsNullOrEmpty(_cfg.Username))
                    await client.AuthenticateAsync(_cfg.Username, _cfg.Password, cancellationToken);

                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                return SmtpResponse.Ok;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Relay failure from {Remote}", context.RemoteEndPoint);
                return SmtpResponse.TransactionFailed;
            }
        }
    }
}
