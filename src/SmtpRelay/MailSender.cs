using System;
using System.IO;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailKit; // for ProtocolLogger
using MimeKit;
using Serilog;

namespace SmtpRelay
{
    public static class MailSender
    {
        // Base directory for service (same as in Program.cs)
        private static readonly string BaseDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "SMTP Relay", "service");

        // Logs live under BaseDir\logs
        private static readonly string LogDir = Path.Combine(BaseDir, "logs");

        public static async Task SendAsync(Config cfg, ReadOnlySequence<byte> buffer, CancellationToken ct)
        {
            // Ensure the logs directory exists
            Directory.CreateDirectory(LogDir);

            // Build per-day protocol log file name: smtp-YYYYMMDD.log
            var protocolLogPath = Path.Combine(LogDir, $"smtp-{DateTime.Now:yyyyMMdd}.log");

            // Attach a ProtocolLogger to capture the full SMTP handshake & data exchange
            using var client = new SmtpClient(new ProtocolLogger(protocolLogPath));

            try
            {
                Log.Information("Connecting to smarthost {Host}:{Port} (STARTTLS={UseTls})",
                    cfg.SmartHost, cfg.SmartHostPort, cfg.UseStartTls);

                // Connect with or without StartTLS
                await client.ConnectAsync(
                    cfg.SmartHost,
                    cfg.SmartHostPort,
                    cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.None,
                    ct).ConfigureAwait(false);

                // Authenticate if credentials are provided
                if (!string.IsNullOrWhiteSpace(cfg.Username))
                {
                    Log.Information("Authenticating as {User}", cfg.Username);
                    await client.AuthenticateAsync(cfg.Username, cfg.Password, ct).ConfigureAwait(false);
                }

                // Load the incoming message from the byte buffer
                var message = MimeMessage.Load(buffer.AsStream());

                Log.Information("Sending message from {From} to {To}", message.From, message.To);
                await client.SendAsync(message, ct).ConfigureAwait(false);

                // Gracefully disconnect
                await client.DisconnectAsync(true, ct).ConfigureAwait(false);
                Log.Information("Smarthost relay complete");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error relaying message to smarthost");
                throw;
            }
        }
    }
}
