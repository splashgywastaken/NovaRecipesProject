using NovaRecipesProject.Services.EmailSender.Models;

namespace NovaRecipesProject.Services.Actions;

using System.Threading.Tasks;

/// <summary>
/// Class used to describe actions performed by app
/// </summary>
public interface IAction
{
    /// <summary>
    /// Action used to send email with EmailSender service in SendUserAccountEmail queue
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task SendEmail(EmailModel email);
    /// <summary>
    /// Action used to send email with EmailSender service in SendRecipesInfoEmail queue
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task SendRecipeInfoEmail(EmailModel email);
}
