using FastEndpoints;
using Shipping.Domain;

namespace Shipping.Features.Shipping.GetShippingDetails;

public class GetShippingDetailsRequest
{
    public Guid Id { get; set; }
}

public class Validator : Validator<GetShippingDetailsRequest>
{
    public Validator()
    {

    }
}

public class ShippingDetailsResponse
{
    public ShippingDetails ResponseData { get; set; }
    public string Message => "This endpoint hasn't been implemented yet!";
}

public class ShippingDetails
{
    public Guid Id { get; set; }

    public ShippingStatus Status { get; set; }

    public string Destination { get; set; } = string.Empty;

    public string StartCountry { get; set; } = string.Empty;

    public IEnumerable<string> History { get; set; } = Enumerable.Empty<string>();
}