using NovaRecipesProject.Context.Entities.Common;
using NovaRecipesProject.Context.Entities.MailingAndSubscriptions;

namespace NovaRecipesProject.Context.Entities.MainData;

/// <summary>
/// Recipe entity that contains base info about recipes 
/// </summary>
public class Recipe : BaseNameDescription
{
    /// <summary>
    /// Id of a user that created this recipe
    /// </summary>
    public int RecipeUserId { get; set; }
    /// <summary>
    /// User's entity
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Recipe's categories to ease search through all recipes
    /// </summary>
    public ICollection<Category>? Categories { get; set; }

    /// <summary>
    /// Recipe's main text data used to describe recipe
    /// </summary>
    public virtual List<RecipeParagraph> RecipeParagraphs { get; set; } = null!;
    /// <summary>
    /// Entity for connections
    /// </summary>
    public virtual List<RecipeIngredient> RecipeIngredients { get; set; } = null!;

    /// <summary>
    /// List of comments related to this recipe
    /// </summary>
    public virtual List<RecipeComment>? RecipeComments { get; set; } = null!;
    /// <summary>
    /// List of subscribers for comment section of a recipe
    /// </summary>
    public virtual List<RecipeCommentsSubscription>? RecipeCommentsSubscribers { get; set; } = null!;
}