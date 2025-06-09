using System;
using System.Windows;
using System.Windows.Threading;

namespace CustomGadgetApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            ClockTextBlock.FontSize = Properties.Settings.Default.ClockFontSize;
            StartClock();
        }

        private void StartClock()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                ClockTextBlock.Text = DateTime.Now.ToString("HH:mm");
            };
            timer.Start();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }

        public void UpdateClockFontSize(double newSize)
        {
            ClockTextBlock.FontSize = newSize;
        }
    }
}
