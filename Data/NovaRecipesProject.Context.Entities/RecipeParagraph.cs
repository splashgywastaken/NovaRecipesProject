using NovaRecipesProject.Context.Entities.Common;

namespace NovaRecipesProject.Context.Entities;

/// <summary>
/// Entity used to describe steps in recipe or other things that need to be in some sort of order
/// </summary>
public class RecipeParagraph : BaseNameDescription
{
    /// <summary>
    /// Value used to describe order of current paragraph in recipe
    /// </summary>
    public int OrderNumber { get; set; }
    /// <summary>
    /// Value used for many-to-one relationship
    /// </summary>
    public int RecipeId { get; set; }
    /// <summary>
    /// Recipe itself
    /// </summary>
    public virtual Recipe Recipe { get; set; } = null!;
}