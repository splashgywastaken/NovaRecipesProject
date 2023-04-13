using Microsoft.Extensions.Configuration;
using NovaRecipesProject.Services.MailSender;

namespace NovaRecipesProject.Services.EmailSender;

using NovaRecipesProject.Settings;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Bootstrapper for email sender service
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Main method used to add email sender to IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddEmailSender(
        this IServiceCollection services,
        IConfiguration? configuration = null
        )
    {
        var settings = Settings.Load<EmailSenderSettings>("EmailSender", configuration);
        services.AddSingleton(settings);

        services.AddSingleton<IEmailSender, EmailSender>();

        return services;
    }
}
