namespace Poster.Core
{
    using System.Net.Http;
    using Http.Serializers;
    using Reflection;

    public class Poster
    {
        private readonly DynamicTypeBuilder _typeBuilder;

        public Poster()
        {
            _typeBuilder = new DynamicTypeBuilder(new HttpClientFactory(), new JsonContentSerializer());
        }

        public TService BuildService<TService>() where TService : class
        {
            var serviceInstance = _typeBuilder.GetInstance<TService>();
            
            return serviceInstance;
        }
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new HttpClient();
        }
    }
}