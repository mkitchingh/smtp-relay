using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using IPAddressRange;

namespace SmtpRelay
{
    public class Config
    {
        private static readonly string _path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "SMTP Relay", "config.json");

        public string SmartHost { get; set; } = "";
        public int SmartHostPort { get; set; } = 25;
        public bool UseStartTls { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public bool AllowAllIPs { get; set; } = true;
        public List<IPAddressRange> AllowedIPs { get; set; } = new();

        public bool EnableLogging { get; set; } = true;
        public int RetentionDays { get; set; } = 7;

        public static Config Load()
        {
            if (!File.Exists(_path))
                return new Config();

            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<Config>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new Config();
        }

        /// <summary>
        /// rawAllowList: comma-separated list of CIDRs/IPs, e.g. "10.0.0.0/8,127.0.0.1"
        /// </summary>
        public void Save(string rawAllowList)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);

            if (!AllowAllIPs)
            {
                AllowedIPs = new List<IPAddressRange>();
                var entries = rawAllowList
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var e in entries)
                {
                    AllowedIPs.Add(IPAddressRange.Parse(e));
                }
            }

            File.WriteAllText(_path, JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }
    }
}
