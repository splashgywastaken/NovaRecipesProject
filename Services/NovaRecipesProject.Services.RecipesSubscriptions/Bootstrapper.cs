using Microsoft.Extensions.DependencyInjection;

namespace NovaRecipesProject.Services.RecipesSubscriptions;

/// <summary>
/// Bootstrapper for DI for RecipeSubscription service
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Adds new service for recipes subscriptions to IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRecipeSubscriptionsService(this IServiceCollection services)
    {
        services.AddSingleton<IRecipeSubscriptionsService, RecipeSubscriptionsService>();

        return services;
    }
}