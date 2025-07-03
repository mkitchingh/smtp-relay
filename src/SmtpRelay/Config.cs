using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NetTools;  // <-- IPAddressRange lives here

namespace SmtpRelay
{
    public class Config
    {
        // General SMTP settings
        public string   SmartHost       { get; set; } = "";
        public int      SmartHostPort   { get; set; } = 25;
        public string?  Username        { get; set; }
        public string?  Password        { get; set; }
        public bool     UseStartTls     { get; set; } = false;

        // Relay restrictions
        public bool     AllowAllIPs     { get; set; } = true;
        public string   AllowedIPsText  { get; set; } = ""; // comma-separated list as entered in GUI
        public List<IPAddressRange> AllowedIPs { get; set; } = new();

        // Logging & retention
        public bool     EnableLogging   { get; set; } = true;
        public int      RetentionDays   { get; set; } = 30;

        // Where the file lives on disk
        private static string ConfigFilePath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service", "config.json");

        public static Config Load()
        {
            if (!File.Exists(ConfigFilePath))
            {
                var cfg = new Config();
                cfg.Save();
                return cfg;
            }

            var json = File.ReadAllText(ConfigFilePath);
            return JsonSerializer.Deserialize<Config>(json)!
                   ?? throw new InvalidOperationException("Failed to deserialize config");
        }

        public void Save()
        {
            // Ensure folder exists
            var dir = Path.GetDirectoryName(ConfigFilePath)!;
            Directory.CreateDirectory(dir);

            // Rebuild the AllowedIPs list from the comma-separated text
            AllowedIPs.Clear();
            if (!AllowAllIPs && !string.IsNullOrWhiteSpace(AllowedIPsText))
            {
                var entries = AllowedIPsText
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim());

                foreach (var entry in entries)
                {
                    // This will throw FormatException if entry is invalid
                    var range = IPAddressRange.Parse(entry);
                    AllowedIPs.Add(range);
                }
            }

            // Write out the full Config object (including the text and the parsed ranges)
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(ConfigFilePath, json);
        }
    }
}
