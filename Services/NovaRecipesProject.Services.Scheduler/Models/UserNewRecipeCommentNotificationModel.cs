namespace NovaRecipesProject.Services.RecipesSubscriptions.Models;

/// <summary>
/// DTO model used to get data about user's comment subscriptions
/// </summary>
public class UserNewRecipeCommentNotificationModel
{
    /// <summary>
    /// User's username
    /// </summary>
    public string UserName { get; set; } = null!;
    /// <summary>
    /// User's email, used for sending email
    /// </summary>
    public string UserEmail { get; set; } = null!;
    /// <summary>
    /// Recipe's name
    /// </summary>
    public string RecipeName { get; set; } = null!;
}