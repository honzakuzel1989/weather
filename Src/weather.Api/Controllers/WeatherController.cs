using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using weather.Api.ControllerDataProviders;

namespace weather.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherDataProvider _weatherDataProvider;

        public WeatherController(ILogger<WeatherController> logger,
            IWeatherDataProvider weatherDataProvider)
        {
            _logger = logger;
            _weatherDataProvider = weatherDataProvider;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(";)");
        }

        [HttpGet("Current")]
        public async Task<IActionResult> GetCurrent()
        {
            var current = await _weatherDataProvider.GetCurrent();
            return new JsonResult(current);
        }

        [HttpGet("Forecast")]
        public async Task<IActionResult> GetForecast()
        {
            var forecast = await _weatherDataProvider.GetForecast();
            return new JsonResult(forecast);
        }
    }
}
