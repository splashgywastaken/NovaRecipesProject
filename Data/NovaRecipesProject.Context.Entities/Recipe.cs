using NovaRecipesProject.Context.Entities.Common;

namespace NovaRecipesProject.Context.Entities;

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
    public List<RecipeParagraph> RecipeParagraphs { get; set; } = null!;
    /// <summary>
    /// Entity for connections
    /// </summary>
    public List<RecipeIngredient> RecipeIngredients { get; set; } = null!;
}