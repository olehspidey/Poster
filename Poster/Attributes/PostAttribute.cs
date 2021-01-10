namespace Poster.Core.Attributes
{
    using System;
    using Abstract;

    /// <summary>
    /// Attribute that indicates method as http POST method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PostAttribute : HttpAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostAttribute"/> class.
        /// </summary>
        /// <param name="url">Http url where request will be sent.</param>
        public PostAttribute(string url)
            : base(url)
        {
        }
    }
}