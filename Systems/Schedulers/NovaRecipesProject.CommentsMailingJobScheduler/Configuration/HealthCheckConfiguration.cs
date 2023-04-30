using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NovaRecipesProject.CommentsMailingJobScheduler.Configuration.HelthChecks;
using NovaRecipesProject.Common.HealthChecks;

namespace NovaRecipesProject.CommentsMailingJobScheduler.Configuration;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<SelfHealthCheck>("NovaRecipesProject.CommentsMailingJobScheduler");

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