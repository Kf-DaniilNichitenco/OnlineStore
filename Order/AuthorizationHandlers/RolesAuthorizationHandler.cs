using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Order.AuthorizationHandlers
{
    public class RolesAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, IEnumerable<string>>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            IEnumerable<string> roles)
        {
            if (context.User == null || roles.Any())
            {
                return Task.CompletedTask;
            }

            var userRoles = context.User.Claims.Where(c => c.Type == "role" && roles.Contains(c.Value));

            if (userRoles.Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
