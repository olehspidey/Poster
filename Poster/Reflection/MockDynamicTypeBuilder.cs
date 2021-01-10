namespace Poster.Reflection
{
    using System.Net.Http;
    using Abstract;
    using Http.Clients.Abstract;
    using Http.Serializers.Abstract;
    using Moq;

    /// <summary>
    /// Represents dynamic type builder that works with <see cref="Mock"/>.
    /// </summary>
    public class MockDynamicTypeBuilder : ITypeBuilder
    {
        private readonly IMethodReplacer _methodReplacer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockDynamicTypeBuilder"/> class.
        /// </summary>
        /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/> that will be used for <see cref="HttpClient"/> building.</param>
        /// <param name="contentSerializer"><see cref="IContentSerializer"/> that will be used for http content serialization.</param>
        /// <param name="httpClient">Http client.</param>
        public MockDynamicTypeBuilder(
            IHttpClientFactory httpClientFactory,
            IContentSerializer contentSerializer,
            IHttpClient? httpClient = null)
        {
            _methodReplacer = new MethodReplacer(httpClientFactory, contentSerializer, httpClient);
        }

        /// <inheritdoc/>
        public T GetInstance<T>()
            where T : class
        {
            var mock = new Mock<T>();
            var methods = typeof(T).GetMethods();

            _methodReplacer.ReplaceMethodsBodies(methods, mock);

            return mock.Object;
        }
    }
}