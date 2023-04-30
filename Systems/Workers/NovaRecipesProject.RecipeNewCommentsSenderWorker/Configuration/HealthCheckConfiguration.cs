using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NovaRecipesProject.Common.HealthChecks;
using NovaRecipesProject.RecipeNewCommentsSenderWorker.Configuration.HealthChecks;

namespace NovaRecipesProject.RecipeNewCommentsSenderWorker.Configuration;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<SelfHealthCheck>("NovaRecipesProject.RecipeNewCommentsSenderWorker");

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