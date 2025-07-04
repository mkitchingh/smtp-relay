using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SmtpRelay
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            // Base folders under Program Files\SMTP Relay\service
            var baseDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service");
            var logDir = Path.Combine(baseDir, "logs");
            Directory.CreateDirectory(logDir);

            // Read retention from config if you like, or hard-code
            const int retentionDays = 30;

            // Serilog: only the general application log
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                // General application log (app-YYYYMMDD.log)
                .WriteTo.File(
                    Path.Combine(logDir, "app-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: retentionDays)
                .CreateLogger();

            try
            {
                Log.Information("Starting SMTP Relay Service");

                Host.CreateDefaultBuilder(args)
                    .UseWindowsService()
                    .UseSerilog()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<Worker>();
                    })
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
