namespace Poster.DependencyInjection
{
    using System;
    using System.Net.Http;
    using Core;
    using Core.Abstraction;
    using Http.Serializers.Abstract;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPoster(this IServiceCollection serviceCollection, Func<PosterBuilder, Poster> posterBuilderFactory)
        {
            var posterBuilder = new PosterBuilder();

            serviceCollection.AddScoped<IPoster, Poster>(provider =>
            {
                var httpClientFactory = provider.GetService<IHttpClientFactory>();
                var contentSerializer = provider.GetService<IContentSerializer>();

                if (httpClientFactory != null)
                    posterBuilder.AddHttpClientFactory(httpClientFactory);


                if (contentSerializer != null)
                    posterBuilder.AddContentSerializer(contentSerializer);

                return posterBuilderFactory(posterBuilder);
            });

            return serviceCollection;
        }

        public static IServiceCollection AddPoster(this IServiceCollection serviceCollection)
            => serviceCollection.AddScoped<IPoster, Poster>(_ => new PosterBuilder().Build());
    }
}