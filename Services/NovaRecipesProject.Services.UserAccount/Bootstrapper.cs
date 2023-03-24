namespace NovaRecipesProject.Services.UserAccount;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Bootstrapper for IServiceCollection
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Adds UserAccountService as scoped
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddUserAccountService(this IServiceCollection services)
    {
        services.AddScoped<IUserAccountService, UserAccountService>();  // !!!  Обратите внимание, что UserAccount должен объявляться как SCOPED

        return services;
    }
}
