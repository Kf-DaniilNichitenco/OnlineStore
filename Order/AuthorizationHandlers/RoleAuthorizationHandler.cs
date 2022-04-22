using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Order.AuthorizationHandlers
{
    public class RoleAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, IEnumerable<string>>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            IEnumerable<string> roles)
        {
            if (context.User == null || roles.Any())
            {
                return Task.CompletedTask;
            }

            var userRoles = context.User.Claims.Where(c => c.Type == "Role");

            var userRole = userRoles.FirstOrDefault(r => roles.Contains(r.Value));

            if(userRole != null)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
