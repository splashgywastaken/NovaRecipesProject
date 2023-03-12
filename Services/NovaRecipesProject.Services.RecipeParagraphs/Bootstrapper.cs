using Microsoft.Extensions.DependencyInjection;

namespace NovaRecipesProject.Services.RecipeParagraphs;

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
    public static IServiceCollection AddRecipeParagraphService(this IServiceCollection services)
    {
        services.AddSingleton<IRecipeParagraphService, RecipeParagraphService>();

        return services;
    }
}