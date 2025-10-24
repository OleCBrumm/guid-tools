using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SitecoreIdConverter
{
    /// <summary>
    /// Application settings manager that handles theme and logging configuration
    /// </summary>
    public static class AppSettings
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SitecoreIdConverter.config");
        
        /// <summary>
        /// Gets a configuration value
        /// </summary>
        public static string GetValue(string key, string defaultValue = "")
        {
            try
            {
                if (!File.Exists(ConfigPath))
                {
                    return defaultValue;
                }
                
                var doc = XDocument.Load(ConfigPath);
                var element = doc.Root?.Element(key);
                return element?.Value ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
        
        /// <summary>
        /// Sets a configuration value
        /// </summary>
        public static void SetValue(string key, string value)
        {
            try
            {
                string logMessage = $"AppSettings.SetValue: key='{key}', value='{value}'";
                WriteToLogFile(logMessage);
                
                logMessage = $"ConfigPath: {ConfigPath}";
                WriteToLogFile(logMessage);
                
                XDocument doc;
                
                if (File.Exists(ConfigPath))
                {
                    doc = XDocument.Load(ConfigPath);
                    logMessage = $"Loaded existing config: {doc}";
                    WriteToLogFile(logMessage);
                }
                else
                {
                    doc = new XDocument(new XElement("configuration"));
                    logMessage = "Created new config document";
                    WriteToLogFile(logMessage);
                    
                    // Initialize default values for new config
                    InitializeDefaultValues(doc);
                }
                
                var element = doc.Root?.Element(key);
                if (element != null)
                {
                    element.Value = value;
                    logMessage = $"Updated existing element: {key} = {value}";
                    WriteToLogFile(logMessage);
                }
                else
                {
                    doc.Root?.Add(new XElement(key, value));
                    logMessage = $"Added new element: {key} = {value}";
                    WriteToLogFile(logMessage);
                }
                
                doc.Save(ConfigPath);
                logMessage = $"Saved config to: {ConfigPath}";
                WriteToLogFile(logMessage);
                
                // Verify the save worked
                string savedValue = GetValue(key, "NOT_FOUND");
                logMessage = $"Verification - read back: {savedValue}";
                WriteToLogFile(logMessage);
            }
            catch (Exception ex)
            {
                string logMessage = $"AppSettings.SetValue error: {ex.Message}";
                WriteToLogFile(logMessage);
            }
        }
        
        /// <summary>
        /// Initializes default configuration values
        /// </summary>
        private static void InitializeDefaultValues(XDocument doc)
        {
            try
            {
                // Add default logging configuration
                doc.Root?.Add(new XElement("LogPurgeThreshold", "400"));
                doc.Root?.Add(new XElement("LogKeepLines", "200"));
                doc.Root?.Add(new XElement("ConfigLogPurgeThreshold", "200"));
                doc.Root?.Add(new XElement("ConfigLogKeepLines", "100"));
                
                WriteToLogFile("Initialized default configuration values");
            }
            catch (Exception ex)
            {
                WriteToLogFile($"Failed to initialize default values: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Writes a message to the log file
        /// </summary>
        private static void WriteToLogFile(string message)
        {
            try
            {
                string logFilePath = Path.Combine(Path.GetDirectoryName(ConfigPath) ?? "", "config-debug.log");
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = $"[{timestamp}] {message}";
                
                // Get purge thresholds from config
                int purgeThreshold = int.Parse(GetValue("ConfigLogPurgeThreshold", "200"));
                int keepLines = int.Parse(GetValue("ConfigLogKeepLines", "100"));
                
                // Read existing lines
                string[] existingLines = File.Exists(logFilePath) ? File.ReadAllLines(logFilePath) : new string[0];
                List<string> allLines = new List<string>(existingLines) { logEntry };
                
                // Batch purge: only trim when we exceed the threshold
                if (allLines.Count > purgeThreshold)
                {
                    allLines = allLines.Skip(allLines.Count - keepLines).ToList();
                    
                    // Add purge notification as first line after purge
                    string purgeNote = $"[{timestamp}] === Config log purged ===";
                    allLines.Insert(0, purgeNote);
                }
                
                File.WriteAllLines(logFilePath, allLines);
            }
            catch
            {
                // Ignore log errors
            }
        }
    }
}
