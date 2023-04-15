namespace NovaRecipesProject.Services.Cache;

using Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Static bootstrapper class for adding cache service
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Method used for adding caching to app
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration? configuration = null)
    {
        var settings = Settings.Load<CacheSettings>("Cache", configuration);
        services.AddSingleton(settings);

        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}