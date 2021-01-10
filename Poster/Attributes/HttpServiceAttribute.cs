namespace Poster.Core.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Interface)]
    public class HttpServiceAttribute : Attribute
    {
        public HttpServiceAttribute(string? baseUrl = null)
        {
            BaseUrl = baseUrl;
        }

        public string? BaseUrl { get; }
    }
}