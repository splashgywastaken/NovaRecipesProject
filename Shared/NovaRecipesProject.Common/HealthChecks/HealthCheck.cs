// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace NovaRecipesProject.Common.HealthChecks;

/// <summary>
/// Health check DTO model, contains main info for HealthCheck setup
/// </summary>
public class HealthCheck
{
#pragma warning disable CS1591
    public string OverallStatus { get; init; } = null!;
    public IEnumerable<HealthCheckItem> HealthChecks { get; init; } = null!;
    public string TotalDuration { get; init; } = null!; 

#pragma warning restore CS1591
}

/// <summary>
/// HealthCheck item DTO model, contains data about current health of API
/// </summary>
public class HealthCheckItem
{
#pragma warning disable CS1591
    public string Status { get; init; } = null!;
    public string Component { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Duration { get; init; } = null!;
#pragma warning restore CS1591
}
