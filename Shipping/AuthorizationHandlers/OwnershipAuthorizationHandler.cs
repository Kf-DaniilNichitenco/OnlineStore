using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Order.Domain.Entities;
using Shipping.Constants;

namespace Shipping.AuthorizationHandlers;

public class OwnershipAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, EntityWithOwner>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
        EntityWithOwner resource)
    {
        if (context.User == null || resource == null)
        {
            return Task.CompletedTask;
        }

        var userId = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");

        if (userId != null && resource.OwnerId.ToString() == userId.Value)
        {
            context.Succeed(requirement);
        }

        var userRole = context.User.Claims.FirstOrDefault(c => c.Type == "role" && c.Value == Roles.Admin);

        if (userRole != null)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}