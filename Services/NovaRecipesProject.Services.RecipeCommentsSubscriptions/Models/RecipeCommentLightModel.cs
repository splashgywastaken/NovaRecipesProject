namespace NovaRecipesProject.Services.RecipeCommentsSubscriptions.Models;

/// <summary>
/// DTO light-weight model used to transfer data about comments
/// </summary>
public class RecipeCommentLightModel
{
    /// <summary>
    /// Id of the comment
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Name of user that left comment
    /// </summary>
    public string UserName { get; set; } = null!;
    /// <summary>
    /// Time when comment was created
    /// </summary>
    public DateTime CreatedDateTime { get; set; }
    /// <summary>
    /// Id of recipe to which this comment is related to 
    /// </summary>
    public int RecipeId { get; set; }
}