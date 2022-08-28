using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace Auth.IdentityServices.AspNetIdentity;

public class UserManager : UserManager<User>
{
    public UserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {

    }

    public async Task<IdentityResult> CreateIfNotExistAsync(User user, string? password = null, List<Claim>? claims = null)
    {
        var userExists = await FindByEmailAsync(user.Email) != null;

        if (userExists)
        {
            Log.Debug($"User '{user.UserName} - {user.Email}' already exists");

            return IdentityResult.Success;
        }

        var result = password == null 
            ? await CreateAsync(user)
            : await CreateAsync(user, password);

        if (result.Succeeded)
        {
            Log.Debug($"Created new user: '{user.UserName} - {user.Email}'");
        }
        else
        {
            var errors = string.Join("\n", result.Errors.Select(x => x.Description));
            Log.Debug($"Could not create new user '{user.UserName} - {user.Email}': {errors}");
        }

        if (claims == null || !claims.Any())
        {
            return result;
        }

        if (!result.Succeeded)
        {
            var errors = string.Join("\n", result.Errors.Select(x => x.Description));
            throw new InvalidOperationException($"Could not create user: {errors}");
        }

        result = await AddClaimsAsync(user, claims);

        if (!result.Succeeded)
        {
            var errors = string.Join("\n", result.Errors.Select(x => x.Description));
            Log.Debug($"Could not add claims to user '{user.UserName} - {user.Email}': {errors}");
        }

        return result;
    }
}