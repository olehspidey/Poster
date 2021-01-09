namespace Poster.Http.Serializers
{
    using System.Text.Json;
    using Abstract;

    public class JsonContentSerializer : IContentSerializer
    {
        public TResult? Deserialize<TResult>(string httpContent) where TResult : class
            => JsonSerializer.Deserialize<TResult>(httpContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        public byte[] Serialize(object content)
            => JsonSerializer.SerializeToUtf8Bytes(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
    }
}