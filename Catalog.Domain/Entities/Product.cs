namespace Catalog.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? ShortName { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? ShortDescription { get; set; }

        public IEnumerable<Tag> Tags { get; set; } = Enumerable.Empty<Tag>();

        public Category? Category { get; set; }

        public Guid CategoryId { get; set; }
    }
}
