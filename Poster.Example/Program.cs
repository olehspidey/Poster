namespace Poster
{
    using System;
    using System.Threading.Tasks;
    using HttpServices;

    class Program
    {
        static async Task Main(string[] args)
        {
            var poster = new PosterBuilder().Build();
            var userService = poster.BuildService<IUserService>();
            var user = await userService.GetUserAsync();

            Console.WriteLine($"Name: {user.Name}");
            Console.Read();
        }
    }

    public class User
    {
        public string Name { get; set; }
    }
}