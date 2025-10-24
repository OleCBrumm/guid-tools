using System;
using System.Windows;
using System.Windows.Media;

namespace IdConverter
{
    /// <summary>
    /// Manages application theming
    /// </summary>
    public class ThemeManager
    {
        private string _currentTheme = "Auto";
        private readonly Window _window;

        public ThemeManager(Window window)
        {
            _window = window;
        }

        /// <summary>
        /// Gets the current theme
        /// </summary>
        public string CurrentTheme => _currentTheme;

        /// <summary>
        /// Loads the saved theme from configuration
        /// </summary>
        public void LoadSavedTheme()
        {
            // Load theme from app settings
            string savedTheme = AppSettings.GetValue("Theme", "Auto");
            _currentTheme = savedTheme;
            
            // Log what theme was loaded
            AppLogger.WriteLog($"Loaded theme: '{savedTheme}' from app settings");
            AppLogger.WriteLog($"Config file: SitecoreIdConverter.config");
        }

        /// <summary>
        /// Sets the current theme and saves it to configuration
        /// </summary>
        public void SetTheme(string theme)
        {
            _currentTheme = theme;
            
            AppLogger.WriteLog($"Theme selection changed to: '{_currentTheme}'");
            
            // Save theme choice to app settings
            AppSettings.SetValue("Theme", _currentTheme);
            AppLogger.WriteLog($"Theme saved to app settings: '{_currentTheme}'");
            
            // Verify what was actually saved
            string savedValue = AppSettings.GetValue("Theme", "Auto");
            AppLogger.WriteLog($"Theme read back from config: '{savedValue}'");
        }

        /// <summary>
        /// Applies the current theme to the window
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                switch (_currentTheme)
                {
                    case "Dark":
                        ApplyDarkTheme();
                        break;
                    case "Light":
                        ApplyLightTheme();
                        break;
                    case "Auto":
                    default:
                        if (ThemeHelper.IsWindowsInDarkMode())
                        {
                            ApplyDarkTheme();
                        }
                        else
                        {
                            ApplyLightTheme();
                        }
                        break;
                }
            }
            catch
            {
                // Fallback to light theme if theme application fails
                ApplyLightTheme();
            }
        }

        /// <summary>
        /// Gets the display name for the current theme
        /// </summary>
        public string GetThemeDisplayName()
        {
            return _currentTheme switch
            {
                "Dark" => "Dark",
                "Light" => "Light", 
                "Auto" => $"Auto (Windows: {(ThemeHelper.IsWindowsInDarkMode() ? "Dark" : "Light")})",
                _ => "Auto"
            };
        }

        private void ApplyDarkTheme()
        {
            // Switch to dark theme resources
            _window.Resources["BackgroundBrush"] = _window.Resources["DarkBackgroundBrush"];
            _window.Resources["CardBackgroundBrush"] = _window.Resources["DarkCardBackgroundBrush"];
            _window.Resources["CardBorderBrush"] = _window.Resources["DarkCardBorderBrush"];
            _window.Resources["TextPrimaryBrush"] = _window.Resources["DarkTextPrimaryBrush"];
            _window.Resources["TextSecondaryBrush"] = _window.Resources["DarkTextSecondaryBrush"];
            _window.Resources["TextTertiaryBrush"] = _window.Resources["DarkTextTertiaryBrush"];
            _window.Resources["InputBackgroundBrush"] = _window.Resources["DarkInputBackgroundBrush"];
            _window.Resources["InputBorderBrush"] = _window.Resources["DarkInputBorderBrush"];
            _window.Resources["ResultBackgroundBrush"] = _window.Resources["DarkResultBackgroundBrush"];
            _window.Resources["ResultBorderBrush"] = _window.Resources["DarkResultBorderBrush"];
            _window.Resources["ResultTextBrush"] = _window.Resources["DarkResultTextBrush"];
            _window.Resources["ResultLabelBrush"] = _window.Resources["DarkResultLabelBrush"];
            _window.Resources["StatusBackgroundBrush"] = _window.Resources["DarkStatusBackgroundBrush"];
            _window.Resources["StatusBorderBrush"] = _window.Resources["DarkStatusBorderBrush"];
            _window.Resources["StatusTextBrush"] = _window.Resources["DarkStatusTextBrush"];
            
            // Update window and grid backgrounds
            _window.Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A));
            if (_window.Content is System.Windows.Controls.Grid mainGrid)
            {
                mainGrid.Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A));
            }
        }

        private void ApplyLightTheme()
        {
            // Switch to light theme resources
            _window.Resources["BackgroundBrush"] = _window.Resources["LightBackgroundBrush"];
            _window.Resources["CardBackgroundBrush"] = _window.Resources["LightCardBackgroundBrush"];
            _window.Resources["CardBorderBrush"] = _window.Resources["LightCardBorderBrush"];
            _window.Resources["TextPrimaryBrush"] = _window.Resources["LightTextPrimaryBrush"];
            _window.Resources["TextSecondaryBrush"] = _window.Resources["LightTextSecondaryBrush"];
            _window.Resources["TextTertiaryBrush"] = _window.Resources["LightTextTertiaryBrush"];
            _window.Resources["InputBackgroundBrush"] = _window.Resources["LightInputBackgroundBrush"];
            _window.Resources["InputBorderBrush"] = _window.Resources["LightInputBorderBrush"];
            _window.Resources["ResultBackgroundBrush"] = _window.Resources["LightResultBackgroundBrush"];
            _window.Resources["ResultBorderBrush"] = _window.Resources["LightResultBorderBrush"];
            _window.Resources["ResultTextBrush"] = _window.Resources["LightResultTextBrush"];
            _window.Resources["ResultLabelBrush"] = _window.Resources["LightResultLabelBrush"];
            _window.Resources["StatusBackgroundBrush"] = _window.Resources["LightStatusBackgroundBrush"];
            _window.Resources["StatusBorderBrush"] = _window.Resources["LightStatusBorderBrush"];
            _window.Resources["StatusTextBrush"] = _window.Resources["LightStatusTextBrush"];
            
            // Update window and grid backgrounds
            _window.Background = new SolidColorBrush(Color.FromRgb(0xF8, 0xF9, 0xFA));
            if (_window.Content is System.Windows.Controls.Grid mainGrid)
            {
                mainGrid.Background = new SolidColorBrush(Color.FromRgb(0xF8, 0xF9, 0xFA));
            }
        }
    }
}
