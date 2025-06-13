using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Reflection;

namespace CustomGadgetApp
{
    public partial class SettingsWindow : Window
    {
        private MainWindow _mainWindow;

        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            // Load current font size from settings.ini
            string iniPath = "settings.ini";
            if (File.Exists(iniPath))
            {
                var lines = File.ReadAllLines(iniPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("FontSize"))
                    {
                        if (double.TryParse(line.Split('=')[1].Trim(), out double size))
                        {
                            FontSizeSlider.Value = size;
                            _mainWindow.UpdateClockFontSize(size);
                        }
                    }
                }
            }
        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _mainWindow.UpdateClockFontSize(e.NewValue);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string iniPath = "settings.ini";
            string[] lines = File.Exists(iniPath) ? File.ReadAllLines(iniPath) : new string[0];
            using (StreamWriter writer = new StreamWriter(iniPath))
            {
                bool fontSizeWritten = false;
                foreach (var line in lines)
                {
                    if (line.StartsWith("FontSize"))
                    {
                        writer.WriteLine($"FontSize = {FontSizeSlider.Value}");
                        fontSizeWritten = true;
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                }
                if (!fontSizeWritten)
                {
                    writer.WriteLine($"FontSize = {FontSizeSlider.Value}");
                }
            }
            this.Close();
        }
    }
}
