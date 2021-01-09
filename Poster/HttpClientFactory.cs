namespace Poster.Core
{
    using System.Net.Http;

    internal class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new();
        }
    }
}