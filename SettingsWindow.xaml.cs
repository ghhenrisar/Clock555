using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CustomGadgetApp
{
    public partial class SettingsWindow : Window
    {
        private readonly MainWindow _mainWindow;

        public SettingsWindow(MainWindow mainWindow, double fontSize)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            FontSizeSlider.Value = fontSize;
        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _mainWindow?.UpdateClockFontSize(FontSizeSlider.Value);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _mainWindow.UpdateClockFontSize(FontSizeSlider.Value);
                _mainWindow.SaveSettings();
                this.Close();
            }
            catch (Exception ex)
            {
                // Log the error if settings cannot be saved
                File.AppendAllText("log.txt", $"{DateTime.Now}: Error saving settings — {ex.Message}{Environment.NewLine}");
            }
        }
    }
}