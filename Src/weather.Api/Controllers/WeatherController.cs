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
        public async Task<IActionResult> GetCurrent(
            double latitude = 50.09704122828471, 
            double longitude = 14.364210138987014)
        {
            var current = await _weatherDataProvider.GetCurrent(latitude, longitude);
            return new JsonResult(current);
        }

        [HttpGet("Forecast")]
        public async Task<IActionResult> GetForecast(
            double latitude = 50.09704122828471,
            double longitude = 14.364210138987014)
        {
            var forecast = await _weatherDataProvider.GetForecast(latitude, longitude);
            return new JsonResult(forecast);
        }
    }
}
