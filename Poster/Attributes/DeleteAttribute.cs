namespace Poster.Core.Attributes
{
    using System;
    using Abstract;

    [AttributeUsage(AttributeTargets.Method)]
    public class DeleteAttribute : HttpAttribute
    {
        public DeleteAttribute(string url) : base(url)
        {
        }
    }
}