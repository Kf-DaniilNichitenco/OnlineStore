using Catalog.Domain.Entities;

namespace Catalog.Features.Products.SearchProducts
{
    public static class Data
    {
        public static IEnumerable<Product> Products => new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Description = "Description1",
                Tags = new List<Tag>
                {
                    new()
                    {
                        Name = "My_Tag1"
                    },
                    new()
                    {
                        Name = "My_Tag2"
                    },
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product2",
                Description = "Description2",
                Tags = new List<Tag>
                {
                    new()
                    {
                        Name = "My_Tag1"
                    },
                    new()
                    {
                        Name = "My_Tag2"
                    },
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product3",
                Description = "Description3",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product4",
                Description = "Description4",
            }
        };
    }
}