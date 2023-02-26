namespace NovaRecipesProject.Api.Configuration.HealthChecks; 

using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class SelfHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var assembly = Assembly.Load("NovaRecipesProject.API");
        var versionNumber = assembly.GetName().Version;

        return Task.FromResult(HealthCheckResult.Healthy(description: $"Build {versionNumber}"));
    }
}