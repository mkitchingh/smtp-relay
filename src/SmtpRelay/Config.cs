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

        public bool EnableLogging  { get; set; } = false;
        public int  RetentionDays  { get; set; } = 30;

        /*───────── persistence ─────────*/
        static string FilePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static Config Load()
        {
            if (!File.Exists(FilePath)) return new();
            return JsonSerializer.Deserialize<Config>(
                       File.ReadAllText(FilePath)) ?? new();
        }

        public void Save()
        {
            if (!AllowAllIPs)
            {
                foreach (var entry in AllowedIPs)
                {
                    try { _ = IPAddressRange.Parse(entry); }
                    catch (Exception ex)
                    {
                        throw new FormatException(
                            $"Invalid IP or CIDR \"{entry}\".\n{ex.Message}");
                    }
                }
            }

            var json = JsonSerializer.Serialize(
                this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
