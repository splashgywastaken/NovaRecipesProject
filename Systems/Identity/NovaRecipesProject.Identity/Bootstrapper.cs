namespace NovaRecipesProject.Worker;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class to boot up services
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Adding services to the app
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    { 
        return services;
    }
}
 



