namespace Poster.Core.Extensions
{
    using System;
    using System.Reflection;
    using Http.Url.Abstract;

    public static class UrlBuilderExtensions
    {
        public static MethodInfo GetBuildUrlMethodInfo(this IUrlBuilder urlBuilder)
        {
            var buildUrlMethodInfo = urlBuilder.GetType().GetMethod(nameof(IUrlBuilder.BuildUrl));

            if (buildUrlMethodInfo == null)
                throw new ArgumentException(
                    $"Instance of {nameof(IUrlBuilder)} doesn't contain {nameof(IUrlBuilder.BuildUrl)} method",
                    nameof(urlBuilder));

            return buildUrlMethodInfo;
        }
    }
}