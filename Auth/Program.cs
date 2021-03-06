using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Auth;
using Auth.Data;
using Auth.IdentityServices.AspNetIdentity;
using Auth.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder();

RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();

try
{
    await SeedDatabaseAsync();

    ConfigurePipeline();

    Log.Information("Starting host...");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("IdentityServer");

    var migrationsAssembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name;

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
        .CreateLogger();

    services.AddSingleton(Log.Logger);

    services.AddControllersWithViews();

    services.AddDatabaseDeveloperPageExceptionFilter();

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    services.AddIdentity<User, Role>()
        .AddRoles<Role>()
        .AddUserManager<UserManager>()
        .AddRoleManager<RoleManager>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;

        // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
        options.EmitStaticAudienceClaim = true;
    })
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = optionsBuilder => optionsBuilder.UseSqlServer(
            connectionString,
            opt => opt.MigrationsAssembly(migrationsAssembly)
        );
        options.DefaultSchema = "is4";
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = optionsBuilder => optionsBuilder.UseSqlServer(
            connectionString,
            opt => opt.MigrationsAssembly(migrationsAssembly)
        );
        options.DefaultSchema = "is4";
    })
    .AddAspNetIdentity<User>()
    .AddDeveloperSigningCredential();

    services.AddAuthentication()
        .AddGoogle(options =>
        {
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            // register your IdentityServer with Google at https://console.developers.google.com
            // enable the Google+ API
            // set the redirect URI to https://localhost:5001/signin-google
            options.ClientId = "copy client ID from Google here";
            options.ClientSecret = "copy client secret from Google here";
        });
}

async Task SeedDatabaseAsync()
{
    Log.Information("Seeding database...");

    await SeedData.EnsureSeedDataAsync(app.Services);

    Log.Information("Done seeding database.");
}

void ConfigurePipeline()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseDeveloperExceptionPage();
    }

    app.UseStaticFiles();

    app.UseRouting();
    app.UseIdentityServer();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapDefaultControllerRoute();
    });

}
