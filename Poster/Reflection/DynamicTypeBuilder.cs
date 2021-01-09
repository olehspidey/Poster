namespace Poster.Core.Reflection
{
    using System.Net.Http;
    using Http.Serializers.Abstract;
    using Moq;

    public class DynamicTypeBuilder
    {
        private readonly MethodReplacer _methodReplacer;

        public DynamicTypeBuilder(IHttpClientFactory httpClientFactory, IContentSerializer contentSerializer)
        {
            _methodReplacer = new MethodReplacer(httpClientFactory, contentSerializer);
        }

        public T GetInstance<T>() where T : class
        {
            var mock = new Mock<T>();

            _methodReplacer.ReplaceMethodsBodies(typeof(T).GetMethods(), mock);

            return mock.Object;
        }
    }
}