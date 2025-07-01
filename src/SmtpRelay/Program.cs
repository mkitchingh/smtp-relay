// src/SmtpRelay/Program.cs
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using MailKit.Net.Smtp;                   // for ProtocolLogger
using SmtpServer;                          // for SmtpServerOptionsBuilder

namespace SmtpRelay
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            // 1) Prepare directories
            var baseDir = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service");
            var logDir = Path.Combine(baseDir, "logs");
            Directory.CreateDirectory(logDir);

            // 2) Load retention setting
            var cfg       = Config.Load();
            var retention = cfg.RetentionDays;

            // 3) Purge old protocol-log files
            foreach (var file in Directory.GetFiles(logDir, "smtp-proto-*.log"))
            {
                if (File.GetCreationTime(file) < DateTime.Now.AddDays(-retention))
                    File.Delete(file);
            }

            // 4) Configure Serilog for application events
            var appLog = Path.Combine(logDir, "app-.log");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    appLog,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: retention)
                .CreateLogger();

            try
            {
                Log.Information("Starting SMTP Relay Service");

                // 5) Build SMTP server options with protocol logging
                var protoLogFile = Path.Combine(
                    logDir,
                    $"smtp-proto-{DateTime.Now:yyyyMMdd}.log");

                var options = new SmtpServerOptionsBuilder()
                    .ServerName("SMTP Relay")
                    .Port(cfg.UseStartTls ? cfg.SmartHostPort : 25, cfg.UseStartTls)
                    .ProtocolLogger(_ => new ProtocolLogger(protoLogFile, append: true))
                    .Build();

                Host.CreateDefaultBuilder(args)
                    .UseWindowsService()
                    .UseSerilog()
                    .ConfigureServices((_, services) =>
                    {
                        services
                            .AddSingleton(options)
                            .AddHostedService<Worker>();
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
