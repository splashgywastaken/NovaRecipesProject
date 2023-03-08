namespace NovaRecipesProject.Services.Categories;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Bootsrtapper for adding new Category related services
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Main method which adds new service to IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCategoryService(this IServiceCollection services)
    {
        services.AddSingleton<ICategoryService, CategoryService>();

        return services;
    }
}