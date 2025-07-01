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

        public string SmartHost { get; set; } = "";
        public int SmartHostPort { get; set; } = 25;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool UseStartTls { get; set; }
        public bool AllowAllIPs { get; set; } = true;
        public List<string> AllowedIPs { get; set; } = new();
        public bool EnableLogging { get; set; }
        public int RetentionDays { get; set; } = 7;

        public static Config Load()
        {
            if (!File.Exists(_path))
            {
                var cfg = new Config();
                Save(cfg);
                return cfg;
            }

            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<Config>(json)!
                ?? throw new InvalidOperationException("Failed to parse config");
        }

        public static void Save(Config cfg)
        {
            // validate IP list
            if (!cfg.AllowAllIPs && cfg.AllowedIPs.Any())
            {
                foreach (var entry in cfg.AllowedIPs)
                {
                    // this will throw if invalid
                    _ = IPAddressRange.Parse(entry.Trim());
                }
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(cfg, options);
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            File.WriteAllText(_path, json);
        }

        /// <summary>
        /// Returns true if the remoteAddress is permitted by the config.
        /// </summary>
        public bool IsAllowed(IPAddress remoteAddress)
        {
            if (AllowAllIPs) return true;

            foreach (var entry in AllowedIPs)
            {
                var range = IPAddressRange.Parse(entry.Trim());
                if (range.Contains(remoteAddress))
                    return true;
            }

            return false;
        }
    }
}
