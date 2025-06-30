using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SmtpRelay
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var logDir = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service", "logs");
            System.IO.Directory.CreateDirectory(logDir);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    System.IO.Path.Combine(logDir, "log-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7)
                .CreateLogger();

            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSerilog()                       // Microsoft logging → Serilog
                .ConfigureServices(s => s.AddHostedService<Worker>())
                .Build()
                .Run();
        }
    }
}
