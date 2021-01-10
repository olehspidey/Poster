namespace Poster.DependencyInjection.Abstract
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Represents abstraction for <see cref="Poster"/> builder.
    /// </summary>
    public interface IDiPosterBuilder
    {
        /// <summary>
        /// Gets the application service collection.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Adds all http services from assembly to <see cref="IServiceCollection"/> which have <see cref="Attributes.HttpServiceAttribute"/>.
        /// </summary>
        /// <param name="assembly">Assembly where services will search.</param>
        /// <returns><see cref="IDiPosterBuilder"/>.</returns>
        IDiPosterBuilder AddAllServices(Assembly assembly);

        /// <summary>
        /// Adds all http services from assemblies to <see cref="IServiceCollection"/> which have <see cref="Attributes.HttpServiceAttribute"/>.
        /// </summary>
        /// <param name="assemblies">List of assemblies where services will search.</param>
        /// <returns><see cref="IDiPosterBuilder"/>.</returns>
        IDiPosterBuilder AddAllServices(IEnumerable<Assembly> assemblies);
    }
}