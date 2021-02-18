using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using weather.Core.Entities;
using weather.Core.Entities.Generated;

namespace weather.Core.Services
{
    public class OwmWeatherProvider : IWeatherProvider
    {
        private const string URL = "https://api.openweathermap.org/data/2.5/onecall";
        private const string URL_PARAMS = "?exclude=minutely,hourly,alerts&lat={0}&lon={1}&units=metric&appid={2}";

        private readonly HttpClient _httpClient;
        private readonly ILogger<OwmWeatherProvider> _logger;

        public OwmWeatherProvider(ILogger<OwmWeatherProvider> logger,
            IHttpClientProvider httpClientProvider)
        {
            _httpClient = httpClientProvider.Get();
            _logger = logger;
        }

        public async Task<WeatherData> Get(double latitude, double longitude)
        {
            var apikey = Environment.GetEnvironmentVariable("OWM_API_KEY")
                ?? throw new InvalidOperationException("You have to provide OWM api key");

            var url = string.Format(URL + URL_PARAMS, latitude, longitude, apikey);
            var result = await _httpClient.GetStringAsync(url);

            _logger.LogInformation($"Data from {url} downloaded...");

            var o = (OwmWeather)JsonSerializer.Deserialize(result, typeof(OwmWeather));

            return new WeatherData(new Coordinates(o.lat, o.lon), 
                GetCurrentWeather(o.current), 
                GetDailyWeather(o.daily));
        }

        private DailyWeather[] GetDailyWeather(Daily[] daily)
        {
            return daily.Select(day => GetDay(day)).ToArray();
        }

        private DailyWeather GetDay(Daily day)
        {
            return new DailyWeather(
                UnixTimeStampToDateTime(day.dt),
                GetSunTime(UnixTimeStampToDateTime(day.sunrise)),
                GetSunTime(UnixTimeStampToDateTime(day.sunset)),
                GetRealTemp(day.temp),
                GetFeelsTemp(day.feels_like),
                day.pressure,
                day.humidity,
                day.wind_speed,
                day.wind_deg,
                day.snow,
                day.rain,
                GetWeatherText(day.weather.First()),
                day.pop);
        }

        private FeelsTemp GetFeelsTemp(Feels_Like feels_like)
        {
            return new FeelsTemp(feels_like.day, feels_like.night, feels_like.eve, feels_like.morn);
        }

        private RealTemp GetRealTemp(Temp temp)
        {
            return new RealTemp(temp.day, temp.min, temp.max, temp.night, temp.eve, temp.morn);
        }

        private static CurrentWeather GetCurrentWeather(Current current)
        {
            return new CurrentWeather(
                GetSunTime(UnixTimeStampToDateTime(current.sunrise)),
                GetSunTime(UnixTimeStampToDateTime(current.sunset)),
                current.temp,
                current.feels_like,
                current.pressure,
                current.humidity,
                current.wind_speed,
                current.wind_deg,
                GetWeatherText(current.weather.First()),
                UnixTimeStampToDateTime(current.dt));
        }

        private static SunTime GetSunTime(DateTime dateTime)
        {
            return new SunTime(dateTime.Hour, dateTime.Minute);
        }

        private static WeatherText GetWeatherText(Weather weather)
        {
            return new WeatherText(weather.main, weather.description);
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
