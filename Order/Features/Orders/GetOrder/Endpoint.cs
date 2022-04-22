using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Order.Features.Orders.GetOrder;

public class Endpoint : Endpoint<GetOrderDetailsRequest, GetOrderDetailsResponse, Mapper>
{
    public IAuthorizationService AuthorizationService { get; set; }

    public override void Configure()
    {
        Post("/order/order/{id}");
    }

    public override async Task HandleAsync(GetOrderDetailsRequest request, CancellationToken cancellationToken)
    {
        var order = Data.Orders.First(x => x.Id == request.Id);

        var authResult = await AuthorizationService.AuthorizeAsync(User, order, new OperationAuthorizationRequirement());

        if (!authResult.Succeeded)
        {
            ThrowError("Forbidden");
        }

        var result = new GetOrderDetailsResponse
        {
            ResponseData = order
        };

        await SendAsync(result, cancellation: cancellationToken);
    }
}