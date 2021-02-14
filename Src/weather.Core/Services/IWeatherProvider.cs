using System.Threading.Tasks;
using weather.Core.Entities;

namespace weather.Core.Services
{
    public interface IWeatherProvider
    {
        Task<WeatherData> Get();
    }
}