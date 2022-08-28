using Order.Domain.Entities;

namespace Shipping.Domain.Entities;

public class Shipping : EntityWithOwner
{
    public ShippingStatus Status { get; set; }

    public string Destination { get; set; } = string.Empty;

    public string StartCountry { get; set; } = string.Empty;

    public IEnumerable<string> History { get; set; } = Enumerable.Empty<string>();
}