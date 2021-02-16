using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using weather.Core.Services;

namespace weather.Api.ControllerDataProviders
{
    public class WeatherDataProvider : IWeatherDataProvider
    {
        private const string WEATHER_INDEX_KEY = "WeatherIndex";
        private const string WEATHER_DATA_KEY = "WeatherData";
        private const int WEATHER_DATA_EXPIRATION_S_DEFAULT = 300;

        private readonly IMemoryCache _memoryCache;
        private readonly IWeatherProvider _weatherProvider;
        private readonly ILogger<WeatherDataProvider> _logger;

        public WeatherDataProvider(ILogger<WeatherDataProvider> logger,
            IWeatherProvider weatherProvider,
            IMemoryCache memoryCache)
        {
            _weatherProvider = weatherProvider;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<object> GetCurrent(double latitude, double longitude)
        {
            _logger.LogInformation($"Getting current data for {DateTime.Now.ToShortDateString()}...");

            var weatherData = await GetWeatherData(latitude, longitude);
            var current = weatherData.Current;

            _logger.LogInformation($"Done.. Result: {current}");

            return new
            {
                Location = weatherData.Coordinates.ToString(),
                Date = current.Date.ToString("ddd dd.MM."),
                Caption = $"{current.Text}",
                RealTemp = $"{current.RealTemp}",
                FeelsTemp = $"{current.FeelsTemp}",
                Humidity = current.Humidity,
                Pressure = current.Pressure,
                WindSpeed = current.WindSpeed,
                Sunrise = $"{current.Sunrise}",
                Sunset = $"{current.Sunset}",
                SunTime = $"{current.Sunrise} - {current.Sunset}",
            };
        }

        public async Task<object> GetForecast(double latitude, double longitude)
        {
            var weatherIndex = _memoryCache.GetOrCreate(WEATHER_INDEX_KEY, _ => 1);
            _logger.LogInformation($"Getting forecast data for {DateTime.Now.AddDays(weatherIndex).ToShortDateString()}...");
            
            var weatherData = await GetWeatherData(latitude, longitude);

            var day = weatherData.Daily[weatherIndex];
            _memoryCache.Set(WEATHER_INDEX_KEY, Interlocked.Increment(ref weatherIndex) >= weatherData.Daily.Length
                ? 1 : weatherIndex);

            _logger.LogInformation($"Done.. Result: {day}");

            return new
            {
                Location = weatherData.Coordinates.ToString(),
                Date = day.Date.ToString("ddd dd.MM."),
                Caption = $"{day.Text}",
                RealTemp = $"{day.RealTemp.Day}",
                FeelsTemp = $"{day.FeelsTemp.Day}",
                RealTempMin = $"{day.RealTemp.Min}",
                RealTempMax = $"{day.RealTemp.Max}",
                RealTempMinMax = $"{day.RealTemp.Min} / {day.RealTemp.Max}",
                Humidity = day.Humidity,
                Pressure = day.Pressure,
                WindSpeed = day.WindSpeed,
                Sunrise = $"{day.Sunrise}",
                Sunset = $"{day.Sunset}",
                SunTime = $"{day.Sunrise} - {day.Sunset}",
                Rain = day.Rain,
                Snow = day.Snow,
                RainAndSnow = $"{day.Rain} / {day.Snow}",
                Pop = (int)(day.Pop * 100),
            };
        }

        private async Task<Core.Entities.WeatherData> GetWeatherData(double latitude, double longitude)
        {
            return await _memoryCache.GetOrCreateAsync(WEATHER_DATA_KEY, async x =>
            {
                _logger.LogInformation($"Cache miss. Downloading...");

                var wdex = int.TryParse(Environment.GetEnvironmentVariable("WEATHER_DATA_EXPIRATION_S"), out var s)
                    ? s
                    : WEATHER_DATA_EXPIRATION_S_DEFAULT;

                x.AbsoluteExpiration = DateTime.UtcNow.AddSeconds(wdex);
                return await _weatherProvider.Get(latitude, longitude);
            });
        }
    }
}
