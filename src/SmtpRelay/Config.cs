using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NetTools; // IPAddressRange

namespace SmtpRelay
{
    public class Config
    {
        public string SmartHost { get; set; } = "";
        public int SmartHostPort { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool UseStartTls { get; set; }
        public bool AllowAllIPs { get; set; }
        public List<IPAddressRange> AllowedIPs { get; set; } = new();
        public bool EnableLogging { get; set; }
        public int RetentionDays { get; set; }

        private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

        private static string PathConfig =>
            System.IO.Path.Combine(Program.BaseDir, "config.json");

        public static Config Load()
        {
            if (!File.Exists(PathConfig))
            {
                var def = new Config
                {
                    SmartHost = "smtp.example.com",
                    SmartHostPort = 25,
                    UseStartTls = false,
                    AllowAllIPs = true,
                    EnableLogging = true,
                    RetentionDays = 7
                };
                Save(def);
                return def;
            }

            var json = File.ReadAllText(PathConfig);
            return JsonSerializer.Deserialize<Config>(json, _jsonOptions)!;
        }

        public static void Save(Config cfg)
        {
            // You could validate cfg.AllowedIPs here if needed
            var json = JsonSerializer.Serialize(cfg, _jsonOptions);
            File.WriteAllText(PathConfig, json);
        }
    }
}
