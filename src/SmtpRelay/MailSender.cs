using System;
using System.IO;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

namespace SmtpRelay
{
    public static class MailSender
    {
        static readonly string BaseDir = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ProgramFiles),
            "SMTP Relay", "service");
        static readonly string LogDir = Path.Combine(BaseDir, "logs");

        public static async Task SendAsync(
            Config cfg,
            ReadOnlySequence<byte> buffer,
            CancellationToken ct)
        {
            // Ensure logs directory exists
            Directory.CreateDirectory(LogDir);

            // Trimmed protocol transcript: smtp-proto-YYYYMMDD.log
            var protoPath = Path.Combine(
                LogDir,
                $"smtp-proto-{DateTime.Now:yyyyMMdd}.log");

            // Use our filtered logger on the outgoing client
            using var client = new SmtpClient(
                new FilteredProtocolLogger(protoPath));

            try
            {
                Log.Information(
                    "Connecting to {Host}:{Port} (STARTTLS={Tls})",
                    cfg.SmartHost, cfg.SmartHostPort, cfg.UseStartTls);

                await client.ConnectAsync(
                        cfg.SmartHost,
                        cfg.SmartHostPort,
                        cfg.UseStartTls,
                        ct)
                    .ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(cfg.Username))
                {
                    Log.Information("Authenticating as {User}", cfg.Username);
                    await client.AuthenticateAsync(
                            cfg.Username,
                            cfg.Password,
                            ct)
                        .ConfigureAwait(false);
                }

                // Load the inbound message
                using var ms = new MemoryStream(buffer.ToArray());
                var message = await MimeMessage
                    .LoadAsync(ms, ct)
                    .ConfigureAwait(false);

                Log.Information(
                    "Sending message from {From} to {To}",
                    message.From, message.To);

                await client.SendAsync(message, ct)
                            .ConfigureAwait(false);

                await client.DisconnectAsync(true, ct)
                            .ConfigureAwait(false);

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
