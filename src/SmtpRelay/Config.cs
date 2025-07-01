using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using IPAddressRange;  // from the IPAddressRange NuGet package

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
        public string AllowedIPs { get; set; } = "";
        public bool EnableLogging { get; set; }
        public int RetentionDays { get; set; }

        private static readonly string ConfigPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "SMTP Relay", "config.json"
            );

        public static Config Load()
        {
            if (!File.Exists(ConfigPath))
                return new Config();

            var json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<Config>(json) ?? new Config();
        }

        public void Save()
        {
            // Ensure folder exists
            var dir = Path.GetDirectoryName(ConfigPath)!;
            Directory.CreateDirectory(dir);

            // If we’re restricting IPs, split on commas or semicolons and parse each
            if (!AllowAllIPs)
            {
                var parsed = AllowedIPs
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Select(s => IPAddressRange.Parse(s))
                    .ToList();

                // Re-serialize back to normalized comma list
                AllowedIPs = string.Join(",", parsed.Select(r => r.ToString()));
            }

            // Write out JSON
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(ConfigPath, json);
        }
    }
}
