namespace Poster.DependencyInjection.Abstract
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public interface IServiceCollectionBuilder
    {
        IServiceCollection Services { get; }
        
        IServiceCollectionBuilder AddAllServices(Assembly assembly);
    }
}