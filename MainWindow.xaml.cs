using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading; // Using DispatcherTimer for UI thread updates

namespace CustomGadgetApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer? _timer;
        // Removed: private bool _isLoaded = false;
        private double _fontSize = 800; // Default Font Size set to 800

        // Fields to store normal window state for full screen toggle
        private bool _isFullScreen = true; // Default to true for full screen by default
        private double _normalLeft = 0, _normalTop = 0, _normalWidth = 600, _normalHeight = 800; // Sensible defaults for normal state


        public MainWindow()
        {
            InitializeComponent();
            // Removed: _isLoaded = true; // Set _isLoaded to true immediately after InitializeComponent()

            LoadSettings(); // Loads values into _fontSize, _isFullScreen, _normal...
            ApplyWindowAndFontSettings(); // Applies these settings to the actual window/controls
            StartClock();
        }

        private void StartClock()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) =>
            {
                if (ClockText != null) // Ensure ClockText is not null before updating
                {
                    ClockText.Text = DateTime.Now.ToString("HH:mm");
                }
            };
            _timer.Start();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Only allow dragging if the left mouse button is pressed and not in full screen mode
            if (e.LeftButton == MouseButtonState.Pressed && !_isFullScreen)
            {
                DragMove();
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings(); // Save settings before closing
            Close();
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(this, _fontSize);
            settingsWindow.Owner = this; // Set the main window as the owner
            settingsWindow.ShowDialog();
        }

        // NEW: Method to apply all window and font settings
        private void ApplyWindowAndFontSettings()
        {
            if (_isFullScreen)
            {
                this.SizeToContent = SizeToContent.Manual; // Disable dynamic sizing
                this.Left = SystemParameters.VirtualScreenLeft;
                this.Top = SystemParameters.VirtualScreenTop;
                this.Width = SystemParameters.VirtualScreenWidth;
                this.Height = SystemParameters.VirtualScreenHeight;
            }
            else
            {
                this.SizeToContent = SizeToContent.WidthAndHeight; // Enable dynamic sizing

                // Set to normal saved/default position and size
                this.Left = _normalLeft;
                this.Top = _normalTop;
                // Note: When SizeToContent is active, explicit Width/Height values
                // on the window will be overridden by content's actual size.
                // However, setting _normalWidth/_normalHeight here is still useful
                // for saving the last known normal size for the next full screen toggle.

                // Position validation/correction for normal mode
                if (Left < SystemParameters.VirtualScreenLeft || Left > SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth - Width ||
                    Top < SystemParameters.VirtualScreenTop || Top > SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight - Height)
                {
                    Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
                    Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
                    // Update _normalLeft/_normalTop if reset
                    _normalLeft = Left;
                    _normalTop = Top;
                    File.AppendAllText("log.txt", $"{DateTime.Now}: Window position defaulted or corrected in normal mode.{Environment.NewLine}");
                }
            }

            // Always apply font size after window size is set (ClockText needs to be valid)
            if (ClockText != null)
            {
                ClockText.FontSize = _fontSize;
            }
        }


        // NEW: Full screen toggle method - now calls ApplyWindowAndFontSettings
        private void ToggleFullScreen_Click(object sender, RoutedEventArgs e)
        {
            // Simply toggle the flag and re-apply settings
            _isFullScreen = !_isFullScreen;
            ApplyWindowAndFontSettings();
        }


        public void UpdateClockFontSize(double newSize)
        {
            _fontSize = newSize;
            ApplyWindowAndFontSettings(); // Re-apply settings to update font size
        }

        public void SaveSettings()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("settings.ini"))
                {
                    writer.WriteLine($"FontSize={_fontSize}");
                    
                    // Save normal window position and size (if not in full screen)
                    // If in full screen, save the last known normal position and size
                    writer.WriteLine($"Left={(_isFullScreen ? _normalLeft : Left)}");
                    writer.WriteLine($"Top={(_isFullScreen ? _normalTop : Top)}");
                    writer.WriteLine($"Width={(_isFullScreen ? _normalWidth : Width)}");
                    writer.WriteLine($"Height={(_isFullScreen ? _normalHeight : Height)}");
                    writer.WriteLine($"IsFullScreen={_isFullScreen}"); // Saving full screen state
                }
            }
            catch (Exception ex)
            {
                // Log the error if settings cannot be saved
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error saving settings — {ex.Message}{Environment.NewLine}");
            }
        }

        // LoadSettings - now only reads values into private fields
        public void LoadSettings()
        {
            // Reset defaults before loading from file, so if file is incomplete, defaults are used.
            _fontSize = 800; // Initial default font size
            _isFullScreen = true; // Initial default full screen state (as per user request for default)
            _normalLeft = 0; // Default normal position. Will be updated by ApplyWindowAndFontSettings if reset.
            _normalTop = 0;
            // _normalWidth and _normalHeight have defaults set at declaration (600, 800)

            try
            {
                if (File.Exists("settings.ini"))
                {
                    foreach (var line in File.ReadAllLines("settings.ini"))
                    {
                        var parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();

                            switch (key)
                            {
                                case "FontSize":
                                    if (double.TryParse(value, out double size))
                                    {
                                        _fontSize = size;
                                    }
                                    break;
                                case "Left":
                                    if (double.TryParse(value, out double left))
                                    {
                                        _normalLeft = left;
                                    }
                                    break;
                                case "Top":
                                    if (double.TryParse(value, out double top))
                                    {
                                        _normalTop = top;
                                    }
                                    break;
                                case "Width": // Load Width
                                    if (double.TryParse(value, out double width))
                                    {
                                        _normalWidth = width;
                                    }
                                    break;
                                case "Height": // Load Height
                                    if (double.TryParse(value, out double height))
                                    {
                                        _normalHeight = height;
                                    }
                                    break;
                                case "IsFullScreen": // Load full screen state
                                    if (bool.TryParse(value, out bool fs))
                                    {
                                        _isFullScreen = fs;
                                    }
                                    break;
                                // Backward compatibility for old settings.ini
                                case "WindowLeft":
                                    if (double.TryParse(value, out double windowLeft))
                                    {
                                        _normalLeft = windowLeft;
                                    }
                                    break;
                                case "WindowTop":
                                    if (double.TryParse(value, out double windowTop))
                                    {
                                        _normalTop = windowTop;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error loading settings — {ex.Message}{Environment.NewLine}");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop(); // Stop the timer when the window closes
            base.OnClosed(e);
        }
    }
}