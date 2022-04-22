namespace Catalog.Features.Products.SearchProducts;
using Domain.Entities;
public static class Data
{
    public static IEnumerable<Product> Products => new List<Product>
    {
        new()
        {
            Id = new Guid("6E7F3F53-60F0-490F-B5EC-AF0EC5A7395A"),
            Name = "Product1",
            Description = "Description1",
            Tags = new List<Tag>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "My_Tag1"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "My_Tag2"
                },
            }
        },
        new()
        {
            Id = new Guid("77D3D0CB-3D3A-4207-86B1-F1DF347CF263"),
            Name = "Product2",
            Description = "Description2",
            Tags = new List<Tag>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "My_Tag1"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "My_Tag2"
                },
            }
        },
        new()
        {
            Id = new Guid("7E85903B-733F-43EF-83C6-E6C01D8362EE"),
            Name = "Product3",
            Description = "Description3",
        },
        new()
        {
            Id = new Guid("EA1E4E15-E493-4EF9-B3B5-3C3F466EF540"),
            Name = "Product4",
            Description = "Description4",
        }
    };
}