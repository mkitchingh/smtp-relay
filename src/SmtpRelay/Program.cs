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
            // Base/service directories
            var baseDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service");
            var logDir = Path.Combine(baseDir, "logs");
            Directory.CreateDirectory(logDir);

            // Read retention from config
            var cfg = Config.Load();
            var retention = cfg.RetentionDays;

            // Serilog: app logs + SMTP-only logs
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                // General application log
                .WriteTo.File(
                    Path.Combine(logDir, "app-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: retention)
                // Dedicated SMTP-only log using a lambda filter
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(le =>
                        le.Properties.TryGetValue("SourceContext", out var sc) &&
                        sc.ToString().Contains("SmtpServer"))
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
