namespace Poster.Core.Exceptions
{
    using System;

    public class ServiceInitializeException : Exception
    {
        public ServiceInitializeException(string message) : base(message)
        {
            
        }
    }
}