namespace Poster.Tests.TestServices.WrongServices
{
    using System.Threading.Tasks;
    using TestModels;

    public interface IWithoutHttpAttributeOrderService
    {
        Task<Order> GetOrderAsync();
    }
}