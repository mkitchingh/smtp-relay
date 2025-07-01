using System;
using System.Buffers;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
        private readonly Config  _cfg;
        private readonly ILogger _log;

        public MessageRelayStore(Config cfg, ILogger log)
        {
            _cfg = cfg;
            _log = log;
        }

        public async Task<SmtpResponse> SaveAsync(
            ISessionContext       context,
            IMessageTransaction   transaction,
            ReadOnlySequence<byte> buffer,
            CancellationToken     cancellationToken)
        {
            // Determine client endpoint (SmtpServer 11.x moved it to NetworkClient.RemoteEndPoint)
            var ep = context.RemoteEndPoint as IPEndPoint
                     ?? (context as dynamic).NetworkClient?.RemoteEndPoint as IPEndPoint;
            if (ep is null)
            {
                _log.Warning("Could not determine remote endpoint—defaulting to 0.0.0.0");
                ep = new IPEndPoint(IPAddress.Any, 0);
            }
            var remoteAddress = ep.Address;
            _log.Information("Incoming relay request from {Address}", remoteAddress);

            try
            {
                // Load the incoming message
                var data = buffer.ToArray();
                var message = MimeMessage.Load(new MemoryStream(data));

                // Connect to smarthost
                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _cfg.SmartHost,
                    _cfg.SmartHostPort,
                    _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto,
                    cancellationToken);

                // Authenticate if configured
                if (!string.IsNullOrEmpty(_cfg.Username))
                {
                    await client.AuthenticateAsync(
                        _cfg.Username,
                        _cfg.Password,
                        cancellationToken);
                }

                // Relay the message
                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                return SmtpResponse.Ok;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Relay failure from {Address}", remoteAddress);
                return SmtpResponse.TransactionFailed;
            }
        }
    }
}
