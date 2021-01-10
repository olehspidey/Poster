namespace Poster.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Abstract;
    using Core.Abstraction;
    using Core.Attributes;
    using Microsoft.Extensions.DependencyInjection;

    internal class DefaultServiceCollectionBuilder : IServiceCollectionBuilder
    {
        public DefaultServiceCollectionBuilder(IServiceCollection serviceCollection)
        {
            Services = serviceCollection;
        }

        public IServiceCollection Services { get; }

        public IServiceCollectionBuilder AddAllServices(Assembly assembly)
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

        public IServiceCollectionBuilder AddAllServices(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                AddAllServices(assembly);
            }

            return this;
        }
    }
}