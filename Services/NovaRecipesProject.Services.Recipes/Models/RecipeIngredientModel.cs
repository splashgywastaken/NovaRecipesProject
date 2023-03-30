using AutoMapper;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models;

/// <summary>
/// All possible data about ingredient in recipe
/// </summary>
public class RecipeIngredientModel
{
    /// <summary>
    /// Recipe's id to used to know to which recipe ingredient is belongs to
    /// </summary>
    public int RecipeId { get; set; }
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
#pragma warning disable CS1591
    public float Weight { get; set; }
    public string Portion { get; set; } = null!;
#pragma warning restore CS1591
}

/// <inheritdoc />
public class RecipeIngredientModelProfile : Profile
{
    /// <inheritdoc />
    public RecipeIngredientModelProfile()
    {
        CreateMap<RecipeIngredient, RecipeIngredientModel>()
            .ForMember(
                dest => dest.Carbohydrates,
                opt => opt.MapFrom(src => src.Ingredient.Carbohydrates)
                )
            .ForMember(
                dest => dest.Fat,
                opt => opt.MapFrom(src => src.Ingredient.Fat)
            )
            .ForMember(
                dest => dest.Proteins,
                opt => opt.MapFrom(src => src.Ingredient.Proteins)
            )
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Ingredient.Name)
            )
            .ReverseMap();
    }
}