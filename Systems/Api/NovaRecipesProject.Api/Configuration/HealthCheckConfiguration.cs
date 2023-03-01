using NovaRecipesProject.Common.HealthChecks;

namespace NovaRecipesProject.Api.Configuration;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks;

/// <summary>
/// Class for HealthCheck configuration 
/// </summary>
public static class HealthCheckConfiguration
{
    /// <summary>
    /// Extension method to add HealthCheck related things
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<SelfHealthCheck>("YourProjectCollection.API");

        return services;
    }

    /// <summary>
    /// Extension method to enable HealthChecks in app
    /// </summary>
    /// <param name="app"></param>
    public static void UseAppHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health");

        app.MapHealthChecks("/health/detail", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckHelper.WriteHealthCheckResponse,
            AllowCachingResponses = false,
        });
    }
}