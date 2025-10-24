using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IdConverter
{
    /// <summary>
    /// Handles application logging with batch purging
    /// </summary>
    public static class AppLogger
    {
        private static string _logFilePath;

        /// <summary>
        /// Initializes the logger with the specified log file path
        /// </summary>
        public static void Initialize(string logFilePath)
        {
            _logFilePath = logFilePath;
            
            try
            {
                // Write initial log entry
                WriteLog("=== Sitecore ID Converter Debug Log Started ===");
                WriteLog($"Log file: {_logFilePath}");
                WriteLog($"Application folder: {Path.GetDirectoryName(_logFilePath)}");
            }
            catch (Exception ex)
            {
                // Fallback to console if file logging fails
                System.Diagnostics.Debug.WriteLine($"Failed to initialize log file: {ex.Message}");
            }
        }

        /// <summary>
        /// Writes a message to the log file with batch purging
        /// </summary>
        public static void WriteLog(string message)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string logEntry = $"[{timestamp}] {message}";
                
                // Use default purge thresholds to avoid circular dependency
                int purgeThreshold = 400;
                int keepLines = 200;
                
                // Read existing lines
                string[] existingLines = File.Exists(_logFilePath) ? File.ReadAllLines(_logFilePath) : new string[0];
                List<string> allLines = new List<string>(existingLines) { logEntry };
                
                // Batch purge: only trim when we exceed the threshold
                if (allLines.Count > purgeThreshold)
                {
                    allLines = allLines.Skip(allLines.Count - keepLines).ToList();
                    
                    // Add purge notification as first line after purge
                    string purgeNote = $"[{timestamp}] === Log purged ===";
                    allLines.Insert(0, purgeNote);
                }
                
                File.WriteAllLines(_logFilePath, allLines);
                
                // Also write to debug console and console
                System.Diagnostics.Debug.WriteLine(logEntry);
                Console.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                // Fallback to console if file writing fails
                string errorMsg = $"Log write failed: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(errorMsg);
                Console.WriteLine(errorMsg);
            }
        }
    }
}
