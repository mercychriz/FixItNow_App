using System.Net.Http;

namespace FixItNow
{
    public interface IHttpClientService
    {
        HttpClient GetHttpClient();
    }
}
