namespace Poster.Attributes
{
    using System;
    using Abstract;

    /// <summary>
    /// Attribute that indicates method as http PATCH method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PatchAttribute : HttpAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatchAttribute"/> class.
        /// </summary>
        /// <param name="url">Http url where request will be sent.</param>
        public PatchAttribute(string url)
            : base(url)
        {
        }
    }
}