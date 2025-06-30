using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NetTools;

namespace SmtpRelay
{
    public class Config
    {
        public string SmartHost     { get; set; } = "";
        public int    SmartHostPort { get; set; } = 25;
        public string Username      { get; set; } = "";
        public string Password      { get; set; } = "";
        public bool   UseStartTls   { get; set; } = false;

        public bool AllowAllIPs       { get; set; } = true;
        public List<string> AllowedIPs { get; set; } = new();

        public bool EnableLogging { get; set; } = false;
        public int  RetentionDays { get; set; } = 30;

        static string FilePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static Config Load()
        {
            if (!File.Exists(FilePath)) return new Config();
            return JsonSerializer.Deserialize<Config>(File.ReadAllText(FilePath))
                   ?? new Config();
        }

        /// <summary>Validate & persist configuration.</summary>
        /// <exception cref="FormatException">if SmartHost empty or any IP/CIDR is invalid</exception>
        /// <exception cref="IOException">if writing the file fails (e.g. no permission)</exception>
        public void Save()
        {
            // Must have a non-empty SMTP host
            if (string.IsNullOrWhiteSpace(SmartHost))
                throw new FormatException("SMTP Host must not be empty.");

            // Validate each IP/CIDR entry
            if (!AllowAllIPs)
            {
                foreach (var entry in AllowedIPs)
                {
                    try
                    {
                        _ = IPAddressRange.Parse(entry);
                    }
                    catch (Exception ex)
                    {
                        throw new FormatException(
                            $"Invalid IP or CIDR entry \"{entry}\": {ex.Message}");
                    }
                }
            }

            // Ensure directory exists
            var dir = Path.GetDirectoryName(FilePath)!;
            Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(
                this,
                new JsonSerializerOptions { WriteIndented = true });

            // Write the file (will throw if permission denied)
            File.WriteAllText(FilePath, json);
        }
    }
}
