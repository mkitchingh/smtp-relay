using System;
using System.Collections.Generic;
using System.IO;
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
        public bool UseStartTls { get; set; } = false;
        public bool AllowAllIPs { get; set; } = true;
        public List<IPAddressRange> AllowedIPs { get; set; } = new();
        public bool EnableLogging { get; set; } = false;
        public int RetentionDays { get; set; } = 7;

        private static string FilePath =>
            Path.Combine(AppContext.BaseDirectory, "config.json");

        public static Config Load()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var opts = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var cfg = JsonSerializer.Deserialize<Config>(json, opts);
                if (cfg != null) return cfg;
            }

            var def = new Config();
            Save(def);
            return def;
        }

        public static void Save(Config cfg)
        {
            // (AllowedIPs is already a parsed list; GUI layer must populate it)
            var opts = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(cfg, opts);
            File.WriteAllText(FilePath, json);
        }
    }
}
