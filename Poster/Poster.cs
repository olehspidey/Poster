namespace Poster.Core
{
    using System.Net.Http;
    using Http.Serializers.Abstract;
    using Reflection;

    public class Poster
    {
        private readonly DynamicTypeBuilder _typeBuilder;
        
        internal Poster(IHttpClientFactory httpClientFactory, IContentSerializer contentSerializer)
        {
            _typeBuilder = new DynamicTypeBuilder(httpClientFactory, contentSerializer);
        }

        public TService BuildService<TService>() where TService : class
        {
            var serviceInstance = _typeBuilder.GetInstance<TService>();
            
            return serviceInstance;
        }
    }
}