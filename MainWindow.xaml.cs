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
        private bool _isLoaded = false;
        private double _fontSize = 32;

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            StartClock();
            _isLoaded = true;
        }

        private void StartClock()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) =>
            {
                if (ClockText != null) // Ensure ClockText is not null before updating
                {
                    ClockText.Text = DateTime.Now.ToString("HH:mm:ss");
                }
            };
            _timer.Start();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Only allow dragging if the left mouse button is pressed
            if (e.LeftButton == MouseButtonState.Pressed)
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
            settingsWindow.ShowDialog();
        }

        public void UpdateClockFontSize(double newSize)
        {
            _fontSize = newSize;
            if (_isLoaded && ClockText != null)
            {
                ClockText.FontSize = _fontSize;
            }
        }

        public void SaveSettings()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("settings.ini"))
                {
                    writer.WriteLine($"FontSize={_fontSize}");
                    writer.WriteLine($"Left={Left}");
                    writer.WriteLine($"Top={Top}");
                }
            }
            catch (Exception ex)
            {
                // Log the error if settings cannot be saved
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error saving settings — {ex.Message}{Environment.NewLine}");
            }
        }

        public void LoadSettings()
        {
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
                                case "Left": // Current key for Left
                                    if (double.TryParse(value, out double left))
                                    {
                                        Left = left;
                                    }
                                    break;
                                case "Top": // Current key for Top
                                    if (double.TryParse(value, out double top))
                                    {
                                        Top = top;
                                    }
                                    break;
                                case "WindowLeft": // Backward compatibility for old settings.ini
                                    if (double.TryParse(value, out double windowLeft))
                                    {
                                        Left = windowLeft;
                                    }
                                    break;
                                case "WindowTop": // Backward compatibility for old settings.ini
                                    if (double.TryParse(value, out double windowTop))
                                    {
                                        Top = windowTop;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error if settings cannot be loaded
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error loading settings — {ex.Message}{Environment.NewLine}");
            }

            // After loading, validate and correct window position if it's off-screen
            // This ensures the window always appears on a visible part of the screen
            if (Left < SystemParameters.VirtualScreenLeft || Left > SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth - Width ||
                Top < SystemParameters.VirtualScreenTop || Top > SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight - Height)
            {
                // Reset to a default visible position (e.g., center of primary screen)
                Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
                Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
                File.AppendAllText("log.txt", $"{DateTime.Now}: Window position reset to default as loaded position was off-screen or invalid.{Environment.NewLine}");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop(); // Stop the timer when the window closes
            base.OnClosed(e);
        }
    }
}