namespace Poster.Http.Serializers.Abstract
{
    public interface IContentSerializer
    {
        TResult? Deserialize<TResult>(string httpContent) where TResult : class;

        byte[] Serialize(object content);
    }
}