using NovaRecipesProject.Services.EmailSender.Models;

namespace NovaRecipesProject.Services.EmailSender;

/// <summary>
/// Class which describes how email sending should be treated
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Method used to send out emails
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task Send(EmailModel email);
}