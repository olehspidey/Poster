using System;

namespace Poster
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Abstraction;
    using Core.Attributes;
    using DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient<IPoster>(client =>
            {
                client.DefaultRequestHeaders.Add("test", "test");
            });
            serviceCollection.AddPoster(builder => builder.Build());

            var provider = serviceCollection.BuildServiceProvider();

            var poster = provider.GetRequiredService<IPoster>();
            
            var userService = poster.BuildService<IUserService>();
            var ts = new CancellationTokenSource();
            await userService.CreateUser(new User
            {
                Name = "as"
            }, 10, ts.Token);
            // var r = await userService.GetUserAsync("oleh", 1);
            
            Console.ReadLine();
        }
    }

    public interface IUserService
    {
        // [Get("http://localhost:5000/auth/test/{name}?id={id}")]
        // public Task<User> GetUserAsync(string name, int id);

        [Post("http://localhost:5000/auth/create/{id}")]
        public Task CreateUser([Body]User user, int id, CancellationToken cancellationToken);
    }

    public class User
    {
        public string Name { get; set; }
    }
}