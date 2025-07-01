using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SmtpRelay
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var baseDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay","service");
            var logDir = Path.Combine(baseDir,"logs");
            Directory.CreateDirectory(logDir);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    Path.Combine(logDir,"app-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: _getRetention())
                .CreateLogger();

            try
            {
                Log.Information("Starting SMTP Relay Service");
                Host.CreateDefaultBuilder(args)
                    .UseWindowsService()
                    .UseSerilog()
                    .ConfigureServices((_,services) =>
                    {
                        var cfg = Config.Load();
                        services.AddSingleton(cfg);
                        services.AddSingleton<Serilog.ILogger>(Log.Logger);
                        services.AddHostedService<Worker>();
                    })
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex,"Service terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            static int _getRetention()
            {
                try { return Config.Load().RetentionDays; }
                catch { return 7; }
            }
        }
    }
}
