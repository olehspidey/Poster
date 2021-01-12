namespace Poster.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Exceptions;
    using Http.Clients.Abstract;
    using Moq;
    using TestServices;
    using TestServices.TestModels;
    using TestServices.WrongServices;
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

        [Fact]
        public void Build_Service_Should_Throw_Exception_If_Get_Method_Contains_Body()
        {
            Assert.Throws<ServiceInitializeException>(() => new PosterBuilder()
                .Build()
                .BuildService<IWrongGetOrderService>());
        }
        
        [Fact]
        public void Build_Service_Should_Throw_Exception_If_Delete_Method_Contains_Body()
        {
            Assert.Throws<ServiceInitializeException>(() => new PosterBuilder()
                .Build()
                .BuildService<IWrongDeleteOrderService>());
        }

        [Fact]
        public void Build_Service_Should_Throw_Exception_If_Service_Contain_Method_Without_HttAttr()
        {
            Assert.Throws<ServiceInitializeException>(() => new PosterBuilder()
                .Build()
                .BuildService<IWithoutHttpAttributeOrderService>());
        }

        [Fact]
        public void Build_Service_Should_Throw_If_Service_Is_Empty()
        {
            Assert.Throws<ServiceInitializeException>(() => new PosterBuilder()
                .Build()
                .BuildService<IWithoutMethodsOrderService>());
        }
    }
}