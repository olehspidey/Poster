namespace Poster.Tests.TestServices.TestModels
{
    using System;

    public class Order
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public decimal Price { get; set; }

        public static Order CreateRandom()
            => new Order
            {
                Id = new Random().Next(1, 10000),
                Name = Guid.NewGuid().ToString(),
                Price = (decimal) (new Random().NextDouble() * 1000)
            };
    }
}