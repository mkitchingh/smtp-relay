using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SmtpRelay
{
    /// <summary>
    /// Strong-typed settings persisted to
    ///   %ProgramData%\SMTP Relay\config.json
    /// </summary>
    public class Config
    {
        /* ── SMTP relay target ─────────────────────────── */
        public string SmartHost      { get; set; } = "smtp.example.com";
        public int    SmartHostPort  { get; set; } = 25;
        public bool   UseStartTls    { get; set; } = true;   // ← new checkbox

        /* ── Auth (optional) ───────────────────────────── */
        public string Username       { get; set; } = "";
        public string Password       { get; set; } = "";

        /* ── IP relay restrictions ─────────────────────── */
        public bool            AllowAllIPs { get; set; } = false;
        public List<string>    AllowedIPs  { get; set; } = new() { "127.0.0.1" };

        /* ── Logging ───────────────────────────────────── */
        public bool EnableLogging  { get; set; } = true;
        public int  RetentionDays  { get; set; } = 30;

        /* ── Disk location for the JSON file ───────────── */
        private static string PathCfg => Path.Combine(
            System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.CommonApplicationData),
            "SMTP Relay", "config.json");

        /* ── Load / Save helpers ───────────────────────── */
        public static Config Load() =>
            File.Exists(PathCfg)
                ? JsonSerializer.Deserialize<Config>(
                        File.ReadAllText(PathCfg)) ?? new Config()
                : new Config();

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PathCfg)!);

            var opts = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(PathCfg, JsonSerializer.Serialize(this, opts));
        }
    }
}
