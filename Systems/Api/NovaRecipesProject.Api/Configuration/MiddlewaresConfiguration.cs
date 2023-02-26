using NovaRecipesProject.Api.Middleware;

namespace NovaRecipesProject.Api.Configuration;

/// <summary>
/// Middleware configuration class
/// </summary>
public static class MiddlewaresConfiguration
{
    /// <summary>
    /// Method for DI to use custom middlewares
    /// </summary>
    /// <param name="app"></param>
    public static void UseAppMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
    }
}