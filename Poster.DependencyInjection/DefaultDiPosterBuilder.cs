namespace Poster.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Abstract;
    using Abstraction;
    using Attributes;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Represents default realization of <see cref="IDiPosterBuilder"/>.
    /// </summary>
    internal class DefaultDiPosterBuilder : IDiPosterBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDiPosterBuilder"/> class.
        /// </summary>
        /// <param name="serviceCollection">The application service collection.</param>
        public DefaultDiPosterBuilder(IServiceCollection serviceCollection)
        {
            Services = serviceCollection;
        }

        /// <inheritdoc/>
        public IServiceCollection Services { get; }

        /// <inheritdoc/>
        public IDiPosterBuilder AddAllServices(Assembly assembly)
        {
            var httServices = assembly
                .GetTypes()
                .Where(t => t.GetCustomAttribute<HttpServiceAttribute>() != null)
                .ToArray();

            foreach (var httService in httServices)
            {
                var parameterExpression = Expression.Parameter(typeof(IServiceProvider), "provider");
                var requiredServiceMethodInfo =
                    typeof(ServiceProviderServiceExtensions)
                        .GetMethods()
                        .First(m => m.Name == nameof(ServiceProviderServiceExtensions.GetRequiredService) && m.IsGenericMethod)
                        .MakeGenericMethod(typeof(IPoster));
                var callGetRequiredServiceExpression = Expression.Call(null, requiredServiceMethodInfo, parameterExpression);
                var callBuilderServiceExpression = Expression.Call(
                    callGetRequiredServiceExpression,
                    typeof(IPoster).GetMethod(nameof(IPoster.BuildService))?.MakeGenericMethod(httService) ?? throw new Exception($"Poster instance doesn't contain {nameof(IPoster.BuildService)} method"));
                var callBuildServiceLambdaExpression = Expression.Lambda(Expression.Convert(callBuilderServiceExpression, typeof(object)), parameterExpression);

                Services.AddSingleton(httService, (Func<IServiceProvider, object>)callBuildServiceLambdaExpression.Compile());
            }

            return this;
        }

        /// <inheritdoc/>
        public IDiPosterBuilder AddAllServices(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                AddAllServices(assembly);
            }

            return this;
        }
    }
}