using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using Serilog;

namespace SmtpRelay
{
    public class MessageRelayStore : IMessageStore
    {
        readonly Config        _cfg;
        readonly ILogger       _log;

        public MessageRelayStore(Config cfg, ILogger log)
        {
            _cfg = cfg;
            _log = log;
        }

        public async Task<SmtpResponse> SaveAsync(
            ISessionContext      context,
            IMessageTransaction  transaction,
            ReadOnlySequence<byte> buffer,
            CancellationToken    cancellationToken)
        {
            try
            {
                // parse the incoming message
                var data = buffer.ToArray();
                var message = MimeMessage.Load(new MemoryStream(data));

                // relay
                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _cfg.SmartHost,
                    _cfg.SmartHostPort,
                    _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto,
                    cancellationToken);

                if (!string.IsNullOrEmpty(_cfg.Username))
                    await client.AuthenticateAsync(_cfg.Username!, _cfg.Password!, cancellationToken);

                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                return SmtpResponse.Ok;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Relay failure from {Remote}", context.RemoteEndPoint);
                return SmtpResponse.TransactionFailed;
            }
        }
    }
}
