using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using IPAddressRange;

namespace SmtpRelay
{
    public class Config
    {
        public string SmartHost { get; set; } = "";
        public int SmartHostPort { get; set; } = 25;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool UseStartTls { get; set; } = false;

        /// <summary>
        /// If true, all IPs are allowed; otherwise only those in AllowedIPs.
        /// </summary>
        public bool AllowAllIPs { get; set; } = true;

        /// <summary>
        /// Comma-separated list of IPs or CIDRs (e.g. "10.0.0.0/8,192.168.0.0/16").
        /// </summary>
        public string AllowedIPs { get; set; } = "";

        public bool EnableLogging { get; set; } = true;
        public int RetentionDays { get; set; } = 30;

        // config.json lives next to the service EXE
        private static string ConfigFilePath =>
            Path.Combine(AppContext.BaseDirectory, "config.json");

        public static Config Load()
        {
            if (!File.Exists(ConfigFilePath))
                return new Config();

            var json = File.ReadAllText(ConfigFilePath);
            var cfg = JsonSerializer.Deserialize<Config>(json);
            return cfg ?? new Config();
        }

        public void Save()
        {
            // Validate and normalize the allowed-IPs list
            if (!AllowAllIPs)
            {
                // Split on commas, remove empty entries, trim whitespace
                var entries = AllowedIPs
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();

                // Validate each one by attempting to parse
                foreach (var entry in entries)
                {
                    // throws FormatException if bad
                    _ = IPAddressRange.Parse(entry);
                }

                // Store back as a clean, comma+space joined string
                AllowedIPs = string.Join(", ", entries);
            }
            else
            {
                // Clear out anything that was there
                AllowedIPs = "";
            }

            // Write out the JSON
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, options);

            File.WriteAllText(ConfigFilePath, json);
        }
    }
}
