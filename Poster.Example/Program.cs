using System;

namespace Poster
{
    using System.Threading.Tasks;
    using Core;
    using Core.Attributes;

    class Program
    {
        static async Task Main(string[] args)
        {
            var poster = new Poster();
            var userService = poster.BuildService<IUserService>();
            var user = await userService.GetUserAsync("oleh", 10);
            
            Console.ReadLine();
        }
    }

    public interface IUserService
    {
        [Get("http://localhost:5000/auth/test/{name}?id={id}")]
        public Task<User> GetUserAsync(string name, int id);
    }

    public class User
    {
        public string Name { get; set; }
    }
}