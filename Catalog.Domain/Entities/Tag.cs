namespace Catalog.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = "Tag";

        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    }
}
