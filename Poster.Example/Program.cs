using System;

namespace Poster
{
    using System.Threading.Tasks;
    using Core;

    class Program
    {
        static async Task Main(string[] args)
        {
            var poster = new Poster();
            var userService = poster.BuildService<IUserService>();
            await userService.CreateUser();
            // var r = await userService.GetUserAsync("oleh", 1);
            
            Console.ReadLine();
        }
    }

    public interface IUserService
    {
        // [Get("http://localhost:5000/auth/test/{name}?id={id}")]
        // public Task<User> GetUserAsync(string name, int id);

        [Post("http://localhost:5000/auth/create")]
        public Task CreateUser();
    }

    public class PostAttribute : Attribute
    {
        public PostAttribute(string httpLocalhostAuthCreate)
        {
            throw new NotImplementedException();
        }
    }

    public class User
    {
        public string Name { get; set; }
    }
}