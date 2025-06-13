
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Configuration;

namespace CustomGadgetApp
{
    public partial class MainWindow : Window
    {
        private readonly WeatherViewModel _viewModel = new WeatherViewModel();
        private readonly string settingsPath = "settings.ini";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;

            LoadSettings();

            Loaded += async (s, e) => await _viewModel.GetWeatherAsync("London");
        }

        private async void RefreshWeather_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.GetWeatherAsync("London");
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(this);
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }

        public void UpdateClockFontSize(double newSize)
        {
            if (ClockText != null)
            {
                ClockText.FontSize = newSize;
            }
        }

        private void LoadSettings()
        {
            if (File.Exists(settingsPath))
            {
                var lines = File.ReadAllLines(settingsPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("FontSize="))
                    {
                        if (double.TryParse(line.Substring("FontSize=".Length), out double size))
                        {
                            UpdateClockFontSize(size);
                        }
                    }
                    else if (line.StartsWith("WindowLeft="))
                    {
                        if (double.TryParse(line.Substring("WindowLeft=".Length), out double left))
                        {
                            Left = left;
                        }
                    }
                    else if (line.StartsWith("WindowTop="))
                    {
                        if (double.TryParse(line.Substring("WindowTop=".Length), out double top))
                        {
                            Top = top;
                        }
                    }
                }
            }
        }

        private void SaveSettings()
        {
            using (var writer = new StreamWriter(settingsPath))
            {
                writer.WriteLine($"FontSize={ClockText.FontSize}");
                writer.WriteLine($"WindowLeft={Left}");
                writer.WriteLine($"WindowTop={Top}");
            }
        }
    }
}
