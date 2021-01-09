namespace Poster.Core.Attributes
{
    using System;
    using Abstract;

    [AttributeUsage(AttributeTargets.Method)]
    public class PatchAttribute : HttpAttribute
    {
        public PatchAttribute(string url) : base(url)
        {
        }
    }
}