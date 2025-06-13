using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Timers;

namespace CustomGadgetApp
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        private string _temperature = string.Empty;
        private string _description = string.Empty;
        private string _currentTime = string.Empty;
        private System.Timers.Timer? _clockTimer;


        public string Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string CurrentTime
        {
            get => _currentTime;
            set => SetProperty(ref _currentTime, value);
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
            const string latitude = "51.5085";
            const string longitude = "-0.1257";
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m&models=ukmo_seamless&timezone=Europe%2FLondon";

            try
            {
                using var client = new HttpClient();
                var response = await client.GetStringAsync(url);
                var json = JObject.Parse(response);

                var times = json["hourly"]?["time"];
                var temps = json["hourly"]?["temperature_2m"];

                if (times != null && temps != null)
                {
                    string now = DateTime.UtcNow.AddHours(1).ToString("yyyy-MM-ddTHH:00");
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
            }
            catch (Exception ex)
            {
                Temperature = "N/A";
                Description = $"Error: {ex.Message}";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
