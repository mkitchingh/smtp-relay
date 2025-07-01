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
            ISessionContext ctx,
            IMessageTransaction tx,
            ReadOnlySequence<byte> buffer,
            CancellationToken ct)
        {
            try
            {
                // load the MIME message from the raw buffer
                var data = buffer.ToArray();
                var message = MimeMessage.Load(new MemoryStream(data));

                // relay via MailKit
                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _cfg.SmartHost,
                    _cfg.SmartHostPort,
                    _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto,
                    ct);

                if (!string.IsNullOrEmpty(_cfg.Username))
                    await client.AuthenticateAsync(_cfg.Username, _cfg.Password, ct);

                await client.SendAsync(message, ct);
                await client.DisconnectAsync(true, ct);

                return SmtpResponse.Ok;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Relay failure from {Remote}", ctx.RemoteEndPoint);
                return SmtpResponse.TransactionFailed;
            }
        }
    }
}
