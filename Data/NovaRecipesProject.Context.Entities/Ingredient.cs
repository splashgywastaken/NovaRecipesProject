using NovaRecipesProject.Context.Entities.Common;

namespace NovaRecipesProject.Context.Entities;

/// <summary>
/// Ingredient entity to hold info about recipe's ingredient
/// </summary>
public class Ingredient : BaseNameDescription
{
    /// <summary>
    /// Amount of carbs in 100 gramms of product
    /// </summary>
    public float Carbohydrates { get; set; }
    /// <summary>
    /// Amount of proteins in 100 gramms of product
    /// </summary>
    public float Proteins { get; set; }
    /// <summary>
    /// Amount of fat in 100 gramms of product
    /// </summary>
    public float Fat { get; set; }
    /// <summary>
    /// List of entities used for connection
    /// </summary>
    public List<RecipeIngredient> RecipeIngredients { get; set; } = null!;
}