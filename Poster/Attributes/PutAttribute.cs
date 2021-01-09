namespace Poster.Core.Attributes
{
    using System;
    using Abstract;

    [AttributeUsage(AttributeTargets.Method)]
    public class PutAttribute : HttpAttribute
    {
        public PutAttribute(string url) : base(url)
        {
        }
    }
}