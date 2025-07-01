using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using IPAddressRange;

namespace SmtpRelay
{
    public class Config
    {
        public string SmartHost { get; set; } = "";
        public int SmartHostPort { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool UseStartTls { get; set; }
        public bool AllowAllIPs { get; set; }
        public List<string> AllowedIPs { get; set; } = new();
        public bool EnableLogging { get; set; }
        public int RetentionDays { get; set; }

        private static readonly string ConfigPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SMTP Relay", "service", "config.json");

        public static Config Load()
        {
            if (!File.Exists(ConfigPath))
                return new Config();
            var json = File.ReadAllText(ConfigPath);
            var opts = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Deserialize<Config>(json, opts)!
                   ?? new Config();
        }

        public void Save()
        {
            var dir = Path.GetDirectoryName(ConfigPath)!;
            Directory.CreateDirectory(dir);
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }

        public IEnumerable<IPAddressRange> GetRanges()
        {
            if (AllowAllIPs)
            {
                yield return IPAddressRange.Parse("0.0.0.0/0");
                yield break;
            }

            foreach (var s in AllowedIPs)
                yield return IPAddressRange.Parse(s);
        }
    }
}
