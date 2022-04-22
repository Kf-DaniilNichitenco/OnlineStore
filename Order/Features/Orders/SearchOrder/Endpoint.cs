﻿using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Order.Features.Orders.SearchOrder;

public class Endpoint : Endpoint<SearchOrderQuery, SearchOrderResultResponse, Mapper>
{
    public IAuthorizationService AuthorizationService { get; set; }

    public override void Configure()
    {
        Post("/route/path/here");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SearchOrderQuery searchOrderQuery, CancellationToken cancellationToken)
    {
        var ownerId = new Guid(searchOrderQuery.Value);

        var orders = Data.Orders.Where(x => x.Id == ownerId)
                                .Skip(searchOrderQuery.Page * searchOrderQuery.Index)
                                .Take(searchOrderQuery.Size)
                                .ToList();

        var authResult = await AuthorizationService.AuthorizeAsync(User, orders, new OperationAuthorizationRequirement());

        if (!authResult.Succeeded)
        {
            ThrowError("Forbidden");
        }

        var result = Map.FromEntity(orders);

        await SendAsync(result, cancellation: cancellationToken);
    }
}