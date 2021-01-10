namespace Poster.Core.Exceptions
{
    using System;

    /// <summary>
    /// Represents exception that throws when http service wrong described.
    /// </summary>
    public class ServiceInitializeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInitializeException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ServiceInitializeException(string message)
            : base(message)
        {
        }
    }
}