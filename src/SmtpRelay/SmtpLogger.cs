using System;
using System.IO;
using MailKit.ProtocolLogger;
using Serilog;
using Serilog.Events;
// Alias Serilog’s ILogger so there’s no clash with Microsoft.Extensions.Logging.ILogger
using SerilogLogger = Serilog.ILogger;

namespace SmtpRelay
{
    public static class SmtpLogger
    {
        /// <summary>
        /// Creates (or re-uses) the MailKit ProtocolLogger writing to
        /// service/logs/smtp-proto-YYYYMMDD.log and returns it.
        /// </summary>
        public static ProtocolLogger CreateProtocolLogger()
        {
            // Determine the folder
            var logDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service", "logs");
            Directory.CreateDirectory(logDir);

            // Filename: smtp-proto-20250702.log for example
            var file = Path.Combine(
                logDir,
                $"smtp-proto-{DateTime.Now:yyyyMMdd}.log");

            // Append to existing file each day
            return new ProtocolLogger(file, append: true);
        }

        /// <summary>
        /// Configures Serilog to also write SMTP-only entries (MailKit’s ProtocolLogger write events)
        /// into the same logs folder, rolling daily.
        /// </summary>
        public static void ConfigureSerilog(SerilogLogger log)
        {
            // (Assumes your Program.cs has already created the "logs" folder)
            // This adds a sub‐logger for anything tagged with "MailKit.ProtocolLogger"
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(evt =>
                        evt.Properties.ContainsKey("SourceContext") &&
                        evt.Properties["SourceContext"].ToString().Contains("MailKit.ProtocolLogger"))
                    .WriteTo.File(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                            "SMTP Relay", "service", "logs",
                            "smtp-proto-.log"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 7))
                .CreateLogger();
        }
    }
}
