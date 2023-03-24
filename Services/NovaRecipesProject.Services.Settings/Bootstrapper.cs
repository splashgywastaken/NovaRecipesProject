namespace NovaRecipesProject.Services.Settings;

using NovaRecipesProject.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Bootstrapper for DI
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Adds main settings as Singleton from given configuration
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddMainSettings(this IServiceCollection services, IConfiguration? configuration = null)
    {
        var settings = Settings.Load<MainSettings>("Main", configuration);
        services.AddSingleton(settings);

        return services;
    }

    /// <summary>
    /// Adds identity settings as Singleton from given configuration
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddIdentitySettings(this IServiceCollection services, IConfiguration? configuration = null)
    {
        var settings = Settings.Load<MainSettings>("Main", configuration);
        services.AddSingleton(settings);

        return services;
    }

    /// <summary>
    /// Adds swagger settings as Singleton from given configuration
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services, IConfiguration? configuration = null)
    {
        var settings = Settings.Load<SwaggerSettings>("Swagger", configuration);
        services.AddSingleton(settings);

        return services;
    }
}