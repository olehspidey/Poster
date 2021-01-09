namespace Poster.Core
{
    using System;
    using System.Net.Http;
    using Http.Serializers;
    using Http.Serializers.Abstract;

    public class PosterBuilder
    {
        private IHttpClientFactory? _httpClientFactory;
        private IContentSerializer? _contentSerializer;

        public PosterBuilder AddHttpClientFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            
            return this;
        }

        public PosterBuilder AddContentSerializer(IContentSerializer contentSerializer)
        {
            _contentSerializer = contentSerializer ?? throw new ArgumentNullException(nameof(contentSerializer));
            
            return this;
        }

        public Poster Build()
        {
            return new(_httpClientFactory ?? new HttpClientFactory(), _contentSerializer ?? new JsonContentSerializer());
        }
    }
}