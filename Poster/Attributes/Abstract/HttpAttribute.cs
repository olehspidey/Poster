namespace Poster.Attributes.Abstract
{
    using System;

    /// <summary>
    /// Represents abstraction of http method.
    /// </summary>
    public abstract class HttpAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpAttribute"/> class.
        /// </summary>
        /// <param name="url">Http url where request will be sent.</param>
        internal HttpAttribute(string url)
        {
            Url = url;
        }

        /// <summary>
        /// Gets http url.
        /// </summary>
        public string Url { get; }
    }
}