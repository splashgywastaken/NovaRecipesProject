using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NovaRecipesProject.Common.HealthChecks;

/// <summary>
/// Extension methods for HealthChecks
/// </summary>
public static class HealthCheckHelper
{
    /// <summary>
    /// Parses data about current health of API
    /// </summary>
    /// <param name="context"></param>
    /// <param name="report"></param>
    public static async Task WriteHealthCheckResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        var response = new HealthCheck()
        {
            OverallStatus = report.Status.ToString(),
            TotalDuration = report.TotalDuration.TotalSeconds.ToString("0:0.00"),
            HealthChecks = report.Entries.Select(x => new HealthCheckItem
            {
                Status = x.Value.Status.ToString(),
                Component = x.Key,
                Description = x.Value.Description ?? "",
                Duration = x.Value.Duration.TotalSeconds.ToString("0:0.00")
            }),

        };

        await context.Response.WriteAsync(text: JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
    }
}
