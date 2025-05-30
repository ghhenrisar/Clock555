using System.Windows;
using System.Windows.Input;

namespace CustomGadgetApp
{
    public partial class MainWindow : Window
    {
        private WeatherViewModel _weatherViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _weatherViewModel = new WeatherViewModel();
            DataContext = _weatherViewModel;
            _weatherViewModel.GetWeatherAsync("London");
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
