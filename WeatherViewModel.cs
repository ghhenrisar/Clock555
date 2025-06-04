using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CustomGadgetApp
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        private string _temperature;
        private string _description;
        private string _currentTime;
        private System.Timers.Timer _clockTimer;

        public string Temperature
        {
            get => _temperature;
            set { _temperature = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public string CurrentTime
        {
            get => _currentTime;
            set { _currentTime = value; OnPropertyChanged(); }
        }

        public WeatherViewModel()
        {
            StartClock();
        }

        private void StartClock()
        {
            _clockTimer = new System.Timers.Timer(1000);
            _clockTimer.Elapsed += (s, e) =>
            {
                CurrentTime = DateTime.UtcNow.AddHours(1).ToString("HH:mm"); // UK time
            };
            _clockTimer.Start();
        }

        public async Task GetWeatherAsync(string city)
        {
            var latitude = "51.5085";
            var longitude = "-0.1257";
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m&models=ukmo_seamless&timezone=Europe%2FLondon";

            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var json = JObject.Parse(response);

            var times = json["hourly"]["time"];
            var temps = json["hourly"]["temperature_2m"];

            var now = DateTime.UtcNow.AddHours(1).ToString("yyyy-MM-ddTHH:00");
            for (int i = 0; i < times.Count(); i++)
            {
                if (times[i]?.ToString() == now)
                {
                    Temperature = $"{temps[i]} °C";
                    Description = "UKMO Forecast";
                    break;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
