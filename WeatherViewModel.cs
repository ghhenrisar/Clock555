using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CustomGadgetApp
{
    public class WeatherViewModel
    {
        public string Temperature { get; set; }
        public string Description { get; set; }

        public async Task GetWeatherAsync(string city)
        {
            using var client = new HttpClient();
            var apiKey = "YOUR_API_KEY";
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
            var response = await client.GetStringAsync(url);
            var weather = JsonConvert.DeserializeObject<WeatherModel>(response);
            Temperature = $"{weather.Main.Temperature} °C";
            Description = weather.Weather[0].Description;
        }
    }
}
