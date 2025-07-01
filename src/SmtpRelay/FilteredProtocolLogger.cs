using System;
using System.IO;
using System.Text;
using MailKit;

namespace SmtpRelay
{
    /// <summary>
    /// Captures only SMTP commands & replies, skipping DATA bodies,
    /// and writes them as timestamped lines.
    /// </summary>
    public class FilteredProtocolLogger : IProtocolLogger, IDisposable
    {
        readonly string _filePath;
        readonly object _sync = new();
        bool _inData;

        public FilteredProtocolLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void LogClient(byte[] buffer, int offset, int count)
        {
            var line = Encoding.UTF8
                .GetString(buffer, offset, count)
                .TrimEnd('\r', '\n');
            ProcessLine("CLIENT → SERVER", line);
        }

        public void LogServer(byte[] buffer, int offset, int count)
        {
            var line = Encoding.UTF8
                .GetString(buffer, offset, count)
                .TrimEnd('\r', '\n');
            ProcessLine("SERVER → CLIENT", line);
        }

        void ProcessLine(string prefix, string line)
        {
            // Skip DATA bodies
            if (_inData)
            {
                if (line == ".") _inData = false;
                return;
            }
            if (line.StartsWith("DATA", StringComparison.OrdinalIgnoreCase))
            {
                _inData = true;
            }

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var outLine = $"{timestamp} {prefix}: {line}";
            lock (_sync)
            {
                File.AppendAllText(_filePath, outLine + Environment.NewLine);
            }
        }

        public void Dispose() { /* nothing to clean up */ }
    }
}
