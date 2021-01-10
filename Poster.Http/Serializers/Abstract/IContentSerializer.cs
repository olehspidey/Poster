namespace Poster.Http.Serializers.Abstract
{
    /// <summary>
    /// Represent abstraction of http content serializer that can serialize and deserialize content.
    /// </summary>
    public interface IContentSerializer
    {
        /// <summary>
        /// Deserializes http content.
        /// </summary>
        /// <param name="httpContent">String http content.</param>
        /// <typeparam name="TResult">Type for deserialization.</typeparam>
        /// <returns>Deserialized object.</returns>
        TResult? Deserialize<TResult>(string httpContent)
            where TResult : class;

        /// <summary>
        /// Serializes http content.
        /// </summary>
        /// <param name="content">Http content object.</param>
        /// <returns>Http content bytes.</returns>
        byte[] Serialize(object content);
    }
}