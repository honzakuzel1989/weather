using System.Net.Http;

namespace weather.Core.Services
{
    public interface IHttpClientProvider
    {
        HttpClient Get();
    }
}