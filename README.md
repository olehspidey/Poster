### Description
This library helps to build a HTTP service very quickly with embedded serialization and deserialization. You just need create interface, mark HTTP methods with attributes (GET, POST, DELETE etc.) and Poster will create your interface implementation for you.
### HTTP service implementation example
```csharp
    public interface IOrderService
    {
        [Get("https://test.com/order/{id}")]
        public Task<Order> GetOrderAsync(int id);

        [Post("https://test.com/order")]
        public Task CreateOrder([Body] Order order);
    }
```
###### Instrance creation
```csharp
            var poster = new PosterBuilder()
                // Here you can add build configuration
                .Build();

            var orderService = poster.BuildService<IOrderService>();
            var order = await orderService.GetOrderAsync(10);
```
