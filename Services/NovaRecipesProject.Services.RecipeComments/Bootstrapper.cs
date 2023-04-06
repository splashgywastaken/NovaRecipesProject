using Microsoft.Extensions.DependencyInjection;

namespace NovaRecipesProject.Services.RecipeComments;

public static class Bootstrapper
{
    public static IServiceCollection AddIngredientService(this IServiceCollection services)
    {
        services.AddSingleton<IIngredientService, IngredientService>();

        return services;
    }
}