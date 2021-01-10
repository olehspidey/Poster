namespace Poster.DependencyInjection
{
    using System;
    using System.Net.Http;
    using Abstract;
    using Core;
    using Core.Abstraction;
    using Http.Serializers.Abstract;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollectionBuilder AddPoster(this IServiceCollection serviceCollection, Func<PosterBuilder, Poster> posterBuilderFactory)
        {
            var posterBuilder = new PosterBuilder();

            serviceCollection.AddSingleton<IPoster, Poster>(provider =>
            {
                var httpClientFactory = provider.GetService<IHttpClientFactory>();
                var contentSerializer = provider.GetService<IContentSerializer>();

                if (httpClientFactory != null)
                    posterBuilder.AddHttpClientFactory(httpClientFactory);


                if (contentSerializer != null)
                    posterBuilder.AddContentSerializer(contentSerializer);

                return posterBuilderFactory(posterBuilder);
            });

            return new DefaultServiceCollectionBuilder(serviceCollection);
        }

        public static IServiceCollectionBuilder AddPoster(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPoster, Poster>(_ => new PosterBuilder().Build());

            return new DefaultServiceCollectionBuilder(serviceCollection);
        }
    }
}