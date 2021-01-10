namespace Poster.Http.Url.Abstract
{
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// Represents abstraction of url builder that can build url with parameters.
    /// </summary>
    public interface IUrlBuilder
    {
        /// <summary>
        /// Build url with parameters.
        /// </summary>
        /// <param name="url">Formatted url.</param>
        /// <param name="parameters">List of url parameters.</param>
        /// <returns>New url with parameters.</returns>
        string BuildUrl(string url, IEnumerable<UrlParameter> parameters);
    }
}