namespace NovaRecipesProject.Services.Actions;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Bootstrapper for "actions" service
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Main method used to add "actions" service to IServiceCollection of an application
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddActions(this IServiceCollection services)
    {
        services.AddSingleton<IAction, Action>();

        return services;
    }
}
