using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models.RecipeIngredientModels;

/// <summary>
/// DTO model to map to entity model and validate data 
/// </summary>
public class UpdateRecipeIngredientModel
{
#pragma warning disable CS1591
    public int RecipeId { get; set; }
    public int IngredientId { get; set; }
    public float Weight { get; set; }
    public string Portion { get; set; } = null!;
#pragma warning restore CS1591
}

/// <inheritdoc />
public class UpdateRecipeIngredientModelProfile : Profile
{
    /// <inheritdoc />
    public UpdateRecipeIngredientModelProfile()
    {
        CreateMap<UpdateRecipeIngredientModel, RecipeIngredient>();
    }
}

/// <inheritdoc />
public class UpdateRecipeIngredientModelValidator : AbstractValidator<UpdateRecipeIngredientModel>
{
    /// <inheritdoc />
    public UpdateRecipeIngredientModelValidator()
    {
        RuleFor(x => x.IngredientId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Ingredient id can't be negative");

        RuleFor(x => x.RecipeId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Recipe id can't be negative");

        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .WithMessage("Weight should be a positive value");
    }
}