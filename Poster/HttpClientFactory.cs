namespace Poster
{
    using System.Net.Http;

    /// <summary>
    /// Represents default class for <see cref="IHttpClientFactory"/>.
    /// </summary>
    internal class HttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// Crates new <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="name"><see cref="HttpClient"/> name.</param>
        /// <returns>New instance of <see cref="HttpClient"/>.</returns>
        public HttpClient CreateClient(string name)
        {
            return new();
        }
    }
}