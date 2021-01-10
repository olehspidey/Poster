namespace Poster.Attributes
{
    using System;

    /// <summary>
    /// Attribute that indicates parameter as request body.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class BodyAttribute : Attribute
    {
    }
}