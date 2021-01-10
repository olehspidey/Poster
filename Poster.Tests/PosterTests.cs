namespace Poster.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Http.Clients.Abstract;
    using Moq;
    using TestServices;
    using TestServices.TestModels;
    using Xunit;

    public class PosterTests
    {
        private readonly IOrderService _testOrderService;
        private readonly Mock<IHttpClient> _httpClientMock;

        public PosterTests()
        {
            _httpClientMock = new Mock<IHttpClient>();
            
            var poster = new PosterBuilder()
                .AddHttpClient(_httpClientMock.Object)
                .Build();

            _testOrderService = poster.BuildService<IOrderService>();
        }
        
        [Fact]
        public void Poster_Should_Create_New_Instance_Of_Service()
        {
            Assert.NotNull(_testOrderService);
        }

        [Fact]
        public async Task Service_Method_With_Generic_Task_Should_Returns_New_Order()
        {
            var order = Order.CreateRandom();

            _httpClientMock
                .Setup(client => client.GetAsync<Order>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(order));
            
            var orderFromPoster = await _testOrderService.GetOrderAsync(1);
            
            Assert.Equal(order.Id, orderFromPoster.Id);
            Assert.Equal(order.Name, orderFromPoster.Name);
            Assert.Equal(order.Price, orderFromPoster.Price);
        }
    }
}