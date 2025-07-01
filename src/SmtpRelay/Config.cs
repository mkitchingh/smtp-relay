using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using IPAddressRange;   // <— This import is required for IPAddressRange to resolve

namespace SmtpRelay
{
    public class Config
    {
        private const string FileName = "config.json";

        public string   SmartHost      { get; set; } = "";
        public int      SmartHostPort  { get; set; } = 25;
        public string?  Username       { get; set; }
        public string?  Password       { get; set; }
        public bool     UseStartTls    { get; set; }
        public bool     AllowAllIPs    { get; set; } = true;
        public List<string> AllowedIPs { get; set; } = new();
        public bool     EnableLogging  { get; set; }
        public int      RetentionDays  { get; set; } = 7;

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
            // Validate the comma-separated list of ranges
            if (!AllowAllIPs)
            {
                // throws FormatException if any entry is invalid
                _ = AllowedIPs
                    .Select(s => s.Trim())
                    .Select(s => new IPAddressRange.IPAddressRange(s))
                    .ToList();
            }

            // Serialize back to disk
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
        }
    }
}
