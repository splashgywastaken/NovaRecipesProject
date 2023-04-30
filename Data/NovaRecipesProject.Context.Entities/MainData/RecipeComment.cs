using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NovaRecipesProject.Context.Entities.Common;
using NovaRecipesProject.Context.Entities.MailingAndSubscriptions;

namespace NovaRecipesProject.Context.Entities.MainData;

/// <summary>
/// Class used to describe entity for comment section in recipes
/// </summary>
public class RecipeComment : BaseEntity
{
    /// <summary>
    /// Name of user that posted comment
    /// </summary>
    [Required]
    public string UserName { get; set; } = null!;
    /// <summary>
    /// Comments text
    /// </summary>
    [Required]
    public string Text { get; set; } = null!;
    /// <summary>
    /// Value for when was comment generated
    /// </summary>
    [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedDateTime { get; set; }
    /// <summary>
    /// Recipe related to comment
    /// </summary>
    [Required]
    public int CommentRecipeId { get; set; }
    /// <summary>
    /// Recipe related to comment
    /// </summary>
    public Recipe CommentRecipe { get; set; } = null!;
}