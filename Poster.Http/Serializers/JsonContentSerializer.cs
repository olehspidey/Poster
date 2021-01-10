namespace Poster.Http.Serializers
{
    using System.Text.Json;
    using Abstract;

    /// <summary>
    /// Represent default implementation of <see cref="IContentSerializer"/> http content serializer.
    /// </summary>
    public class JsonContentSerializer : IContentSerializer
    {
        /// <inheritdoc/>
        public TResult? Deserialize<TResult>(string httpContent)
            where TResult : class
            => JsonSerializer.Deserialize<TResult>(httpContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

        /// <inheritdoc/>
        public byte[] Serialize(object content)
            => JsonSerializer.SerializeToUtf8Bytes(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
    }
}