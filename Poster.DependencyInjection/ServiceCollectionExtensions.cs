namespace Poster.DependencyInjection
{
    using System;
    using System.Net.Http;
    using Abstract;
    using Abstraction;
    using Http.Serializers.Abstract;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Represents class with <see cref="IServiceCollection"/> extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="Poster"/> object to <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The application service collection.</param>
        /// <param name="posterBuilderFactory">Factory of <see cref="PosterBuilder"/>.</param>
        /// <returns><see cref="IDiPosterBuilder"/>.</returns>
        public static IDiPosterBuilder AddPoster(this IServiceCollection serviceCollection, Func<PosterBuilder, Poster> posterBuilderFactory)
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

            return new DefaultDiPosterBuilder(serviceCollection);
        }

        /// <summary>
        /// Adds <see cref="Poster"/> object to <see cref="IServiceCollection"/> with default Poster build factory.
        /// </summary>
        /// <param name="serviceCollection">The application service collection.</param>
        /// <returns><see cref="IDiPosterBuilder"/>.</returns>
        public static IDiPosterBuilder AddPoster(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPoster, Poster>(_ => new PosterBuilder().Build());

            return new DefaultDiPosterBuilder(serviceCollection);
        }
    }
}