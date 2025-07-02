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

// alias the SMTP-server response so it doesn't conflict with MailKit
using ProtocolResponse = SmtpServer.Protocol.SmtpResponse;

namespace SmtpRelay
{
    public class MessageRelayStore : IMessageStore
    {
        readonly Config  _cfg;
        readonly ILogger _log; // Serilog.ILogger

        public MessageRelayStore(Config cfg, ILogger log)
        {
            _cfg = cfg;
            _log = log;
        }

        public async Task<ProtocolResponse> SaveAsync(
            ISessionContext     context,
            IMessageTransaction _,
            ReadOnlySequence<byte> buffer,
            CancellationToken   cancellationToken)
        {
            try
            {
                // parse the incoming message
                var data    = buffer.ToArray();
                var message = MimeMessage.Load(new MemoryStream(data));

                // relay via MailKit
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

                return ProtocolResponse.Ok;
            }
            catch (Exception ex)
            {
                _log.Error(
                    ex,
                    "Relay failure from {Remote}",
                    context.RemoteEndPoint);
                return ProtocolResponse.TransactionFailed;
            }
        }
    }
}
