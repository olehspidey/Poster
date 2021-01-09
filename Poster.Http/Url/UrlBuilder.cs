namespace Poster.Http.Url
{
    using System.Collections.Generic;
    using Abstract;
    using Models;

    public class UrlBuilder : IUrlBuilder
    {
        public string BuildUrl(string url, IEnumerable<Parameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                url = url.Replace($"{{{parameter.Name}}}", parameter.Value.ToString());
            }

            return url;
        }
    }
}