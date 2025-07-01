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
        public static void Main(string[] args)
        {
            // ensure log directory
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
                    retainedFileCountLimit:  _loadRetention())
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

            static int _loadRetention()
            {
                try
                {
                    var cfg = Config.Load();
                    return cfg.RetentionDays;
                }
                catch { return 7; }
            }
        }
    }
}
