using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using IPAddressRange; // from the IPAddressRange NuGet package

namespace SmtpRelay
{
    public class Config
    {
        public string SmartHost { get; set; } = "";
        public int SmartHostPort { get; set; } = 25;
        public bool UseStartTls { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public bool AllowAllIPs { get; set; } = true;
        public List<string>? AllowedIPs { get; set; } = new();

        public bool EnableLogging { get; set; } = true;
        public int RetentionDays { get; set; } = 7;

        static string ConfigFilePath =>
            Path.Combine(AppContext.BaseDirectory, "config.json");

        public static Config Load()
        {
            if (!File.Exists(ConfigFilePath))
                return new Config();

            var json = File.ReadAllText(ConfigFilePath);
            return JsonSerializer.Deserialize<Config>(json)!
                   ?? new Config();
        }

        public void Save()
        {
            if (!AllowAllIPs && (AllowedIPs == null || !AllowedIPs.Any()))
                throw new InvalidOperationException(
                    "When AllowAllIPs is false, you must specify at least one AllowedIP.");

            var opts = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, opts);
            File.WriteAllText(ConfigFilePath, json);
        }

        public IEnumerable<IPAddressRange> GetAllowedRanges()
        {
            if (AllowAllIPs)
            {
                yield return new IPAddressRange(
                    IPAddress.Parse("0.0.0.0"),
                    IPAddress.Parse("255.255.255.255"));
            }
            else
            {
                foreach (var text in AllowedIPs!)
                    yield return IPAddressRange.Parse(text);
            }
        }
    }
}
