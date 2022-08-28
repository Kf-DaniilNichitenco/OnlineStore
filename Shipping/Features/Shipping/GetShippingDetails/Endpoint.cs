using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Shipping.Features.TestingFeature;

namespace Shipping.Features.Shipping.GetShippingDetails;

public class Endpoint : BaseEndpoint<GetShippingDetailsRequest, ShippingDetailsResponse, Mapper>
{
    public IAuthorizationService AuthorizationService { get; set; }

    public override void Configure()
    {
        Get("/shipping/details/{id}");

        RequireAnyRoles(Constants.Roles.Admin, Constants.Roles.Buyer, Constants.Roles.Vendor);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetShippingDetailsRequest request, CancellationToken cancellationToken)
    {
        var shipping = Data.Shipping.FirstOrDefault(x => x.Id == request.Id);

        if (shipping == null)
        {
            ThrowError("NotFound");
        }

        var authResult = await AuthorizationService.AuthorizeAsync(User, shipping, new OperationAuthorizationRequirement());

        if (!authResult.Succeeded)
        {
            ThrowError("Forbidden");
        }

        var result = Map.FromEntity(shipping!);

        await SendAsync(result, cancellation: cancellationToken);
    }
}