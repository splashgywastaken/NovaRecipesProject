using NovaRecipesProject.Common.Extensions;

namespace NovaRecipesProject.Api.Configuration;

/// <summary>
/// Class to use in DI in Program
/// </summary>
public static class ControllerAndViewsConfiguration
{
    /// <summary>
    /// DI for services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppControllerAndViews(this IServiceCollection services)
    {
        services
            .AddRazorPages();

        services
            .AddControllers()
            .AddNewtonsoftJson(options => options.SerializerSettings.SetDefaultSettings())
            .AddAppValidator()
            ;

        return services;
    }

    /// <summary>
    /// DI for builder
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder UseAppControllerAndViews(this IEndpointRouteBuilder app)
    {
        app.MapRazorPages();
        app.MapControllers();

        return app;
    }
}