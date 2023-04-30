using NovaRecipesProject.Context.Entities.Common;

namespace NovaRecipesProject.Context.Entities.MainData;

/// <summary>
/// Category entity
/// </summary>
public class Category : BaseNameDescription
{
    /// <summary>
    /// Recipes that are linked to that category
    /// </summary>
    public ICollection<Recipe>? Recipes { get; set; } = null;
}