using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Serilog;
using SmtpServer.Protocol;
using SmtpServer.Storage;

namespace SmtpRelay
{
    public class MessageRelayStore : MessageStore
    {
        readonly Config _cfg;

        public MessageRelayStore(Config cfg) => _cfg = cfg;

        public override async Task<SmtpResponse> SaveAsync(
            ISessionContext     context,
            IMessageTransaction transaction,
            CancellationToken   cancellationToken)
        {
            // figure out remote IP
            var remote = context.RemoteEndPoint?.Address ?? IPAddress.None;
            Log.Information("Incoming relay request from {Remote}", remote);

            if (!_cfg.IsAllowed(remote))
            {
                Log.Warning("DENIED {Remote} — not in allow-list", remote);
                return new SmtpResponse(SmtpReplyCode.RelayDenied, "Relaying Denied");
            }

            try
            {
                // parse the message from the transaction stream
                var message = MimeMessage.Load(transaction.Message.Content);

                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _cfg.SmartHost,
                    _cfg.SmartHostPort,
                    _cfg.UseStartTls
                        ? SecureSocketOptions.StartTls
                        : SecureSocketOptions.Auto,
                    cancellationToken);

                if (!string.IsNullOrWhiteSpace(_cfg.Username))
                {
                    await client.AuthenticateAsync(
                        _cfg.Username,
                        _cfg.Password!,
                        cancellationToken);
                }

                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                return SmtpResponse.Ok;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Relay failure from {Remote}", remote);
                return SmtpResponse.TransactionFailed;
            }
        }
    }
}
