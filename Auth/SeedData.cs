#nullable enable
using Auth.Data;
using Auth.Models;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Constants;
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

namespace Auth;

public class SeedData
{
    private const string DropOnMigrateKey = "DropOnMigrate";
    private const string OnlineStoreClientHostKey = "OnlineStoreClientHost";
    private const string OnlineStoreClientPortKey = "OnlineStoreClientPort";

    private const string HomeRouteKey = "home";
    private const string AuthRouteKey = "auth";
    private const string SignInCallbackRouteKey = $"{AuthRouteKey}/signin-callback";
    private const string SignUpCallbackRouteKey = $"{AuthRouteKey}/signup-callback";
    private const string SignOutCallbackRouteKey = $"{AuthRouteKey}/signout-callback";

    public static async Task EnsureSeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager>();

        var dropOnMigrate = configuration.GetValue<bool>(DropOnMigrateKey);

        await PrepareDbContextsAsync(dropOnMigrate, applicationDbContext, configurationDbContext, persistedGrantDbContext);

        await SeedRoles(roleManager);
        await SeedUsers(userManager);

        await SeedIdentityServer(configurationDbContext, configuration);
    }

    private static async Task SeedIdentityServer(ConfigurationDbContext configurationDbContext, IConfiguration configuration)
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
            await SeedClients(configurationDbContext, configuration);
        }

        await configurationDbContext.SaveChangesAsync();
    }

    private static async Task SeedScopes(ConfigurationDbContext configurationDbContext)
    {
        await configurationDbContext.ApiScopes.AddRangeAsync(new List<ApiScope>
        {
            new()
            {
                Name = Scopes.Read
            },
            new()
            {
                Name = Scopes.Write
            },
            new()
            {
                Name = Scopes.Full
            },
            new()
            {
                Name = Scopes.OfflineAccess
            }
        });
    }

    private static async Task SeedClients(ConfigurationDbContext configurationDbContext, IConfiguration configuration)
    {
        var onlineStoreClientHost = configuration.GetValue<string>(OnlineStoreClientHostKey);
        var onlineStoreClientPort = configuration.GetValue<int?>(OnlineStoreClientPortKey);

        if (onlineStoreClientHost != null)
        {
            onlineStoreClientPort ??= 4200;

            var clientUri = $"http://{onlineStoreClientHost}:{onlineStoreClientPort.Value}";

            var client = new Client
            {
                ClientId = "6A491EB6-99A7-4277-9884-72904DF2BA9A",
                ClientName = "Online Store Client",
                ClientUri = clientUri,
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
                        Value = "6FF4EC10-OnlineStore-4BFD-Client-8532-351C6D056462".ToSha256()
                    }
                },
                RequireClientSecret = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedScopes = new List<ClientScope>
                {
                    new()
                    {
                        Scope = Scopes.Full
                    },

                    new()
                    {
                        Scope = Scopes.Read
                    },

                    new()
                    {
                        Scope = Scopes.Write
                    },

                    new()
                    {
                        Scope = Scopes.OpenId
                    },

                    new()
                    {
                        Scope = Scopes.OfflineAccess
                    },

                    new()
                    {
                        Scope = Scopes.Profile
                    }
                }
            };

            client.RedirectUris = new List<ClientRedirectUri>()
            {
                new()
                {
                    RedirectUri = $"{clientUri}/{HomeRouteKey}",
                    Client = client
                },
                new()
                {
                    RedirectUri = $"{clientUri}/{SignInCallbackRouteKey}",
                    Client = client
                },
                new()
                {
                    RedirectUri = $"{clientUri}/{SignUpCallbackRouteKey}",
                    Client = client
                }
            };

            client.PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>()
            {
                new() {PostLogoutRedirectUri = $"{clientUri}/{SignOutCallbackRouteKey}", Client = client}
            };

            await configurationDbContext.Clients.AddAsync(client);

            await configurationDbContext.ClientCorsOrigins.AddAsync(new ClientCorsOrigin
            {
                Client = client,
                Origin = clientUri
            });
        }
    }

    private static async Task SeedApiResources(ConfigurationDbContext configurationDbContext)
    {
        await configurationDbContext.ApiResources.AddAsync(
            new ApiResource
            {
                Name = "AuthService",
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
                        Scope = Scopes.Profile
                    },
                    new()
                    {
                        Scope = Scopes.Read
                    },
                    new()
                    {
                        Scope = Scopes.Write
                    },
                    new()
                    {
                        Scope = Scopes.Full
                    },
                    new()
                    {
                        Scope = Scopes.OpenId
                    },
                    new()
                    {
                        Scope = Scopes.OfflineAccess
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
                Name = Scopes.Profile,
                UserClaims = new List<IdentityResourceClaim>
                {
                    new()
                    {
                        Type = Claims.Name
                    },
                    new()
                    {
                        Type = Claims.Email
                    }
                },
                DisplayName = "Your profile data"
            },
            new()
            {
                Name = Scopes.OpenId,
                UserClaims = new List<IdentityResourceClaim>
                {
                    new()
                    {
                        Type = Claims.Sub
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
            await roleManager.CreateIfNotExistAsync(Roles.Admin),
            await roleManager.CreateIfNotExistAsync(Roles.Vendor),
            await roleManager.CreateIfNotExistAsync(Roles.Buyer)
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
            throw new InvalidOperationException($"Could not create '{Roles.Admin}' user");
        }

        result = await userManager.AddToRoleAsync(adminUser, Roles.Admin);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Could not add a user to role '{Roles.Admin}'");
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
            throw new InvalidOperationException($"Could not create '{Roles.Vendor}' user");
        }

        result = await userManager.AddToRoleAsync(vendorUser, Roles.Vendor);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Could not add a user to role {Roles.Vendor}");
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
            throw new InvalidOperationException($"Could not create '{Roles.Buyer}' user");
        }

        result = await userManager.AddToRoleAsync(buyerUser, Roles.Buyer);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Could not add a user to role {Roles.Buyer}");
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