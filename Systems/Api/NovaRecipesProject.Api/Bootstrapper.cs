using NovaRecipesProject.Api.Settings;
using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.Cache;
using NovaRecipesProject.Services.Categories;
using NovaRecipesProject.Services.EmailSender;
using NovaRecipesProject.Services.Ingredients;
using NovaRecipesProject.Services.MailSender;
using NovaRecipesProject.Services.RecipeParagraphs;
using NovaRecipesProject.Services.Recipes;
using NovaRecipesProject.Services.UserAccount;
using NovaRecipesProject.Services.RabbitMq;
using NovaRecipesProject.Services.RecipesSubscriptions;

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
            .AddEmailSender()
            .AddActions()
            .AddRabbitMq()
            // Controller-related services
            .AddRecipeService()
            .AddCategoryService()
            .AddIngredientService()
            .AddRecipeParagraphService()
            .AddUserAccountService()
            .AddRecipeSubscriptionsService()
            ;

        return services;
    }
}
