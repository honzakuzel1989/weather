using System.Threading.Tasks;

namespace weather.Api.ControllerDataProviders
{
    public interface IWeatherDataProvider
    {
        Task<object> GetCurrent();
        Task<object> GetForecast();
    }
}