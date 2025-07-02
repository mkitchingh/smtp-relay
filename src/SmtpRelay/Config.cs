using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NetTools;

namespace SmtpRelay
{
    public class Config
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "SMTP Relay",
            "config.json");

        public string SmartHost { get; set; } = "";
        public int SmartHostPort { get; set; } = 25;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool UseStartTls { get; set; } = false;
        public bool AllowAllIPs { get; set; } = true;
        public List<string> AllowedIPs { get; set; } = new();
        public bool EnableLogging { get; set; } = false;
        public int RetentionDays { get; set; } = 7;

        public static Config Load()
        {
            if (!File.Exists(ConfigPath))
                return new Config();

            var json = File.ReadAllText(ConfigPath);
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Config>(json, opts) ?? new Config();
        }

        public void Save()
        {
            // ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);

            // validate each entry if not allowing all
            if (!AllowAllIPs)
            {
                foreach (var entry in AllowedIPs.Where(e => !string.IsNullOrWhiteSpace(e)))
                {
                    // throws if invalid CIDR or IP
                    _ = IPAddressRange.Parse(entry.Trim());
                }
            }

            var opts = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, opts);
            File.WriteAllText(ConfigPath, json);
        }
    }
}
