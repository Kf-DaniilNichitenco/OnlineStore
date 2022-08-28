namespace Order.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Guid ExternalId { get; set; }

        public string Name { get; set; } = string.Empty;

        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();

        public int Amount { get; set; }
    }
}
