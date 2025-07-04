// src/SmtpRelay/Program.cs

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
            // where we keep logs under Program Files\SMTP Relay\service\logs
            var baseDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service");
            var logDir = Path.Combine(baseDir, "logs");
            Directory.CreateDirectory(logDir);

            // Serilog: only the application log here
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                // General application log (app-*.log)
                .WriteTo.File(
                    Path.Combine(logDir, "app-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30)
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
