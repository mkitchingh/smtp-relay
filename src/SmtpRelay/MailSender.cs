using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SmtpServer.Protocol;      // ← needed for IMessageTransaction

namespace SmtpRelay
{
    internal static class MailSender
    {
        public static async Task SendAsync(
            Config cfg,
            IMessageTransaction txn,
            ReadOnlySequence<byte> buffer,
            CancellationToken ct)
        {
            /* load MimeMessage from the raw buffer */
            var msg = await MimeMessage.LoadAsync(buffer.AsStream(), ct);

            using var client = new SmtpClient();

            await client.ConnectAsync(
                cfg.SmartHost,
                cfg.SmartHostPort,
                cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.None,
                ct);

            if (!string.IsNullOrWhiteSpace(cfg.Username))
                await client.AuthenticateAsync(cfg.Username, cfg.Password, ct);

            await client.SendAsync(msg, ct);
            await client.DisconnectAsync(true, ct);
        }
    }
}
