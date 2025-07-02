using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using IPAddressRange;

namespace SmtpRelay
{
    public class Config
    {
        public string   SmartHost      { get; set; } = "";
        public int      SmartHostPort  { get; set; }
        public string?  Username       { get; set; }
        public string?  Password       { get; set; }
        public bool     UseStartTls    { get; set; }
        public bool     AllowAllIPs    { get; set; }
        public List<string> AllowedIPs { get; set; } = new();
        public bool     EnableLogging  { get; set; }
        public int      RetentionDays  { get; set; }

        const string ConfigPath = @"C:\Program Files\SMTP Relay\service\config.json";

        public static Config Load()
        {
            if (!File.Exists(ConfigPath)) throw new FileNotFoundException(ConfigPath);
            var json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<Config>(json)
                ?? throw new InvalidOperationException("Failed to parse config.json");
        }

        public void Save()
        {
            if (!AllowAllIPs)
            {
                // validate each IP or CIDR
                foreach (var entry in AllowedIPs)
                {
                    _ = new IPAddressRange(entry); // throws on invalid
                }
            }
            var opts = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(ConfigPath, JsonSerializer.Serialize(this, opts));
        }

        public bool IsAllowed(IPAddress address)
        {
            if (AllowAllIPs) return true;
            foreach (var entry in AllowedIPs)
            {
                var range = new IPAddressRange(entry);
                if (range.Contains(address)) return true;
            }
            return false;
        }
    }
}
