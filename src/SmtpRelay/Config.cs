using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using NetTools;

namespace SmtpRelay
{
    public class Config
    {
        private const string FileName = "config.json";
        private static readonly string _path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "SMTP Relay", "service", FileName);

        public string SmartHost { get; set; } = "localhost";
        public int SmartHostPort { get; set; } = 25;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool UseStartTls { get; set; }
        public bool AllowAllIPs { get; set; } = true;
        public List<string> AllowedIPs { get; set; } = new();
        public bool EnableLogging { get; set; } = true;
        public int RetentionDays { get; set; } = 7;

        public static Config Load()
        {
            if (!File.Exists(_path))
            {
                var fresh = new Config();
                Save(fresh);
                return fresh;
            }

            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<Config>(json)!
                   ?? throw new InvalidOperationException("Failed to parse config.json");
        }

        public static void Save(Config cfg)
        {
            // Validate each CIDR/IP if not AllowAll
            if (!cfg.AllowAllIPs)
            {
                foreach (var entry in cfg.AllowedIPs)
                {
                    IPAddressRange.Parse(entry.Trim());
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_path, JsonSerializer.Serialize(cfg, options));
        }

        public bool IsAllowed(IPAddress address)
        {
            if (AllowAllIPs) return true;
            return AllowedIPs
                .Select(e => IPAddressRange.Parse(e.Trim()))
                .Any(r => r.Contains(address));
        }
    }
}
