using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Order.Domain.Entities;
using Shipping.Constants;

namespace Shipping.AuthorizationHandlers
{
    public class OwnershipsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, IEnumerable<EntityWithOwner>>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            IEnumerable<EntityWithOwner> resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (userId != null && resource.All(x => x.OwnerId.ToString() == userId.Value))
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
}
