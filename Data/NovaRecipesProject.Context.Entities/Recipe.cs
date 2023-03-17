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
}