using AutoMapper;
using NovaRecipesProject.Services.Recipes.Models.RecipeIngredientModels;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models.RecipeIngredientModels;

/// <summary>
/// Basic model with data for adding new Ingredient to a recipe
/// </summary>
public class AddRecipeIngredientRequest
{
#pragma warning disable CS1591
    public int IngredientId { get; set; }
    public float Weight { get; set; }
    public string Portion { get; set; } = null!;
#pragma warning restore CS1591
}

/// <inheritdoc />
public class AddRecipeIngredientRequestProfile : Profile
{
    /// <inheritdoc />
    public AddRecipeIngredientRequestProfile()
    {
        CreateMap<AddRecipeIngredientRequest, AddRecipeIngredientModel>();
    }
}