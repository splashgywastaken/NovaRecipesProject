using NovaRecipesProject.Api.Settings;
using NovaRecipesProject.Services.Cache;
using NovaRecipesProject.Services.Categories;
using NovaRecipesProject.Services.Recipes;
using NovaRecipesProject.Services.UserAccount;

namespace NovaRecipesProject.Api;

using NovaRecipesProject.Services.Settings;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// API services bootstrapper
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Main to register app services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddMainSettings()
            .AddIdentitySettings()
            .AddSwaggerSettings()
            .AddApiSpecialSettings()
            .AddCache()
            // Controller-related services
            .AddRecipeService()
            .AddCategoryService()
            .AddUserAccountService()
            ;

        return services;
    }
}
