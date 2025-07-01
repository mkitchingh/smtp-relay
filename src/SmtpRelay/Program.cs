using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Filters;
using Serilog.Filters.Expressions;

namespace SmtpRelay
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            // Ensure service folder exists
            var baseDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service");
            var logDir = Path.Combine(baseDir, "logs");
            Directory.CreateDirectory(logDir);

            // Load retention setting from shared config.json
            var cfg = Config.Load();
            var retention = cfg.RetentionDays;

            // Serilog: app log + dedicated SMTP log with dynamic retention
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                // General application log
                .WriteTo.File(
                    Path.Combine(logDir, "app-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: retention)
                // SMTP‐only log (entries from the SmtpServer library)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(Matching.FromSource("SmtpServer"))
                    .WriteTo.File(
                        Path.Combine(logDir, "smtp-.log"),
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
