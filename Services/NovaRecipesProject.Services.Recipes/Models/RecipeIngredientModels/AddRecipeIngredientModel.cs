using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models.RecipeIngredientModels;

/// <summary>
/// Model for adding new ingredient to a recipe
/// </summary>
public class AddRecipeIngredientModel
{
#pragma warning disable CS1591
    public int RecipeId { get; set; }
    public int IngredientId { get; set; }
    public float Weight { get; set; }
    public string Portion { get; set; } = null!;
#pragma warning restore CS1591
}

/// <inheritdoc />
public class AddRecipeIngredientModelProfile : Profile
{
    /// <summary>
    /// Mapper definitions:
    /// here property IngredientId skipped beacause it is used only to get data about recipe from DB.
    /// And add new entry for RecipeIngredient
    /// </summary> 
    public AddRecipeIngredientModelProfile()
    {
        CreateMap<AddRecipeIngredientModel, RecipeIngredient>();
    }
}

/// <inheritdoc />
public class AddRecipeIngredientModelValidator : AbstractValidator<AddRecipeIngredientModel>
{
    /// <summary>
    /// Validator description, yet its declaring anything for portion, becuase currently portion is a placeholder
    /// </summary>
    public AddRecipeIngredientModelValidator()
    {
        RuleFor(x => x.RecipeId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Id can't be a negative value");

        RuleFor(x => x.IngredientId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Id can't be a negative value");

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Weight can't be a negative value");
    }
}