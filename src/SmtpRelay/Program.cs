using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters.Matching;

namespace SmtpRelay
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            // Prepare directories
            var baseDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service");
            var logDir = Path.Combine(baseDir, "logs");
            Directory.CreateDirectory(logDir);

            // Load retention setting
            var cfg = Config.Load();
            var retention = cfg.RetentionDays;

            // Paths
            var appLogPath  = Path.Combine(logDir, "app-.log");
            var smtpLogPath = Path.Combine(logDir, "smtp-.log");

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()

                // 1) General application log
                .WriteTo.File(
                    appLogPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: retention)

                // 2) Dedicated SMTP-only log
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(Matching.FromSource("SmtpServer"))
                    .WriteTo.File(
                        smtpLogPath,
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: retention))

                .CreateLogger();

            try
            {
                Log.Information("Starting SMTP Relay Service");
                Host.CreateDefaultBuilder(args)
                    .UseWindowsService()
                    .UseSerilog()
                    .ConfigureServices((_, services) =>
                        services.AddHostedService<Worker>())
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Service terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
