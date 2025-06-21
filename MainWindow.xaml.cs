using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;

namespace Clock555
{
    public partial class MainWindow : Window
    {
        // --- P/Invoke declarations for the Win32 API ---
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        
        // --- End of P/Invoke declarations ---

        private DispatcherTimer? _clockTimer;
        private DispatcherTimer? _topmostTimer;

        private double _normalFontSize = 100;
        private double _fullScreenFontSize = 800;
        private bool _isFullScreen = true;
        private double _normalLeft = 100, _normalTop = 100, _normalWidth = 600, _normalHeight = 400;

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            ApplyWindowAndFontSettings();
            StartClock();
            StartTopmostTimer();
        }

        private void StartClock()
        {
            _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _clockTimer.Tick += (s, e) => {
                if (ClockText != null) ClockText.Text = DateTime.Now.ToString("HH:mm");
            };
            _clockTimer.Start();
        }
        
        private void StartTopmostTimer()
        {
            _topmostTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
            _topmostTimer.Tick += (s, e) =>
            {
                if (!_isFullScreen)
                {
                    ForceTopmost();
                }
            };
            _topmostTimer.Start();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !_isFullScreen) DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (!_isFullScreen)
            {
                _normalLeft = Left;
                _normalTop = Top;
                _normalWidth = Width;
                _normalHeight = Height;
            }
            SaveSettings();
            Close();
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!_isFullScreen)
            {
                _normalLeft = Left;
                _normalTop = Top;
                _normalWidth = Width;
                _normalHeight = Height;
            }
            double currentFontSize = _isFullScreen ? _fullScreenFontSize : _normalFontSize;
            var settingsWindow = new SettingsWindow(this, currentFontSize) { Owner = this };
            settingsWindow.ShowDialog();
        }

        private void ApplyWindowAndFontSettings()
        {
            if (_isFullScreen)
            {
                this.Topmost = false;
                this.SizeToContent = SizeToContent.Manual;
                this.Left = SystemParameters.VirtualScreenLeft;
                this.Top = SystemParameters.VirtualScreenTop;
                this.Width = SystemParameters.VirtualScreenWidth;
                this.Height = SystemParameters.VirtualScreenHeight;
                if (ClockText != null) ClockText.FontSize = _fullScreenFontSize;
            }
            else
            {
                this.SizeToContent = SizeToContent.WidthAndHeight;
                this.Left = _normalLeft;
                this.Top = _normalTop;
                if (ClockText != null) ClockText.FontSize = _normalFontSize;
            }
        }
        
        private void ForceTopmost()
        {
            IntPtr hWnd = new WindowInteropHelper(this).Handle;
            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        private void ToggleFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (!_isFullScreen)
            {
                _normalLeft = Left;
                _normalTop = Top;
                _normalWidth = Width;
                _normalHeight = Height;
                SaveSettings();
            }
            _isFullScreen = !_isFullScreen;
            ApplyWindowAndFontSettings();
        }

        public void UpdateClockFontSize(double newSize)
        {
            if (_isFullScreen) _fullScreenFontSize = newSize;
            else _normalFontSize = newSize;
            ApplyWindowAndFontSettings();
        }

        public void SaveSettings()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("settings.ini"))
                {
                    writer.WriteLine($"NormalFontSize={_normalFontSize}");
                    writer.WriteLine($"FullScreenFontSize={_fullScreenFontSize}");
                    writer.WriteLine($"NormalLeft={_normalLeft}");
                    writer.WriteLine($"NormalTop={_normalTop}");
                    writer.WriteLine($"NormalWidth={_normalWidth}");
                    writer.WriteLine($"NormalHeight={_normalHeight}");
                    writer.WriteLine($"IsFullScreen={_isFullScreen}");
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error saving settings — {ex.Message}{Environment.NewLine}");
            }
        }

        public void LoadSettings()
        {
            // Set initial defaults
            _normalFontSize = 100;
            _fullScreenFontSize = 800;
            _isFullScreen = true;
            _normalLeft = 100;
            _normalTop = 100;
            _normalWidth = 600;
            _normalHeight = 400;
            try
            {
                if (File.Exists("settings.ini"))
                {
                    foreach (var line in File.ReadAllLines("settings.ini"))
                    {
                        var parts = line.Split('=');
                        if (parts.Length != 2) continue;
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();
                        switch (key)
                        {
                            case "FontSize": if (double.TryParse(value, out double size)) _normalFontSize = size; break;
                            case "NormalFontSize": if (double.TryParse(value, out double nfs)) _normalFontSize = nfs; break;
                            case "FullScreenFontSize": if (double.TryParse(value, out double ffs)) _fullScreenFontSize = ffs; break;
                            case "NormalLeft": if (double.TryParse(value, out double left)) _normalLeft = left; break;
                            case "NormalTop": if (double.TryParse(value, out double top)) _normalTop = top; break;
                            case "NormalWidth": if (double.TryParse(value, out double width)) _normalWidth = width; break;
                            case "NormalHeight": if (double.TryParse(value, out double height)) _normalHeight = height; break;
                            case "IsFullScreen": if (bool.TryParse(value, out bool fs)) _isFullScreen = fs; break;
                        }
                    }
                }

                // A NEW, SMARTER VALIDATION BLOCK
                // This checks if the loaded position is outside the bounds of all connected screens.
                bool isPositionInvalid = (_normalLeft > SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth - 50) ||
                                         (_normalTop > SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight - 50) ||
                                         (_normalLeft < SystemParameters.VirtualScreenLeft) ||
                                         (_normalTop < SystemParameters.VirtualScreenTop);

                if (isPositionInvalid)
                {
                    // If the position is invalid, reset it to the center of the primary screen.
                    _normalLeft = (SystemParameters.PrimaryScreenWidth - _normalWidth) / 2;
                    _normalTop = (SystemParameters.PrimaryScreenHeight - _normalHeight) / 2;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error saving settings — {ex.Message}{Environment.NewLine}");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _clockTimer?.Stop();
            _topmostTimer?.Stop();
            base.OnClosed(e);
        }
    }
}