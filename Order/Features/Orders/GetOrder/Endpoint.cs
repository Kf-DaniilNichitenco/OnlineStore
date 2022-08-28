using Catalog.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Order.Features.Orders.GetOrder;

public class Endpoint : BaseEndpoint<GetOrderDetailsRequest, GetOrderDetailsResponse, Mapper>
{
    public IAuthorizationService AuthorizationService { get; set; }

    public override void Configure()
    {
        Get("/order/order-details/{id}");

        RequireAnyRoles(Constants.Roles.Buyer, Constants.Roles.Vendor , Constants.Roles.Admin);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetOrderDetailsRequest request, CancellationToken cancellationToken)
    {
        var order = Data.Orders.First(x => x.Id == request.Id);

        var authResult = await AuthorizationService.AuthorizeAsync(User, order, new OperationAuthorizationRequirement());

        if (!authResult.Succeeded)
        {
            ThrowError("Forbidden");
        }

        var result = Map.FromEntity(order);

        await SendAsync(result, cancellation: cancellationToken);
    }
}