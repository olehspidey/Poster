namespace Poster.Core.Attributes
{
    using System;
    using Abstract;

    [AttributeUsage(AttributeTargets.Method)]
    public class PostAttribute : HttpAttribute
    {
        public PostAttribute(string url) : base(url)
        {
        }
    }
}