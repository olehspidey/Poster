namespace Poster.Core.Attributes
{
    using System;
    using Abstract;

    /// <summary>
    /// Attribute that indicates method as http DELETE method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class GetAttribute : HttpAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAttribute"/> class.
        /// </summary>
        /// <param name="url">Http url where request will be sent.</param>
        public GetAttribute(string url)
            : base(url)
        {
        }
    }
}