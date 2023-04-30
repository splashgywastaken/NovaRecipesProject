using NovaRecipesProject.Context.Entities.Common;

namespace NovaRecipesProject.Context.Entities.MainData;

/// <summary>
/// Additional entity to make connections between recipe and ingredients
/// </summary>
public class RecipeIngredient : BaseEntity
{
#pragma warning disable CS1591
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
#pragma warning restore CS1591
    /// <summary>
    /// Weight of added to recipe product 
    /// </summary>
    public float Weight { get; set; }
    /// <summary>
    /// Mock implementation of description of portion (later will be changed to additional class with prerecorded values)
    /// </summary>
    public string Portion { get; set; } = null!;
}