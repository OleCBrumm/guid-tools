using System;
using System.IO;
using System.Windows;

namespace SitecoreIdConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                // Initialize logger with log file in the application directory
                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app-debug.log");
                AppLogger.Initialize(logFilePath);
                
                AppLogger.WriteLog("=== Sitecore ID Converter Starting ===");
                AppLogger.WriteLog($"Working Directory: {Environment.CurrentDirectory}");
                AppLogger.WriteLog($"Base Directory: {AppDomain.CurrentDomain.BaseDirectory}");
                
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                AppLogger.WriteLog($"FATAL ERROR during startup: {ex.Message}");
                AppLogger.WriteLog($"Stack trace: {ex.StackTrace}");
                AppLogger.WriteLog($"FATAL ERROR: {ex}");
                throw;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                AppLogger.WriteLog("=== Sitecore ID Converter Shutting Down ===");
                base.OnExit(e);
            }
            catch (Exception ex)
            {
                AppLogger.WriteLog($"Error during shutdown: {ex.Message}");
                AppLogger.WriteLog($"Shutdown error: {ex}");
            }
        }
    }
}
