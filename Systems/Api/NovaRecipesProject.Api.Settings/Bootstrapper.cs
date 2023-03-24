namespace NovaRecipesProject.Api.Settings;

using NovaRecipesProject.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Bootstrapper to use in DI
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Adds api special settings from file to given services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddApiSpecialSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Settings.Load<ApiSpecialSettings>("ApiSpecial", configuration);
        services.AddSingleton(settings);

        return services;
    }
}