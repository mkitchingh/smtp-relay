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

// Alias the server’s SmtpResponse so we never pull in MailKit’s
using ProtocolResponse = SmtpServer.Protocol.SmtpResponse;

namespace SmtpRelay
{
    public class MessageRelayStore : IMessageStore
    {
        private readonly Config  _cfg;
        private readonly ILogger _log;

        public MessageRelayStore(Config cfg, ILogger log)
        {
            _cfg = cfg;
            _log = log;
        }

        public async Task<ProtocolResponse> SaveAsync(
            ISessionContext       context,
            IMessageTransaction   transaction,
            ReadOnlySequence<byte> buffer,
            CancellationToken     cancellationToken)
        {
            try
            {
                // Convert the ReadOnlySequence<byte> into a MemoryStream without extra array allocations
                using var ms = new MemoryStream();
                if (buffer.IsSingleSegment)
                {
                    await ms.WriteAsync(buffer.First, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    foreach (var segment in buffer)
                        await ms.WriteAsync(segment, cancellationToken).ConfigureAwait(false);
                }

                ms.Position = 0;
                var message = await MimeMessage.LoadAsync(ms, cancellationToken).ConfigureAwait(false);

                // Relay via MailKit
                using var client = new SmtpClient();
                await client.ConnectAsync(
                        _cfg.SmartHost,
                        _cfg.SmartHostPort,
                        _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto,
                        cancellationToken
                    ).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(_cfg.Username))
                {
                    await client.AuthenticateAsync(
                        _cfg.Username!,
                        _cfg.Password!,
                        cancellationToken
                    ).ConfigureAwait(false);
                }

                await client.SendAsync(message, cancellationToken).ConfigureAwait(false);
                await client.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);

                return ProtocolResponse.Ok;
            }
            catch (Exception ex)
            {
                // log the remote endpoint (IP:port) that failed
                _log.Error(ex, "Relay failure from {RemoteEndPoint}", context.RemoteEndPoint);
                return ProtocolResponse.TransactionFailed;
            }
        }
    }
}
