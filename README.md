[![Version](https://img.shields.io/nuget/vpre/Poster.svg)](https://www.nuget.org/packages/Poster)

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

### Supported HTTP requests
Poster support all main request types: **GET, POST, PUT, PATCH, DELETE**. You need to mark your method in service interface via `GetAttribute`, `PostAttribute`, `PutAttribute`, `PatchAttribute` or `DeleteAttribute` accordingly.
Each http attribute has constructor with *url* parameter. Service will do request to this url addres.
*Url* supports inline parameters. Parameter should be covered in bracket `{}` and method should contains this parameter as argument with the same name.
##### Method with parameters example
```csharp
        [Get("https://test.com/order/{id}")]
        Task<Order> GetOrderAsync(int id);

        [Post("https://test.com/message/{testParam}?param={param}")]
        Task<Message> CreateMessageAsync(string testParam, double param);
```
##### Method return requirements
Each method in your service should returns `Task` or `Task<TResponseModel>`.

##### Http request body
If you want to send request body, you can add it to your method as argument and mark it via `BodyAttribute`
#### Request body example
```csharp
        [Post("https://test.com/order")]
        Task CreateOrderAsync([Body] Order order);
```
*Warning*: only POST, PUT, PATCH methods support request body.

