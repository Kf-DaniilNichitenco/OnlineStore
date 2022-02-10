// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Auth.Data;
using Auth.Models;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.IdentityServices.AspNetIdentity;
using IdentityServer4.EntityFramework.DbContexts;
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

            var alice = await userManager.FindByNameAsync("alice");
            if (alice == null)
            {
                alice = new User
                {
                    UserName = "alice",
                    Email = "AliceSmith@email.com",
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(alice, "Pass123$");
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = await userManager.AddClaimsAsync(alice, new Claim[]{
                    new(JwtClaimTypes.Name, "Alice Smith"),
                    new(JwtClaimTypes.GivenName, "Alice"),
                    new(JwtClaimTypes.FamilyName, "Smith"),
                    new(JwtClaimTypes.WebSite, "http://alice.com"),
                });
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("alice created");
            }
            else
            {
                Log.Debug("alice already exists");
            }

            var bob = userManager.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new User
                {
                    UserName = "bob",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = await userManager.AddClaimsAsync(bob, new Claim[]{
                    new(JwtClaimTypes.Name, "Bob Smith"),
                    new(JwtClaimTypes.GivenName, "Bob"),
                    new(JwtClaimTypes.FamilyName, "Smith"),
                    new(JwtClaimTypes.WebSite, "http://bob.com"),
                    new("location", "somewhere")
                });

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
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
