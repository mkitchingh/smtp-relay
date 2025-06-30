using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SmtpRelay
{
    internal static class MailSender
    {
        public static async Task SendAsync(
            Config cfg,
            ReadOnlySequence<byte> buffer,
            CancellationToken ct)
        {
            /* copy ReadOnlySequence → MemoryStream (C# 12-compatible) */
            using var ms = new MemoryStream();
            foreach (var segment in buffer)
            {
                ms.Write(segment.Span);      // sync write avoids C# 13 features
            }
            ms.Position = 0;

            var msg = await MimeMessage.LoadAsync(ms, ct);

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
