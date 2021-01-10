namespace Poster.Core.Extensions
{
    using System;
    using System.Reflection;
    using Http.Url.Abstract;

    /// <summary>
    /// Represents class for <see cref="IUrlBuilder"/> extensions.
    /// </summary>
    public static class UrlBuilderExtensions
    {
        /// <summary>
        /// Gets BuildUrl <see cref="MethodInfo"/> from <see cref="IUrlBuilder"/> object.
        /// </summary>
        /// <param name="urlBuilder">Url builder object.</param>
        /// <returns>BuildUrl <see cref="MethodInfo"/> from <see cref="IUrlBuilder"/> object.</returns>
        /// <exception cref="ArgumentException">When <see cref="IUrlBuilder"/> instance does not have BuildUrl method.</exception>
        public static MethodInfo GetBuildUrlMethodInfo(this IUrlBuilder urlBuilder)
        {
            var buildUrlMethodInfo = urlBuilder.GetType().GetMethod(nameof(IUrlBuilder.BuildUrl));

            if (buildUrlMethodInfo == null)
            {
                throw new ArgumentException(
                    $"Instance of {nameof(IUrlBuilder)} doesn't contain {nameof(IUrlBuilder.BuildUrl)} method",
                    nameof(urlBuilder));
            }

            return buildUrlMethodInfo;
        }
    }
}