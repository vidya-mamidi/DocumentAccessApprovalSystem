using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAccessApprovalSystem.Infrastructure.Services
{
 
    public static class FileLogger
    {
        private static readonly string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        public static void Log(string fileName, string message)
        {
            try
            {
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                var logPath = Path.Combine(logDir, fileName);
                var logEntry = $"{DateTime.UtcNow:u} | {message}{Environment.NewLine}";
                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LogError] {ex.Message}");
            }
        }

        public static string[] ReadRecent(string fileName, int lineCount = 50)
        {
            var logPath = Path.Combine(logDir, fileName);
            if (!File.Exists(logPath)) return Array.Empty<string>();
            return File.ReadLines(logPath).Reverse().Take(lineCount).Reverse().ToArray();
        }
    }

}
