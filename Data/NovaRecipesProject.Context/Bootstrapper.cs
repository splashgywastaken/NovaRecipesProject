using NovaRecipesProject.Context.Factories;

namespace NovaRecipesProject.Context;

using NovaRecipesProject.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Startup class for application's db context
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Register db context
    /// </summary>
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration? configuration = null)
    {
        var settings = NovaRecipesProject.Settings.Settings.Load<Settings.DbSettings>("Database", configuration);
        services.AddSingleton(settings);

        var dbInitOptionsDelegate = DbContextOptionsFactory.Configure(
            settings.ConnectionString,
            settings.Type);

        services.AddDbContextFactory<MainDbContext>(dbInitOptionsDelegate);

        return services;
    }
}