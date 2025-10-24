using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using IdConverter.Properties;
using static IdConverter.ValidationExtensions;
namespace IdConverter;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ThemeManager _themeManager;
    private bool _smallIsLower = true;
    private bool _mediumIsLower = true;
    private bool _largeIsLower = true;
    private bool _fullIsLower = false;
    private Guid? _currentGuid;
    
    public MainWindow()
    {
        try
        {
            AppLogger.WriteLog("MainWindow constructor starting...");
            
            InitializeComponent();
            AppLogger.WriteLog("InitializeComponent completed");
            
            // Initialize logging
            string appFolder = AppDomain.CurrentDomain.BaseDirectory;
            string logFilePath = Path.Combine(appFolder, "app-debug.log");
            AppLogger.Initialize(logFilePath);
            AppLogger.WriteLog("AppLogger initialized");
            
            // Initialize theme manager
            _themeManager = new ThemeManager(this);
            AppLogger.WriteLog("ThemeManager initialized");
            
            AppLogger.WriteLog("MainWindow constructor completed successfully");
        }
        catch (Exception ex)
        {
            string errorMsg = $"FATAL ERROR in MainWindow constructor: {ex.Message}";
            AppLogger.WriteLog(errorMsg);
            AppLogger.WriteLog($"Stack trace: {ex.StackTrace}");
            AppLogger.WriteLog($"Debug: {errorMsg}");
            
            // Try to log the error if possible
            try
            {
                AppLogger.WriteLog(errorMsg);
            }
            catch
            {
                // If logging fails, just continue
            }
            
            // Re-throw to ensure the error is visible
            throw;
        }
    }


    protected override void OnSourceInitialized(EventArgs e)
    {
        try
        {
            AppLogger.WriteLog("OnSourceInitialized starting...");
            base.OnSourceInitialized(e);
            
            AppLogger.WriteLog("OnSourceInitialized called - starting theme initialization");
            
            // Log simple config information
            AppLogger.WriteLog("Using simple config system");
            
            // Load saved theme from settings
            _themeManager.LoadSavedTheme();
            AppLogger.WriteLog("Theme loaded from settings");
            
            _themeManager.ApplyTheme();
            AppLogger.WriteLog("Theme applied");
            
            AppLogger.WriteLog("Theme initialization completed");
            AppLogger.WriteLog("OnSourceInitialized completed successfully");
        }
        catch (Exception ex)
        {
            string errorMsg = $"FATAL ERROR in OnSourceInitialized: {ex.Message}";
            AppLogger.WriteLog(errorMsg);
            AppLogger.WriteLog($"Stack trace: {ex.StackTrace}");
            AppLogger.WriteLog($"Debug: {errorMsg}");
            
            try
            {
                AppLogger.WriteLog(errorMsg);
            }
            catch
            {
                // If logging fails, just continue
            }
            
            // Re-throw to ensure the error is visible
            throw;
        }
    }

    private void LoadSavedTheme()
    {
        // Load theme from simple config
        _themeManager.LoadSavedTheme();
        
        // Show in status for user feedback
        if (StatusText != null)
        {
            StatusText.Text = $"✅ Loaded theme: {_themeManager.CurrentTheme}";
        }
    }

    private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e) => ConvertId();

    private void OnGenerateGuid(object sender, RoutedEventArgs e)
    {
        var newGuid = System.Guid.NewGuid();
        InputTextBox.Text = newGuid.ToString();
        // ConvertId() will be called automatically by TextChanged event
    }

    private void OnSelectDarkTheme(object sender, RoutedEventArgs e)
    {
        if (_themeManager == null)
        {
            return;
        }

        ApplyThemeSelection("Dark");
    }

    private void OnSelectLightTheme(object sender, RoutedEventArgs e)
    {
        if (_themeManager == null)
        {
            return;
        }

        ApplyThemeSelection("Light");
    }

    private void OnSelectAutoTheme(object sender, RoutedEventArgs e)
    {
        if (_themeManager == null)
        {
            return;
        }

        ApplyThemeSelection("Auto");
    }

    private void ApplyThemeSelection(string theme)
    {
        _themeManager.SetTheme(theme);
        _themeManager.ApplyTheme();
        
        string displayName = _themeManager.GetThemeDisplayName();
        StatusText.Text = $"✅ Theme: {displayName} (saved)";
        
        InvalidateVisual();
    }

    private void ConvertId()
    {
        string input = InputTextBox.Text.Trim();
        
        if (input.IsEmpty())
        {
            ClearResults();
            return;
        }

        try
        {
            var guid = GuidProcessor.ProcessInput(input);
            
            if (guid.HasValue)
            {
                DisplayResults(guid.Value);
            }
            else
            {
                ShowError("Invalid GUID format");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Error: {ex.Message}");
        }
    }

    private void DisplayResults(Guid guid)
    {
        _currentGuid = guid;
        
        if (SmallTextBlock != null)
        {
            SmallTextBlock.Text = guid.Short(_smallIsLower);
        }
        
        if (MediumTextBlock != null)
        {
            MediumTextBlock.Text = guid.Medium(_mediumIsLower);
        }
        
        if (LargeTextBlock != null)
        {
            LargeTextBlock.Text = guid.Large(_largeIsLower);
        }
        
        if (FullTextBlock != null)
        {
            FullTextBlock.Text = guid.Full(_fullIsLower);
        }
        
        if (Base64TextBlock != null)
        {
            Base64TextBlock.Text = guid.Base64();
        }
        
        if (BinaryTextBlock != null)
        {
            BinaryTextBlock.Text = guid.Binary();
        }
        
        if (StatusText != null)
        {
            StatusText.Text = "✅";
        }
    }

    private void ClearResults()
    {
        if (SmallTextBlock != null)
        {
            SmallTextBlock.Clear();
        }
        
        if (MediumTextBlock != null)
        {
            MediumTextBlock.Clear();
        }
        
        if (LargeTextBlock != null)
        {
            LargeTextBlock.Clear();
        }
        
        if (FullTextBlock != null)
        {
            FullTextBlock.Clear();
        }
        
        if (Base64TextBlock != null)
        {
            Base64TextBlock.Clear();
        }
        
        if (BinaryTextBlock != null)
        {
            BinaryTextBlock.Clear();
        }
    }

    private void ShowError(string message)
    {
        ClearResults(); // Clear previous results first
        
        if (StatusText != null)
        {
            StatusText.Text = $"❌ {message}";
        }
    }

    private void OnCopySmall(object sender, RoutedEventArgs e) =>
        CopyToClipboard(SmallTextBlock.Text, sender as Border);

    private void OnCopyMedium(object sender, RoutedEventArgs e) =>
        CopyToClipboard(MediumTextBlock.Text, sender as Border);

    private void OnCopyLarge(object sender, RoutedEventArgs e) =>
        CopyToClipboard(LargeTextBlock.Text, sender as Border);

    private void OnCopyFull(object sender, RoutedEventArgs e) =>
        CopyToClipboard(FullTextBlock.Text, sender as Border);

    private void OnCopyBase64(object sender, RoutedEventArgs e) =>
        CopyToClipboard(Base64TextBlock.Text, sender as Border);

    private void OnCopyBinary(object sender, RoutedEventArgs e) =>
        CopyToClipboard(BinaryTextBlock.Text, sender as Border);

    private void CopyToClipboard(string text, Border border)
    {
        if (!text.IsEmpty())
        {
            Clipboard.SetText(text);
            ShowCopyFeedback(border);
        }
    }

    private void ShowCopyFeedback(Border border)
    {
        if (border != null)
        {
            // Flash effect - change background briefly
            var originalBackground = border.Background;
            border.Background = new SolidColorBrush(Color.FromRgb(40, 167, 69)); // Green flash
            
            // Reset after 200ms
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            timer.Tick += (s, args) =>
            {
                border.Background = originalBackground;
                timer.Stop();
            };
            timer.Start();
        }
    }

    private void TitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            // Double-click to maximize/restore
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
        else
        {
            // Single click to drag
            DragMove();
        }
    }

    private void OnMinimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void OnMaximize(object sender, RoutedEventArgs e) => 
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

    private void OnClose(object sender, RoutedEventArgs e) => Close();

    #region Casing Toggle Handlers

    private void OnToggleSmallCasing(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        _smallIsLower = !_smallIsLower;
        RefreshDisplay();
        e.Handled = true;
    }

    private void OnToggleMediumCasing(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        _mediumIsLower = !_mediumIsLower;
        RefreshDisplay();
        e.Handled = true;
    }

    private void OnToggleLargeCasing(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        _largeIsLower = !_largeIsLower;
        RefreshDisplay();
        e.Handled = true;
    }

    private void OnToggleFullCasing(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        _fullIsLower = !_fullIsLower;
        RefreshDisplay();
        e.Handled = true;
    }

    private void RefreshDisplay()
    {
        if (_currentGuid.HasValue)
        {
            DisplayResults(_currentGuid.Value);
        }
    }

    #endregion
}
