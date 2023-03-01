namespace NovaRecipesProject.Services.Recipes;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Bootsrtapper for adding new Recipe related services
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Main method which adds new service to IServiceCollection
    /// </summary>
    /// <param name="services">IServiceCollection itself</param>
    /// <returns>Modified "services" argument</returns>
    public static IServiceCollection AddRecipeService(this IServiceCollection services)
    {
        services.AddSingleton<IRecipeService, RecipeService>();

        return services;
    }
}