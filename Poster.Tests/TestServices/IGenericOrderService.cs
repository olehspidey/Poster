namespace Poster.Tests.TestServices
{
    using System.Threading.Tasks;
    using Attributes;
    using TestModels;

    public interface IGenericOrderService
    {
        [Get("https://test.com/order/{id}")]
        public Task<Order> GetOrderAsync(int id);

        [Post("https://test.com/order")]
        public Task<Order> CreateOrderAsync([Body] Order order);

        [Put("https://test.com/order")]
        public Task<Order> UpdateOrderAsync([Body] UpdateOrderDto updateOrderDto);
        
        [Patch("https://test.com/order")]
        public Task<Order> UpdatePatchOrderAsync([Body] UpdateOrderDto updateOrderDto);

        [Delete("https://test.com/order/{id}")]
        public Task<Order> DeleteOrderAsync(int id);
    }
}