using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace SmtpRelay
{
    public class Config
    {
        private const string FileName = "config.json";

        public string SmartHost { get; set; } = "";
        public int SmartHostPort { get; set; } = 25;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool UseStartTls { get; set; } = false;
        public bool AllowAllIPs { get; set; } = true;
        public List<string> AllowedIPs { get; set; } = new List<string>();
        public bool EnableLogging { get; set; } = true;
        public int RetentionDays { get; set; } = 30;

        public static Config Load()
        {
            if (!File.Exists(FileName))
                throw new FileNotFoundException($"Configuration file '{FileName}' not found");

            var json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<Config>(json)!
                ?? throw new InvalidOperationException("Failed to deserialize configuration");
        }

        public void Save()
        {
            // validate
            if (UseStartTls && SmartHostPort == 25)
                SmartHostPort = 587;
            if (!UseStartTls && SmartHostPort == 587)
                SmartHostPort = 25;

            if (!AllowAllIPs)
            {
                // ensure every entry is either an IP or CIDR
                var parsed = AllowedIPs.Select(s =>
                {
                    var parts = s.Split('/');
                    if (!IPAddress.TryParse(parts[0], out var ip))
                        throw new FormatException($"Invalid IP address '{s}'");

                    if (parts.Length == 1)
                        return s; // single IP is OK

                    if (!int.TryParse(parts[1], out var bits) || bits < 0 || bits > (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ? 32 : 128))
                        throw new FormatException($"Invalid CIDR suffix in '{s}'");

                    return s; // e.g. "10.0.0.0/8"
                }).ToList();

                AllowedIPs = parsed;
            }
            else
            {
                AllowedIPs.Clear();
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(FileName, JsonSerializer.Serialize(this, options));
        }
    }
}
