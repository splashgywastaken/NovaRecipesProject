namespace NovaRecipesProject.Services.RabbitMq;

using Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Bootstrapper class for RabbitMQ, used to add RabbitMQ to app's services
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Main method of current bootstrapper, used to add RabbitMQ to IServiceCollection of app
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration? configuration = null)
    {
        var settings = Settings.Load<RabbitMqSettings>("RabbitMq", configuration);
        services.AddSingleton(settings);

        services.AddSingleton<IRabbitMq, RabbitMq>();

        return services;
    }
}