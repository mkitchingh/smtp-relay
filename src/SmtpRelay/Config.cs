using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using IPAddressRange;

namespace SmtpRelay
{
    public class Config
    {
        public string SmartHost { get; set; } = "";
        public int SmartHostPort { get; set; } = 25;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool UseStartTls { get; set; }
        public bool AllowAllIPs { get; set; } = true;
        public List<string> AllowedIPs { get; set; } = new List<string>();
        public bool EnableLogging { get; set; }
        public int RetentionDays { get; set; } = 7;

        static string FilePath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service", "config.json");

        public static Config Load()
        {
            if (!File.Exists(FilePath))
                return new Config();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<Config>(json)!
                   ?? new Config();
        }

        public void Save()
        {
            // Validate IP/CIDR entries if not AllowAll
            if (!AllowAllIPs)
            {
                var entries = AllowedIPs
                    .SelectMany(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    .Select(s => s.Trim());
                foreach (var e in entries)
                {
                    // throws if invalid
                    if (e.Contains('/'))
                        _ = IPAddressRange.Parse(e);
                    else
                        _ = IPAddress.Parse(e);
                }
            }

            // Ensure directory
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);

            // Write
            var opts = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(FilePath,
                JsonSerializer.Serialize(this, opts));
        }
    }
}
