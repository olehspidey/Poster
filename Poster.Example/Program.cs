using System;

namespace Poster
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Attributes;

    class Program
    {
        static async Task Main(string[] args)
        {
            var poster = new Poster();
            var userService = poster.BuildService<IUserService>();
            var ts = new CancellationTokenSource();
            await userService.CreateUser(new User
            {
                Name = "as"
            });
            // var r = await userService.GetUserAsync("oleh", 1);
            
            Console.ReadLine();
        }
    }

    public interface IUserService
    {
        // [Get("http://localhost:5000/auth/test/{name}?id={id}")]
        // public Task<User> GetUserAsync(string name, int id);

        [Get("http://localhost:5000/auth/create")]
        public Task CreateUser([Body]User user);
    }

    public class User
    {
        public string Name { get; set; }
    }
}