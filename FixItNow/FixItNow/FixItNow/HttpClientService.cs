using System.Net.Http;

namespace FixItNow
{
    public class HttpClientService : IHttpClientService
    {
        public HttpClient GetHttpClient()
        {
            // Create and return a new HttpClient instance
            return new HttpClient();
        }
    }
}
