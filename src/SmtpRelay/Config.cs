using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using NetTools;

namespace SmtpRelay
{
    public class Config
    {
        public string  SmartHost      { get; set; } = "";
        public int     SmartHostPort  { get; set; } = 25;
        public string  Username       { get; set; } = "";
        public string  Password       { get; set; } = "";
        public bool    UseStartTls    { get; set; } = false;

        public bool    AllowAllIPs    { get; set; } = true;
        public List<string> AllowedIPs { get; set; } = new();

        public bool    EnableLogging  { get; set; } = false;
        public int     RetentionDays  { get; set; } = 30;

        private static string FilePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static Config Load()
        {
            if (!File.Exists(FilePath))
                return new Config();
            return JsonSerializer.Deserialize<Config>(File.ReadAllText(FilePath)) ?? new Config();
        }

        public void Save()
        {
            /* Validate IP / CIDR entries */
            try
            {
                if (!AllowAllIPs)
                {
                    foreach (var s in AllowedIPs)
                        _ = IPAddressRange.Parse(s);   // throws if invalid
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Invalid IP/CIDR in allow-list:\n{s}\n\n{ex.Message}",
                                "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;  // prevent save
            }

            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
