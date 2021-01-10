namespace Poster.Core
{
    using System;
    using System.Net.Http;
    using Http.Serializers;
    using Http.Serializers.Abstract;

    /// <summary>
    /// Represents class that is responsible for <see cref="Poster"/> building.
    /// </summary>
    public class PosterBuilder
    {
        private IHttpClientFactory? _httpClientFactory;
        private IContentSerializer? _contentSerializer;

        /// <summary>
        /// Adds <see cref="IHttpClientFactory"/> that will be used for <see cref="HttpClient"/> building.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory that will use for <see cref="HttpClient"/> building.</param>
        /// <returns><see cref="PosterBuilder"/>.</returns>
        public PosterBuilder AddHttpClientFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

            return this;
        }

        /// <summary>
        /// Adds <see cref="IContentSerializer"/> that will be used for http content serialization.
        /// </summary>
        /// <param name="contentSerializer"><see cref="IContentSerializer"/> that will be used for http content serialization.</param>
        /// <returns><see cref="PosterBuilder"/>.</returns>
        public PosterBuilder AddContentSerializer(IContentSerializer contentSerializer)
        {
            _contentSerializer = contentSerializer ?? throw new ArgumentNullException(nameof(contentSerializer));

            return this;
        }

        /// <summary>
        /// Builds new instance of <see cref="Poster"/>.
        /// </summary>
        /// <returns>New instance of <see cref="Poster"/>.</returns>
        public Poster Build()
        {
            return new(_httpClientFactory ?? new HttpClientFactory(), _contentSerializer ?? new JsonContentSerializer());
        }
    }
}