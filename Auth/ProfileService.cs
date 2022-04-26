using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Extensions;
using Auth.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace Auth
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                var name = context.Subject.Claims.FirstOrDefault(c => c.Type == "username");
                if (name != null && !string.IsNullOrEmpty(name.Value))
                {
                    var user = await _userManager.FindByNameAsync(name.Value);

                    if (user != null)
                    {
                        //set issued claims to return
                        context.IssuedClaims = await GetUserClaims(user);
                        context.IssuedClaims.AddRange(context.Subject.Claims.Where(x => context.IssuedClaims.All(c => c.Type != x.Type)));
                    }
                }
                else
                {
                    //get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    //where and subject was set to my user id.
                    var userId = context.Subject.GetClaim("sub");

                    if (!string.IsNullOrEmpty(userId?.Value))
                    {
                        //get user from db (find user by user id)
                        var user = await _userManager.FindByIdAsync(userId.Value);

                        // issue the claims for the user
                        if (user != null)
                        {
                            context.IssuedClaims = await GetUserClaims(user);
                            context.IssuedClaims.AddRange(context.Subject.Claims.Where(x => context.IssuedClaims.All(c => c.Type != x.Type)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                var userId = context.Subject.GetClaim("user_id") ?? context.Subject.GetClaim("sub");

                if (!string.IsNullOrEmpty(userId?.Value))
                {
                    var user = await _userManager.FindByIdAsync(userId.Value);

                    if (user != null)
                    {
                        if (user.IsActive && user.EmailConfirmed)
                        {
                            context.IsActive = true;
                        }
                        else
                        {
                            context.IsActive = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public async Task<List<Claim>> GetUserClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim("user_id", user.Id.ToString() ?? ""),
                new Claim("sub", user.Id.ToString() ?? ""),
                new Claim("username", user.UserName ?? ""),
                new Claim(JwtClaimTypes.Email, user.Email ?? "")
            };

            claims.AddRange(roles.Select(role => new Claim("role", role)));

            return claims;
        }
            
    }
}