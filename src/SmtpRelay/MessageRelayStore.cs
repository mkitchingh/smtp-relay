using System;
using System.Buffers;
using System.IO;
using System.Net;
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
        private readonly Config  _cfg;
        private readonly ILogger _log;

        public MessageRelayStore(Config cfg, ILogger log)
        {
            _cfg = cfg;
            _log = log;
        }

        public async Task<SmtpServer.Protocol.SmtpResponse> SaveAsync(
            ISessionContext        context,
            IMessageTransaction    transaction,
            ReadOnlySequence<byte> buffer,
            CancellationToken      cancellationToken)
        {
            // Get the real remote endpoint:
            var ep = context.RemoteEndPoint as IPEndPoint;
            if (ep == null && context is dynamic dyn && dyn.NetworkClient != null)
                ep = dyn.NetworkClient.RemoteEndPoint as IPEndPoint;
            var remoteAddr = ep?.Address ?? IPAddress.Any;
            _log.Information("Incoming relay request from {Address}", remoteAddr);

            try
            {
                // Load the incoming message bytes
                var data = buffer.ToArray();
                var message = MimeMessage.Load(new MemoryStream(data));

                // Connect & optionally STARTTLS
                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _cfg.SmartHost,
                    _cfg.SmartHostPort,
                    _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto,
                    cancellationToken);

                // Authenticate if needed
                if (!string.IsNullOrWhiteSpace(_cfg.Username))
                    await client.AuthenticateAsync(_cfg.Username!, _cfg.Password!, cancellationToken);

                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                return SmtpServer.Protocol.SmtpResponse.Ok;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Relay failure from {Address}", remoteAddr);
                return SmtpServer.Protocol.SmtpResponse.TransactionFailed;
            }
        }
    }
}
