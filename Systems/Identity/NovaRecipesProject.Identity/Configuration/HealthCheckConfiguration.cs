namespace NovaRecipesProject.Identity.Configuration;

using Common.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

/// <summary>
/// Class for basic HealthCheck
/// </summary>
public static class HealthCheckConfiguration
{
    /// <summary>
    /// Extension method for adding HealthCheck for IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<SelfHealthCheck>("NovaRecipesProject.Identity");

        return services;
    }

    /// <summary>
    /// Extension method for adding HealthCheck for WebApplication
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