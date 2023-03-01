namespace NovaRecipesProject.Api.Configuration.HealthChecks; 

using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

/// <inheritdoc />
public class SelfHealthCheck : IHealthCheck
{
    /// <inheritdoc />
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var assembly = Assembly.Load("NovaRecipesProject.API");
        var versionNumber = assembly.GetName().Version;

        return Task.FromResult(HealthCheckResult.Healthy(description: $"Build {versionNumber}"));
    }
}