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
        private readonly IGenericOrderService _testGenericOrderService;
        private readonly Mock<IHttpClient> _httpClientMock;

        public PosterTests()
        {
            _httpClientMock = new Mock<IHttpClient>();
            
            var poster = new PosterBuilder()
                .AddHttpClient(_httpClientMock.Object)
                .Build();

            _testGenericOrderService = poster.BuildService<IGenericOrderService>();
        }
        
        [Fact]
        public void Poster_Should_Create_New_Instance_Of_Service()
        {
            Assert.NotNull(_testGenericOrderService);
        }

        [Fact]
        public async Task Service_Get_Method_With_Generic_Task_Should_Returns_Value()
        {
            var order = Order.CreateRandom();

            _httpClientMock
                .Setup(client => client.GetAsync<Order>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(order));
            
            var orderFromPoster = await _testGenericOrderService.GetOrderAsync(1);
            
            Assert.Equal(order.Id, orderFromPoster.Id);
            Assert.Equal(order.Name, orderFromPoster.Name);
            Assert.Equal(order.Price, orderFromPoster.Price);
        }
        
        [Fact]
        public async Task Service_Post_Method_With_Generic_Task_Should_Returns_Value()
        {
            var order = Order.CreateRandom();

            _httpClientMock
                .Setup(client => client.PostAsync<Order>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(order));
            
            var orderFromPoster = await _testGenericOrderService.CreateOrderAsync(Order.CreateRandom());
            
            Assert.Equal(order.Id, orderFromPoster.Id);
            Assert.Equal(order.Name, orderFromPoster.Name);
            Assert.Equal(order.Price, orderFromPoster.Price);
        }
        
        [Fact]
        public async Task Service_Put_Method_With_Generic_Task_Should_Returns_Value()
        {
            var order = Order.CreateRandom();

            _httpClientMock
                .Setup(client => client.PutAsync<Order>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(order));
            
            var orderFromPoster = await _testGenericOrderService.UpdateOrderAsync(new UpdateOrderDto());
            
            Assert.Equal(order.Id, orderFromPoster.Id);
            Assert.Equal(order.Name, orderFromPoster.Name);
            Assert.Equal(order.Price, orderFromPoster.Price);
        }
        
        [Fact]
        public async Task Service_Patch_Method_With_Generic_Task_Should_Returns_Value()
        {
            var order = Order.CreateRandom();

            _httpClientMock
                .Setup(client => client.PatchAsync<Order>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(order));
            
            var orderFromPoster = await _testGenericOrderService.UpdatePatchOrderAsync(new UpdateOrderDto());
            
            Assert.Equal(order.Id, orderFromPoster.Id);
            Assert.Equal(order.Name, orderFromPoster.Name);
            Assert.Equal(order.Price, orderFromPoster.Price);
        }
        
        [InlineData(1)]
        [Theory]
        public async Task Service_Delete_Method_With_Generic_Task_Should_Returns_Value(int id)
        {
            var order = Order.CreateRandom();

            _httpClientMock
                .Setup(client => client.DeleteAsync<Order>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(order));
            
            var orderFromPoster = await _testGenericOrderService.DeleteOrderAsync(id);
            
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