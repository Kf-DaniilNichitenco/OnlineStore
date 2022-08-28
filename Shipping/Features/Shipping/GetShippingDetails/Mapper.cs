using FastEndpoints;
using Shipping.Features.TestingFeature;

namespace Shipping.Features.Shipping.GetShippingDetails;

using Domain.Entities;

public class Mapper : Mapper<GetShippingDetailsRequest, ShippingDetailsResponse, Shipping>
{
    public override ShippingDetailsResponse FromEntity(Shipping shipping)
    {
        var data = new ShippingDetails
        {
            Id = shipping.Id,
            Status = shipping.Status,
            Destination = shipping.Destination,
            StartCountry = shipping.StartCountry,
            History = shipping.History,
        };

    var result = new ShippingDetailsResponse
        {
            ResponseData = data
        };

        return result;
    }
}