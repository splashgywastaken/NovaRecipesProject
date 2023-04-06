using AutoMapper;
using NovaRecipesProject.Services.Recipes.Models.RecipeIngredientModels;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models.RecipeIngredientModels;

/// <summary>
/// Model used in recipes to also get weight and protion type
/// </summary>
public class RecipeIngredientResponse
{
    /// <summary>
    /// Id of ingredient to later work with on frontend
    /// </summary>
    public int IngredientId { get; set; }
    /// <summary>
    /// Ingredient's name
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Basic nutrition data
    /// </summary>
    public float Carbohydrates { get; set; }
    /// <summary>
    /// Basic nutrition data
    /// </summary>
    public float Proteins { get; set; }
    /// <summary>
    /// Basic nutrition data
    /// </summary>
    public float Fat { get; set; }
    /// <summary>
    /// Basic nutrition data (value must be calculated)
    /// </summary>
    public float TotalCarbohydrates { get; set; }
    /// <summary>
    /// Basic nutrition data (value must be calculated)
    /// </summary>
    public float TotalProteins { get; set; }
    /// <summary>
    /// Basic nutrition data (value must be calculated)
    /// </summary>
    public float TotalFat { get; set; }
#pragma warning disable CS1591
    public float Weight { get; set; }
    public string Portion { get; set; } = null!;
#pragma warning restore CS1591
}

/// <inheritdoc />
public class RecipeIngredientResponseProfile : Profile
{
    /// <inheritdoc />
    public RecipeIngredientResponseProfile()
    {
        CreateMap<RecipeIngredientModel, RecipeIngredientResponse>()
            .ForMember(
                dest => dest.TotalCarbohydrates,
                opt =>
                    opt.MapFrom(x => x.Carbohydrates * x.Weight / 100))
            .ForMember(
                dest => dest.TotalFat,
                opt =>
                    opt.MapFrom(x => x.Fat * x.Weight / 100))
            .ForMember(
                dest => dest.TotalProteins,
                opt =>
                    opt.MapFrom(x => x.Proteins * x.Weight / 100))
            ;
    }
}