using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Serilog;

namespace SmtpRelay
{
    public class MailSender
    {
        private readonly Config _cfg;
        private readonly ILogger _log;

        public MailSender(Config cfg, ILogger log)
        {
            _cfg = cfg;
            _log = log;
        }

        /// <summary>
        /// Relay the raw SMTP message to the smart host.
        /// </summary>
        public async Task SendAsync(Config cfg, ReadOnlySequence<byte> buffer, CancellationToken ct)
        {
            try
            {
                // deserialize incoming message
                var data = buffer.ToArray();
                var message = MimeMessage.Load(new MemoryStream(data));

                // only one client, no extra smtp-*.log file
                using var client = new SmtpClient();

                _log.Information(
                    "Connecting to {Host}:{Port} (STARTTLS={Tls})",
                    cfg.SmartHost, cfg.SmartHostPort, cfg.UseStartTls);

                await client.ConnectAsync(
                    cfg.SmartHost,
                    cfg.SmartHostPort,
                    cfg.UseStartTls
                        ? SecureSocketOptions.StartTls
                        : SecureSocketOptions.Auto,
                    ct);

                if (!string.IsNullOrEmpty(cfg.Username))
                    await client.AuthenticateAsync(
                        cfg.Username!,
                        cfg.Password!,
                        ct);

                await client.SendAsync(message, ct);
                await client.DisconnectAsync(true, ct);

                _log.Information("Message relayed successfully");
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Relay failure from smart host");
                throw;
            }
        }
    }
}
