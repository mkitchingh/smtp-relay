using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using IPAddressRange;

namespace SmtpRelay
{
    public class Config
    {
        public string SmartHost    { get; set; } = "";
        public int    SmartHostPort{ get; set; } = 25;
        public string? Username    { get; set; }
        public string? Password    { get; set; }
        public bool   UseStartTls  { get; set; }
        public bool   AllowAllIPs  { get; set; }
        public List<string>? AllowedIPs   { get; set; } = new();
        public bool   EnableLogging      { get; set; }
        public int    RetentionDays      { get; set; } = 7;

        const string FileName = "config.json";

        public static Config Load()
        {
            if(!File.Exists(FileName))
                return new Config();

            var json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<Config>(json)!
                ?? new Config();
        }

        public void Save()
        {
            if(!AllowAllIPs)
            {
                // validate each entry:
                foreach(var s in AllowedIPs!)
                    _ = IPAddressRange.Parse(s);
            }

            var opts = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, opts);
            File.WriteAllText(FileName, json);
        }
    }
}
