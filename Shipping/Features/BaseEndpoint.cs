using System.IdentityModel.Tokens.Jwt;
using FastEndpoints;
using FluentValidation.Results;

namespace Shipping.Features;

public abstract class BaseEndpoint<TRequest, TResponse, TMapper> : Endpoint<TRequest, TResponse, TMapper> where TRequest : notnull, new() where TResponse : notnull, new() where TMapper : notnull, IEntityMapper, new()
{
    protected void RequireAnyRoles(params string[] roles)
    {
        if (roles.Any())
        {
            PreProcessors(new RolesChecker<TRequest>(roles));
        }
    }

    protected void RequireAllRoles(params string[] roles)
    {
        if (roles.Any())
        {
            PreProcessors(new RolesChecker<TRequest>(roles, true));
        }
    }

    public override Task HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        return base.HandleAsync(request, cancellationToken);
    }
}

public class RolesChecker<TRequest> : IPreProcessor<TRequest>
{
    private readonly string[] _roles;
    private readonly bool _requireAll;

    public RolesChecker(string[] roles, bool requireAll = false)
    {
        _roles = roles;
        _requireAll = requireAll;
    }
    public Task PreProcessAsync(TRequest req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
    {
        var logger = ctx.RequestServices.GetRequiredService<ILogger<TRequest>>();

        if(!ctx.Request.Headers.TryGetValue("Authorization", out var tokenHeader))
        {
            return Task.CompletedTask;
        }

        var tokenString = tokenHeader.First().Split(' ', 2)[1];

        var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

        var userRoles = token.Claims.Where(x => x.Type == "role");

        var authPassed = _requireAll 
            ? _roles.All(role => userRoles.Any(x => x.Value == role)) 
            : _roles.Any(role => userRoles.Any(x => x.Value == role));

        if (authPassed)
        {
            logger.LogInformation($"request:{req?.GetType().FullName} path: {ctx.Request.Path}\n Roles authorization passed");

            return Task.CompletedTask;
        }

        logger.LogError($"request:{req?.GetType().FullName} path: {ctx.Request.Path}\n Roles authorization Failed");

        failures.Add(new("Authorization failed", "Role based authorization failed"));

        return ctx.Response.SendErrorsAsync(failures, 401, cancellation: ct);
    }
}