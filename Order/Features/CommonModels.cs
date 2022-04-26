namespace Order.Features;

public class ProductViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();

    public int Amount { get; set; }
}