using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NovaRecipesProject.RecipeInfoSenderWorker.Configuration.HelthChecks;

public class SelfHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var assembly = Assembly.Load("NovarecipesProject.RecipeInfoSenderWorker");
        var versionNumber = assembly.GetName().Version;

        return Task.FromResult(HealthCheckResult.Healthy(description: $"Build {versionNumber}"));
    }
}