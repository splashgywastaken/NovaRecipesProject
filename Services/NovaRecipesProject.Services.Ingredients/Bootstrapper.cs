using Microsoft.Extensions.DependencyInjection;
using NovaRecipesProject.Services.Cache;

namespace NovaRecipesProject.Services.Ingredients;

/// <summary>
/// Bootsrtapper for adding new Ingredient related services
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Main method which adds new service to IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddIngredientService(this IServiceCollection services)
    {
        services.AddSingleton<IIngredientService, IngredientService>();

        return services;
    }
}