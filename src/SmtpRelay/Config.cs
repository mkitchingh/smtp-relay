using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NetTools;

namespace SmtpRelay
{
    public class Config
    {
        private const string FileName = "config.json";

        public string SmartHost { get; set; } = "localhost";
        public int    SmartHostPort { get; set; } = 25;
        public string? Username      { get; set; }
        public string? Password      { get; set; }
        public bool   UseStartTls    { get; set; } = false;

        public bool         AllowAllIPs  { get; set; } = true;
        public List<string> AllowedIPs   { get; set; } = new();

        public bool EnableLogging { get; set; } = false;
        public int  RetentionDays { get; set; } = 7;

        public static Config Load()
        {
            if (!File.Exists(FileName))
            {
                var cfg = new Config();
                cfg.Save();
                return cfg;
            }

            var json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<Config>(json, new JsonSerializerOptions { WriteIndented = true })!;
        }

        public void Save()
        {
            if (!AllowAllIPs)
            {
                // validate each CIDR/IP
                foreach (var entry in AllowedIPs)
                {
                    _ = IPAddressRange.Parse(entry);
                }
            }

            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
        }
    }
}
