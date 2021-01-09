namespace Poster.Http.Clients
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Abstract;
    using Exceptions;
    using Serializers.Abstract;

    public class PosterHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IContentSerializer _contentSerializer;

        public PosterHttpClient(IHttpClientFactory httpClientFactory, IContentSerializer contentSerializer)
        {
            _contentSerializer = contentSerializer;
            _httpClient = httpClientFactory.CreateClient("test");
        }

        public async Task<TResult> GetAsync<TResult>(string url) where TResult : class
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Url should not be empty or null", nameof(url));

            var httpResponseMessage = await _httpClient.GetAsync(url);

            return await DeserializeResponseMessageAsync<TResult>(httpResponseMessage);
        }

        public async Task<TResult> PostAsync<TResult>(string url, object body) where TResult : class
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Url should not be empty or null", nameof(url));

            var httpResponseMessage = await _httpClient.PostAsync(url, new ByteArrayContent(_contentSerializer.Serialize(body)));

            return await DeserializeResponseMessageAsync<TResult>(httpResponseMessage);
        }

        private async Task<TResult> DeserializeResponseMessageAsync<TResult>(HttpResponseMessage responseMessage) where TResult : class
        {
            var httpContent = responseMessage.EnsureSuccessStatusCode().Content;
            var content = await httpContent.ReadAsStringAsync();

            if (content == null)
                throw new ContentNullException();

            var result = _contentSerializer.Deserialize<TResult>(content);

            if (result == null)
                throw new ContentNullException();

            return result;
        }
    }
}