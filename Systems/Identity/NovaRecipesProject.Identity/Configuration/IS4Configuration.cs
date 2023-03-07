// ReSharper disable InconsistentNaming
namespace NovaRecipesProject.Identity.Configuration;

using Context;
using Context.Entities;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// IS4 setup related class
/// </summary>
public static class IS4Configuration
{
    /// <summary>
    /// Method to configure IS4
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddIS4(this IServiceCollection services)
    {
        services
            .AddIdentity<User, UserRole>(opt =>
            {
                opt.Password.RequiredLength = 0;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<MainDbContext>()
            .AddUserManager<UserManager<User>>()
            .AddDefaultTokenProviders()
            ;

        services
            .AddIdentityServer()
            
            .AddAspNetIdentity<User>()

            .AddInMemoryApiScopes(AppApiScopes.ApiScopes)
            .AddInMemoryClients(AppClients.Clients)
            .AddInMemoryApiResources(AppResources.Resources)
            .AddInMemoryIdentityResources(AppIdentityResources.Resources)

            //.AddTestUsers(AppApiTestUsers.ApiUsers)

            .AddDeveloperSigningCredential();

        return services;
    }

    /// <summary>
    /// Extension method that enables using of IS4
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseIS4(this IApplicationBuilder app)
    {
        app.UseIdentityServer();

        return app;
    }
}
