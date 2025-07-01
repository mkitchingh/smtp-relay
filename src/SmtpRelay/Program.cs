using System;
using System.IO;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SmtpRelay
{
    internal static class Program
    {
        // Base directory for config & logs
        public static string BaseDir { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "SMTP Relay", "service");

        static void Main(string[] args)
        {
            // Ensure folders exist
            Directory.CreateDirectory(BaseDir);
            Directory.CreateDirectory(Path.Combine(BaseDir, "logs"));

            // Configure Serilog (app-only log, rotated daily)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    Path.Combine(BaseDir, "logs", "app-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7)
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
