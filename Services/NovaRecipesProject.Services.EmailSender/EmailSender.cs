using Microsoft.Extensions.Logging;
using NovaRecipesProject.Services.EmailSender.Models;

namespace NovaRecipesProject.Services.EmailSender;

/// <inheritdoc />
public class EmailSender : IEmailSender
{
    /// <summary>
    /// Logger used to log data about sending emails 
    /// </summary>
    private ILogger<EmailSender> Logger { get; }

    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="logger"></param>
    public EmailSender(ILogger<EmailSender> logger)
    {
        Logger = logger;
    }

    /// <inheritdoc />
    public async Task Send(EmailModel model)
    {
        // emulating sending emails TODO: replace this in the future with properly working code
        await Task.Delay(2000);

        Logger.LogDebug($"Email sent: {model.Email} {model.Subject} {model.Message}");
    }
}
