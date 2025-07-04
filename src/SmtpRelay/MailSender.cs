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
        private readonly string _logDirectory;

        public MailSender(Config cfg, ILogger log)
        {
            _cfg = cfg;
            _log = log;
            // where the service writes its logs (same as Program.cs)
            var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            _logDirectory = Path.Combine(baseDir, "SMTP Relay", "service", "logs");
        }

        public async Task SendAsync(ReadOnlySequence<byte> buffer, CancellationToken ct)
        {
            try
            {
                // parse incoming message
                var data = buffer.ToArray();
                var message = MimeMessage.Load(new MemoryStream(data));

                // relay to smart host
                using var client = new SmtpClient();

                // connect (STARTTLS or plaintext depending on config)
                await client.ConnectAsync(
                    _cfg.SmartHost,
                    _cfg.SmartHostPort,
                    _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto,
                    ct);

                if (!string.IsNullOrEmpty(_cfg.Username))
                    await client.AuthenticateAsync(_cfg.Username, _cfg.Password, ct);

                await client.SendAsync(message, ct);
                await client.DisconnectAsync(true, ct);

                _log.Information("Relayed message from {Remote} to {Host}:{Port}",
                    message.From, _cfg.SmartHost, _cfg.SmartHostPort);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Relay failure from {Remote}", 
                    System.Net.IPAddress.None); // you can swap in context.Remote
                throw;
            }
        }
    }
}
