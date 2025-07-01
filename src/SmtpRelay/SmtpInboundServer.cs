using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmtpServer;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using System.Buffers;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace SmtpRelay
{
    public class MessageRelayStore : MessageStore
    {
        private readonly Config _cfg;
        private readonly ILogger _logger;

        public MessageRelayStore(Config cfg, ILogger logger)
        {
            _cfg = cfg;
            _logger = logger;
        }

        public override async Task<SmtpResponse> SaveAsync(
            ISessionContext context,
            IMessageTransaction transaction,
            ReadOnlySequence<byte> buffer,
            CancellationToken cancellationToken)
        {
            var ip = context.RemoteEndPoint?.Address.ToString() ?? "unknown";
            _logger.LogInformation("Incoming relay request from {IP}", ip);

            // enforce IP restrictions
            if (!_cfg.AllowAllIPs)
            {
                var clientIp = context.RemoteEndPoint!.Address;
                var allowed = false;
                foreach (var range in _cfg.GetRanges())
                {
                    if (range.Contains(clientIp))
                    {
                        allowed = true;
                        break;
                    }
                }
                if (!allowed)
                {
                    _logger.LogWarning("DENIED {IP} — not in allow-list", ip);
                    return new SmtpResponse(SmtpStatusCode.TransactionFailed, "550 Relaying Denied");
                }
            }

            try
            {
                await MailSender.SendAsync(_cfg, buffer, cancellationToken);
                _logger.LogInformation("Relay success from {IP}", ip);
                return SmtpResponse.Ok;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error relaying message to smarthost");
                return new SmtpResponse(SmtpStatusCode.LocalErrorInProcessing, "451 Requested action aborted: local error in processing");
            }
        }
    }
}
