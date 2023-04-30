namespace NovaRecipesProject.Services.RecipesSubscriptions.Models;

/// <summary>
/// DTO model used for emails mailing and stuff, holds data about all recipes that had new comment in a while
/// </summary>
public class UserNewCommentRecipesMailingModel
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
    public List<string> RecipeNames { get; set; } = null!;
}