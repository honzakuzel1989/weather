using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using weather.Core.Services;

namespace weather.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {

        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherProvider _forecastWeatherProvider;

        public WeatherController(ILogger<WeatherController> logger,
            IWeatherProvider forecastWeatherProvider)
        {
            _logger = logger;
            _forecastWeatherProvider = forecastWeatherProvider;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(";)");
        }

        [HttpGet("Forecast")]
        public async Task<IActionResult> GetForecast()
        {
            var weatherData = await _forecastWeatherProvider.Get();
            return new JsonResult(new
            {
                Location = weatherData.Location.Title,
                Current = new
                {
                    Caption = $"{weatherData.Current.Text}",
                    RealTemp = $"{weatherData.Current.RealTemp}",
                    FeelsTemp = $"{weatherData.Current.FeelsTemp}",
                    Humidity = weatherData.Current.Humidity,
                    Pressure = weatherData.Current.Pressure,
                    WindSpeed = weatherData.Current.WindSpeed,
                    Sunrise = $"{weatherData.Current.Sunrise}",
                    Sunset = $"{weatherData.Current.Sunset}",
                },
                Daily = weatherData.Daily.Select(day =>
                {
                    return new
                    {
                        Date = day.Date.ToString("ddd dd.MM."),
                        Caption = $"{day.Text}",
                        RealTemp = $"{day.RealTemp.Day}",
                        FeelsTemp = $"{day.FeelsTemp.Day}",
                        RealTempMin = $"{day.RealTemp.Min}",
                        RealTempMax = $"{day.RealTemp.Max}",
                        Humidity = day.Humidity,
                        Pressure = day.Pressure,
                        WindSpeed = day.WindSpeed,
                        Sunrise = $"{day.Sunrise}",
                        Sunset = $"{day.Sunset}",
                        RainAndSnow= $"{day.Rain}/{day.Snow}",
                        Probability = day.Probability
                    };
                }).ToArray()
            });
        }
    }
}
