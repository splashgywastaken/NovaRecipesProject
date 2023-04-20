using Microsoft.Extensions.DependencyInjection;

namespace NovaRecipesProject.Services.RecipeCommentsSubscriptions;

/// <summary>
/// Bootstrapper for RecipeCommentsSubscriptions Service
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Adds RecipeCommentsSubscriptionsService to IServiceCollection and all that needs to be added with it
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection AddRecipeCommentsSubscriptionsService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IRecipeCommentsSubscriptionsService, RecipeCommentsSubscriptionsService>();

        return serviceCollection;
    }
}