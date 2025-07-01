using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using SmtpServer;
using SmtpServer.Protocol;

namespace SmtpRelay
{
    public class SmtpInboundServer : MessageStore
    {
        public override async Task<SmtpResponse> SaveAsync(
            ISessionContext context,
            IMessageTransaction transaction,
            ReadOnlySequence<byte> buffer,
            CancellationToken cancellationToken)
        {
            // Relay the raw SMTP transaction to the smarthost
            var success = await MailSender.SendAsync(
                Config.Load(),
                buffer,
                cancellationToken);

            return success
                ? SmtpResponse.Ok
                : new SmtpResponse(
                    SmtpReplyCode.TransactionFailed,
                    "Message relay failed");
        }
    }
}
