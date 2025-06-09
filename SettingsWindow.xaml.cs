using System.Windows;

namespace CustomGadgetApp
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            FontSizeSlider.Value = Properties.Settings.Default.ClockFontSize;
        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Properties.Settings.Default.ClockFontSize = e.NewValue;
            Properties.Settings.Default.Save();
            ((MainWindow)Application.Current.MainWindow).UpdateClockFontSize(e.NewValue);
        }
    }
}
