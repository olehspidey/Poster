namespace Poster.Http.Url
{
    using System.Collections.Generic;
    using Abstract;
    using Models;

    /// <summary>
    /// Represents url builder that builds url with parameters.
    /// </summary>
    public class UrlBuilder : IUrlBuilder
    {
        /// <inheritdoc/>
        public string BuildUrl(string url, IEnumerable<UrlParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                url = url.Replace($"{{{parameter.Name}}}", parameter.Value.ToString());
            }

            return url;
        }
    }
}