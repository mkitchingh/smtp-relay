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

namespace SmtpRelay
{
    // alias the SmtpServer protocol SmtpResponse so it doesn't collide with MailKit's
    using ProtocolSmtpResponse = SmtpServer.Protocol.SmtpResponse;

    public class MessageRelayStore : IMessageStore
    {
        readonly Config        _cfg;
        readonly ILogger       _log;  // Serilog.ILogger

        public MessageRelayStore(Config cfg, ILogger log)
        {
            _cfg = cfg;
            _log = log;
        }

        public async Task<ProtocolSmtpResponse> SaveAsync(
            ISessionContext      context,
            IMessageTransaction  transaction,
            ReadOnlySequence<byte> buffer,
            CancellationToken    cancellationToken)
        {
            try
            {
                var data    = buffer.ToArray();
                var message = MimeMessage.Load(new MemoryStream(data));

                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _cfg.SmartHost,
                    _cfg.SmartHostPort,
                    _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto,
                    cancellationToken);

                if (!string.IsNullOrEmpty(_cfg.Username))
                {
                    await client.AuthenticateAsync(
                        _cfg.Username, _cfg.Password!, cancellationToken);
                }

                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                return ProtocolSmtpResponse.Ok;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Relay failure from {Remote}", context.RemoteEndPoint);
                return ProtocolSmtpResponse.TransactionFailed;
            }
        }
    }
}
