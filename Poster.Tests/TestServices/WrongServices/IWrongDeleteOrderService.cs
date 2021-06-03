namespace Poster.Tests.TestServices.WrongServices
{
    using System.Threading.Tasks;
    using global::Poster.Attributes;
    using global::Poster.Tests.TestServices.TestModels;

    public interface IWrongDeleteOrderService
    {
        [Delete("http://localhost:5000/order")]
        Task<Order> DeleteOrderAsync([Body] Order order);
    }
}