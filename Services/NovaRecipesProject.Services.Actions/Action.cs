using Microsoft.Extensions.Logging;
using NovaRecipesProject.Services.EmailSender.Models;

namespace NovaRecipesProject.Services.Actions;

using Consts;
using RabbitMq;
using System.Threading.Tasks;

/// <inheritdoc />
public class Action : IAction
{
    private readonly IRabbitMq _rabbitMq;

    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="rabbitMq"></param>
    public Action(IRabbitMq rabbitMq)
    {
        _rabbitMq = rabbitMq;
    }

    /// <inheritdoc />
    public async Task SendEmail(EmailModel email)
    {
        await _rabbitMq.PushAsync(RabbitMqTaskQueueNames.SendUserAccountEmail, email);
    }
    /// <inheritdoc />
    public async Task SendRecipeInfoEmail(EmailModel email)
    {
        await _rabbitMq.PushAsync(RabbitMqTaskQueueNames.SendRecipesInfoEmail, email);
    }
    /// <inheritdoc />
    public async Task SendNewRecipeCommentNotification(EmailModel email)
    {
        await _rabbitMq.PushAsync(RabbitMqTaskQueueNames.SendNewRecipeCommentNotification, email);
    }
}
