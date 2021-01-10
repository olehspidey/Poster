namespace Poster.Attributes
{
    using System;
    using Abstract;

    /// <summary>
    /// Attribute that indicates method as http DELETE method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DeleteAttribute : HttpAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAttribute"/> class.
        /// </summary>
        /// <param name="url">Http url where request will be sent.</param>
        public DeleteAttribute(string url)
            : base(url)
        {
        }
    }
}