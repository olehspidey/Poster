namespace Poster.Tests.TestServices
{
    using System.Threading.Tasks;
    using Attributes;
    using TestModels;

    public interface IOrderService
    {
        [Get("https://test.com/order/{id}")]
        public Task<Order> GetOrderAsync(int id);

        [Post("https://test.com/order")]
        public Task CreateOrder([Body] Order order);
    }
}