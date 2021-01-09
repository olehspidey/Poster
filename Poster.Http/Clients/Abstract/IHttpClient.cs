namespace Poster.Http.Clients.Abstract
{
    using System.Threading.Tasks;

    public interface IHttpClient
    {
        Task<TResult> GetAsync<TResult>(string url) where TResult : class;
        
        Task<TResult> PostAsync<TResult>(string url, object body) where TResult : class;
    }
}