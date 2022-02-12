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
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;
using ApiScope = IdentityServer4.EntityFramework.Entities.ApiScope;
using Client = IdentityServer4.EntityFramework.Entities.Client;
using IdentityResource = IdentityServer4.EntityFramework.Entities.IdentityResource;

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

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager>();

            var dropOnMigrate = configuration.GetValue<bool>("DropOnMigrate");

            await PrepareDbContextsAsync(dropOnMigrate, applicationDbContext, configurationDbContext, persistedGrantDbContext);

            await SeedRoles(roleManager);
            await SeedUsers(userManager);

            await SeedIdentityServer(configurationDbContext);
        }

        private static async Task SeedIdentityServer(ConfigurationDbContext configurationDbContext)
        {
            if (!configurationDbContext.IdentityResources.Any())
            {
                await SeedIdentityResources(configurationDbContext);
            }

            if (!configurationDbContext.ApiResources.Any())
            {
                await SeedApiResources(configurationDbContext);
            }

            if (!configurationDbContext.ApiScopes.Any())
            {
                await SeedScopes(configurationDbContext);
            }

            if (!configurationDbContext.Clients.Any())
            {
                await SeedClients(configurationDbContext);
            }

            await configurationDbContext.SaveChangesAsync();
        }

        private static async Task SeedScopes(ConfigurationDbContext configurationDbContext)
        {
            await configurationDbContext.ApiScopes.AddRangeAsync(new List<ApiScope>
            {
                new()
                {
                    Name = "read"
                },
                new()
                {
                    Name = "write"
                },
                new()
                {
                    Name = "full"
                },
                new()
                {
                    Name = "openid"
                },
                new()
                {
                    Name = "offline_access"
                },
                new()
                {
                    Name = "profile"
                },
            });
        }

        private static async Task SeedClients(ConfigurationDbContext configurationDbContext)
        {
            var client = new Client
            {
                ClientId = "6A491EB6-99A7-4277-9884-72904DF2BA9A",
                ClientName = "Online Shop Client",
                ClientUri = "https://localhost:4200",
                AllowedGrantTypes = new List<ClientGrantType>
                {
                    new()
                    {
                        GrantType = GrantType.AuthorizationCode
                    }
                },
                RequirePkce = true,
                AllowOfflineAccess = true,
                ClientSecrets = new List<ClientSecret>
                {
                    new()
                    {
                        Value = "6FF4EC10-OnlineShop-4BFD-Client-8532-351C6D056462".ToSha256()
                    }
                },
                RequireClientSecret = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedScopes = new List<ClientScope>
                {
                    new()
                    {
                        Scope = "full"
                    },

                    new()
                    {
                        Scope = "read"
                    },

                    new()
                    {
                        Scope = "write"
                    },

                    new()
                    {
                        Scope = "openid"
                    },

                    new()
                    {
                        Scope = "offline_access"
                    },

                    new()
                    {
                        Scope = "profile"
                    }
                }
            };

            client.RedirectUris = new List<ClientRedirectUri>()
            {
                new()
                {
                    RedirectUri = "https://localhost:4200/home",
                    Client = client
                },
                new()
                {
                    RedirectUri = "https://localhost:4200/auth/signin-callback",
                    Client = client
                },
                new()
                {
                    RedirectUri = "https://localhost:4200/auth/signup-callback",
                    Client = client
                }
            };

            client.PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>()
            {
                new() {PostLogoutRedirectUri = "https://localhost:4200/auth/signout-callback", Client = client}
            };

            await configurationDbContext.Clients.AddAsync(client);

            await configurationDbContext.ClientCorsOrigins.AddAsync(new ClientCorsOrigin
            {
                Client = client,
                Origin = "https://localhost:4200"
            });
        }

        private static async Task SeedApiResources(ConfigurationDbContext configurationDbContext)
        {
            await configurationDbContext.ApiResources.AddAsync(
                new ApiResource
                {
                    Name = "Auth",
                    DisplayName = "Auth service",
                    Secrets = new List<ApiResourceSecret>
                    {
                        new()
                        {
                            Value = "A1837CD3-Auth-5340-API-4B40-Service-BE7C-55E5B5C9FAAB".ToSha256()
                        }
                    },
                    Scopes = new List<ApiResourceScope>
                    {
                        new()
                        {
                            Scope = "profile"
                        },
                        new()
                        {
                            Scope = "read"
                        },
                        new()
                        {
                            Scope = "write"
                        },
                        new()
                        {
                            Scope = "full"
                        },
                        new()
                        {
                            Scope = "openid"
                        },
                        new()
                        {
                            Scope = "offline_access"
                        }
                    }
                });
        }

        private static async Task SeedIdentityResources(ConfigurationDbContext configurationDbContext)
        {
            var resources = new IdentityResource[]
            {
                new()
                {
                    Name = "profile",
                    UserClaims = new List<IdentityResourceClaim>
                    {
                        new()
                        {
                            Type = "name"
                        },
                        new()
                        {
                            Type = "email"
                        },
                        new()
                        {
                            Type = "website"
                        }
                    },
                    DisplayName = "Your profile data"
                },
                new()
                {
                    Name = "openid",
                    UserClaims = new List<IdentityResourceClaim>
                    {
                        new()
                        {
                            Type = "sub"
                        }
                    },
                    DisplayName = "Your user identifier"
                }
            };

            await configurationDbContext.IdentityResources.AddRangeAsync(resources);
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
