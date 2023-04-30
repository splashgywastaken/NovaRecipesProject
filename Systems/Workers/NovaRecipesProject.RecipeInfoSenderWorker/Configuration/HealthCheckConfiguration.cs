using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NovaRecipesProject.Common.HealthChecks;
using NovaRecipesProject.RecipeInfoSenderWorker.Configuration.HelthChecks;

namespace NovaRecipesProject.RecipeInfoSenderWorker.Configuration;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<SelfHealthCheck>("NovarecipesProject.RecipeInfoSenderWorker");

        return services;
    }

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