using NovaRecipesProject.Services.EmailSender.Models;

namespace NovaRecipesProject.Services.EmailSender;

/// <summary>
/// Class which describes how email sending should be treated
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Method used to send e-mails
    /// </summary>
    /// <param name="from">e-mail of user who sends</param>
    /// <param name="to">e-mail of someone who should recieve</param>
    /// <param name="subject">subject of e-mail</param>
    /// <param name="body">body part of e-mail</param>
    public void Send(string from, string to, string subject, string body);
    /// <summary>
    /// Async method used to send e-mails using DTO model
    /// </summary>
    /// <param name="from">e-mail of user who sends</param>
    /// <param name="to">e-mail of someone who should recieve</param>
    /// <param name="subject">subject of e-mail</param>
    /// <param name="body">body part of e-mail</param>
    public Task SendAsync(string from, string to, string subject, string body);
    /// <summary>
    /// Async method used to send e-mails
    /// </summary>
    /// <param name="data">DTO model for email</param>
    public Task SendAsync(EmailModel data);
}