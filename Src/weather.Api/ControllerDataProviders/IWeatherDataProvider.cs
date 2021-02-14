using System.Threading.Tasks;

namespace weather.Api.ControllerDataProviders
{
    public interface IWeatherDataProvider
    {
        Task<object> GetCurrent(double latitude, double longitude);
        Task<object> GetForecast(double latitude, double longitude);
    }
}