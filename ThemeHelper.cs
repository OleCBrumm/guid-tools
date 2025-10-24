using Microsoft.Win32;

namespace IdConverter;

public static class ThemeHelper
{
    public static bool IsWindowsInDarkMode()
    {
        try
        {
            // Check Windows 10/11 theme preference
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
            {
                if (key?.GetValue("AppsUseLightTheme") is int value)
                {
                    return value == 0; // 0 means dark mode, 1 means light mode
                }
            }
        }
        catch
        {
            // If we can't read the registry, default to light theme
        }
        
        return false; // Default to light theme
    }
}
