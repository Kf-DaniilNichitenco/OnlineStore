namespace Catalog.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    }
}
