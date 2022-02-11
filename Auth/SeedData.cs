using Auth.Data;
using Auth.Models;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.IdentityServices.AspNetIdentity;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth
{
    public class SeedData
    {
        public static async Task EnsureSeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();

            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var dropOnMigrate = configuration.GetValue<bool>("DropOnMigrate");

            await PrepareDbContextsAsync(dropOnMigrate, applicationDbContext, configurationDbContext, persistedGrantDbContext);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager>();

            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        private static async Task SeedRoles(RoleManager roleManager)
        {
            var results = new List<IdentityResult>
            {
                await roleManager.CreateIfNotExistAsync("admin"),
                await roleManager.CreateIfNotExistAsync("vendor"),
                await roleManager.CreateIfNotExistAsync("buyer")
            };

            if (results.Any(x => !x.Succeeded))
            {
                throw new InvalidOperationException("Could not create new role");
            }
        }

        private static async Task SeedUsers(UserManager userManager)
        {
            IdentityResult? result = null;

            #region Create admin user

            var adminUser = new User
            {
                UserName = "admin123",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };

            var adminClaims = new List<Claim>
            {
                new(JwtClaimTypes.Name, "Firstname Lastname"),
                new(JwtClaimTypes.GivenName, "Firstname")
            };

            result = await userManager.CreateIfNotExistAsync(adminUser, "123Qwerty_", adminClaims);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Could not create admin user");
            }

            result = await userManager.AddToRoleAsync(adminUser, "admin");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Could not add admin user to role");
            }

            #endregion

            #region Create vendor user

            var vendorUser = new User
            {
                UserName = "vendor123",
                Email = "vendor@gmail.com",
                EmailConfirmed = true,
            };

            var vendorClaims = new List<Claim>
            {
                new(JwtClaimTypes.Name, "Firstname Lastname"),
                new(JwtClaimTypes.GivenName, "Firstname")
            };

            result = await userManager.CreateIfNotExistAsync(vendorUser, "123Qwerty_", vendorClaims);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Could not create vendor user");
            }

            result = await userManager.AddToRoleAsync(vendorUser, "vendor");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Could not add vendor user to role");
            }

            #endregion

            #region Create buyer user

            var buyerUser = new User
            {
                UserName = "buyer123",
                Email = "buyer@gmail.com",
                EmailConfirmed = true,
            };

            var buyerClaims = new List<Claim>
            {
                new(JwtClaimTypes.Name, "Firstname Lastname"),
                new(JwtClaimTypes.GivenName, "Firstname")
            };

            result = await userManager.CreateIfNotExistAsync(buyerUser, "123Qwerty_", buyerClaims);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Could not create buyer user");
            }

            result = await userManager.AddToRoleAsync(buyerUser, "buyer");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Could not add buyer user to role");
            }

            #endregion
        }

        private static async Task PrepareDbContextsAsync(bool dropOnMigrate, params DbContext[] contexts)
        {
            if (dropOnMigrate)
            {
                foreach (var dbContext in contexts)
                {
                    await dbContext.Database.EnsureDeletedAsync();
                }
            }

            foreach (var dbContext in contexts)
            {
                await dbContext.Database.MigrateAsync();
            }
        }
    }
}
