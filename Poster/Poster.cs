namespace Poster.Core
{
    using System.Net.Http;
    using Abstraction;
    using Http.Serializers.Abstract;
    using Reflection;
    using Reflection.Abstract;

    /// <summary>
    /// Represents Poster default main class that is responsible for http services building.
    /// </summary>
    public class Poster : IPoster
    {
        private readonly ITypeBuilder _typeBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="Poster"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory.</param>
        /// <param name="contentSerializer">Content serializer.</param>
        internal Poster(IHttpClientFactory httpClientFactory, IContentSerializer contentSerializer)
        {
            _typeBuilder = new MockDynamicTypeBuilder(httpClientFactory, contentSerializer);
        }

        /// <summary>
        /// Builds http service.
        /// </summary>
        /// <typeparam name="TService">Type of http service.</typeparam>
        /// <returns>New instance of http service.</returns>
        public TService BuildService<TService>()
            where TService : class
        {
            var serviceInstance = _typeBuilder.GetInstance<TService>();

            return serviceInstance;
        }
    }
}