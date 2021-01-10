namespace Poster.Core.Reflection
{
    using System.Net.Http;
    using Abstract;
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
        public MockDynamicTypeBuilder(IHttpClientFactory httpClientFactory, IContentSerializer contentSerializer)
        {
            _methodReplacer = new MethodReplacer(httpClientFactory, contentSerializer);
        }

        /// <inheritdoc/>
        public T GetInstance<T>()
            where T : class
        {
            var mock = new Mock<T>();

            _methodReplacer.ReplaceMethodsBodies(typeof(T).GetMethods(), mock);

            return mock.Object;
        }
    }
}