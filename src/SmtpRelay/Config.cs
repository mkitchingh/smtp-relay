using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NetTools;

namespace SmtpRelay
{
    public class Config
    {
        private static readonly string ConfigFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "SMTP Relay", "service", "config.json");

        public string SmartHost { get; set; } = string.Empty;
        public int SmartHostPort { get; set; } = 25;
        public string? Username  { get; set; }
        public string? Password  { get; set; }
        public bool   UseStartTls   { get; set; } = false;
        public bool   AllowAllIPs   { get; set; } = true;
        public List<string>? AllowedIPs   { get; set; } = new();
        public bool   EnableLogging { get; set; } = false;
        public int    RetentionDays { get; set; } = 7;

        public static Config Load()
        {
            if (!File.Exists(ConfigFilePath))
            {
                var cfg = new Config();
                cfg.Save();
                return cfg;
            }

            var json = File.ReadAllText(ConfigFilePath);
            return JsonSerializer.Deserialize<Config>(json)!;
        }

        public void Save()
        {
            if (!AllowAllIPs && AllowedIPs is not null)
            {
                // validate each entry or throw
                foreach (var entry in AllowedIPs)
                {
                    _ = IPAddressRange.Parse(entry);
                }
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json    = JsonSerializer.Serialize(this, options);
            File.WriteAllText(ConfigFilePath, json);
        }
    }
}
