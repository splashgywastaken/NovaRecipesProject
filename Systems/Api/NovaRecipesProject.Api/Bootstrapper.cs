using NovaRecipesProject.Api.Settings;

namespace NovaRecipesProject.Api;

using NovaRecipesProject.Services.Settings;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// API services bootstrapper
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Main to register app services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddMainSettings()
            .AddIdentitySettings()
            .AddSwaggerSettings()
            .AddApiSpecialSettings()
            ;

        return services;
    }
}
