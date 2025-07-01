using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

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

            // Load retention from config.json
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

                // 2) Dedicated SMTP log, filtering on SourceContext
                .WriteTo.File(
                    smtpLogPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: retention,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    filter: logEvent =>
                        logEvent.Properties.TryGetValue("SourceContext", out var sc) &&
                        sc.ToString().Contains("SmtpServer"))

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
