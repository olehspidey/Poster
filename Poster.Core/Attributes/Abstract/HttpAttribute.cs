namespace Poster.Core.Attributes.Abstract
{
    using System;

    public class HttpAttribute : Attribute
    {
        public HttpAttribute(string url)
        {
            Url = url;
        }

        public string Url { get; }
    }
}