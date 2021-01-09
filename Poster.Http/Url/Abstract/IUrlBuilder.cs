namespace Poster.Http.Url.Abstract
{
    using System.Collections.Generic;
    using Models;

    public interface IUrlBuilder
    {
        string BuildUrl(string url, IEnumerable<Parameter> parameters);
    }
}