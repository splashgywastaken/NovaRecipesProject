using Microsoft.Extensions.DependencyInjection;

namespace NovaRecipesProject.Services.EmailSender;

/// <summary>
/// Bootstrapper for email sender service
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Main method used to add email sender to IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddEmailSender(this IServiceCollection services)
    {
        services.AddSingleton<IEmailSender, EmailSender>();

        return services;
    }
}
