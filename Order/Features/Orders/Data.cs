namespace Order.Features.Orders;

using Domain.Enums;
using Domain.Entities;
public static class Data
{
    public static IQueryable<Order> Orders => new List<Order>
    {
        new()
        {
            Id = Guid.NewGuid(),
            Address = "Alecu Ruso",
            Cost = 1254,
            OwnerId = new Guid("92EF910B-137F-44FF-AC83-D92AD8BB5421"),
            Status = OrderStatus.PaymentAwaiting,
            Products = new List<Product>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ExternalId = new Guid("6E7F3F53-60F0-490F-B5EC-AF0EC5A7395A"),
                    Name = "Product1",
                    Amount = 1,
                    Tags = new List<string>
                    {
                        "My_Tag1",
                        "My_Tag2"
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ExternalId = new Guid("77D3D0CB-3D3A-4207-86B1-F1DF347CF263"),
                    Name = "Product2",
                    Amount = 1,
                    Tags = new List<string>
                    {
                        "My_Tag1",
                        "My_Tag2"
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ExternalId = new Guid("7E85903B-733F-43EF-83C6-E6C01D8362EE"),
                    Name = "Product3",
                    Amount = 3,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ExternalId = new Guid("EA1E4E15-E493-4EF9-B3B5-3C3F466EF540"),
                    Name = "Product4",
                    Amount = 1,
                }
            }
        }
    }.AsQueryable();
}