namespace Poster.HttpServices
{
    using System.Threading.Tasks;
    using Attributes;

    [HttpService]
    public interface IUserService
    {
        [Post("http://localhost:5000/user")]
        public Task<User> CreateUserAsync([Body]User user);

        [Get("http://localhost:5000/home")]
        public Task<User> GetUserAsync();
    }
}