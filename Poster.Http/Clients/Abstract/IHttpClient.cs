namespace Poster.Http.Clients.Abstract
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents abstraction of http client.
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Execute GET http method and returns <see cref="TResult"/> result object.
        /// </summary>
        /// <param name="url">Http url.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <returns><see cref="TResult"/> result object.</returns>
        Task<TResult> GetAsync<TResult>(string url, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Execute POST http method and returns <see cref="TResult"/> result object.
        /// </summary>
        /// <param name="url">Http url.</param>
        /// <param name="body">POST method body.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <returns><see cref="TResult"/> result object.</returns>
        Task<TResult> PostAsync<TResult>(string url, object? body, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Execute PUT http method and returns <see cref="TResult"/> result object.
        /// </summary>
        /// <param name="url">Http url.</param>
        /// <param name="body">POST method body.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <returns><see cref="TResult"/> result object.</returns>
        Task<TResult> PutAsync<TResult>(string url, object? body, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Execute DELETE http method and returns <see cref="TResult"/> result object.
        /// </summary>
        /// <param name="url">Http url.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <returns><see cref="TResult"/> result object.</returns>
        Task<TResult> DeleteAsync<TResult>(string url, CancellationToken cancellationToken = default)
            where TResult : class;

        /// <summary>
        /// Execute PATCH http method and returns <see cref="TResult"/> result object.
        /// </summary>
        /// <param name="url">Http url.</param>
        /// <param name="body">POST method body.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <returns><see cref="TResult"/> result object.</returns>
        Task<TResult> PatchAsync<TResult>(string url, object? body, CancellationToken cancellationToken = default)
            where TResult : class;
    }
}