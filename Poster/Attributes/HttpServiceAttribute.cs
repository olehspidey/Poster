namespace Poster.Core.Attributes
{
    using System;

    /// <summary>
    /// Attribute that indicates interface as http service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class HttpServiceAttribute : Attribute
    {
    }
}