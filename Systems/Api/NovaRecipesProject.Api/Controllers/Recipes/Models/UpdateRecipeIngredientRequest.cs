using AutoMapper;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models;

/// <summary>
/// DTO used to update data for recipe's ingredients
/// </summary>
public class UpdateRecipeIngredientRequest
{
#pragma warning disable CS1591
    public float Weight { get; set; }
    public string Portion { get; set; } = null!;
#pragma warning restore CS1591
}

/// <inheritdoc />
public class UpdateRecipeIngredientRequestProfile : Profile
{
    /// <inheritdoc />
    public UpdateRecipeIngredientRequestProfile()
    {
        CreateMap<UpdateRecipeIngredientRequest, UpdateRecipeIngredientModel>();
    }
}