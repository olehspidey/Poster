namespace Poster.Attributes
{
    using System;
    using Abstract;

    /// <summary>
    /// Attribute that indicates method as http PUT method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PutAttribute : HttpAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutAttribute"/> class.
        /// </summary>
        /// <param name="url">Http url where request will be sent.</param>
        public PutAttribute(string url)
            : base(url)
        {
        }
    }
}