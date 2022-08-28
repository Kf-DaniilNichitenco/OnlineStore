using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Shipping.AuthorizationHandlers
{
    public class RoleAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            string role)
        {
            if (context.User == null || role == null)
            {
                return Task.CompletedTask;
            }

            var userRole = context.User.Claims.FirstOrDefault(c => c.Type == "role" && c.Value == role);

            if(userRole != null)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
