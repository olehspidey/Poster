namespace Poster.Core.Attributes
{
    using System;
    using Abstract;

    [AttributeUsage(AttributeTargets.Method)]
    public class GetAttribute : HttpAttribute
    {
        public GetAttribute(string url) : base(url)
        {
        }
    }
}