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
        private const string FileName = "config.json";

        public string   SmartHost     { get; set; } = "";
        public int      SmartHostPort { get; set; } = 25;
        public bool     UseStartTls   { get; set; } = false;
        public string?  Username      { get; set; }
        public string?  Password      { get; set; }
        public bool     AllowAllIPs   { get; set; } = true;
        public List<string> AllowedIPs { get; set; } = new();
        public bool     EnableLogging { get; set; } = false;
        public int      RetentionDays { get; set; } = 7;

        public static Config Load()
        {
            if (!File.Exists(FileName))
                return new Config();

            var json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<Config>(json)!
                ?? new Config();
        }

        public void Save()
        {
            // Validate CIDRs & IPs
            if (!AllowAllIPs)
            {
                foreach (var entry in AllowedIPs)
                {
                    _ = IPAddressRange.IPAddressRange.Parse(entry.Trim());
                }
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(FileName, json);
        }
    }
}
