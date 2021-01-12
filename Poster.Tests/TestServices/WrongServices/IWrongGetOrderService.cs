namespace Poster.Tests.TestServices.WrongServices
{
    using System.Threading.Tasks;
    using global::Poster.Attributes;
    using global::Poster.Tests.TestServices.TestModels;

    public interface IWrongGetOrderService
    {
        [Get("http://localhost:5000/order")]
        Task<Order> GetOrderAsync([Body] Order order);
    }
}