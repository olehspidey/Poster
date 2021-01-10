namespace Poster.Http.Models
{
    /// <summary>
    /// Represents url parameter.
    /// </summary>
    public class UrlParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlParameter"/> class.
        /// </summary>
        /// <param name="name">Url parameter name.</param>
        /// <param name="value">Url parameter value.</param>
        public UrlParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets url parameter name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets url parameter value.
        /// </summary>
        public object Value { get; }
    }
}