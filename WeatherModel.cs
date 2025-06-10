
using Newtonsoft.Json;

namespace CustomGadgetApp
{
    public class WeatherModel
    {
        [JsonProperty("name")]
        public string? City { get; set; }

        [JsonProperty("main")]
        public Main? Main { get; set; }

        [JsonProperty("weather")]
        public Weather[]? Weather { get; set; }
    }

    public class Main
    {
        [JsonProperty("temp")]
        public double Temperature { get; set; }
    }

    public class Weather
    {
        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}
