
namespace Order.Domain.Entities
{
using Enums;

    public class Order : EntityWithOwner
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

        public string Address { get; set; } = string.Empty;

        public decimal Cost { get; set; }

        public OrderStatus Status { get; set; }

        public bool IsClosed { get; set; }
    }
}
