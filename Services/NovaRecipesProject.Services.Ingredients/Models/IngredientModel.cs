using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities;

#pragma warning disable CS1591

namespace NovaRecipesProject.Services.Ingredients.Models;

/// <summary>
/// Main DTO for Ingredient entity
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class IngredientModel : BaseNameDescriptionModel
{
    public int Id { get; set; }
    public float Carbohydrates { get; set; }
    public float Proteins { get; set; }
    public float Fat { get; set; }
}

/// <summary>
/// Model validator for ingredient model
/// </summary>
public class IngredientModelValidator : BaseNameDescriptionModelValidator<IngredientModel>
{
    public IngredientModelValidator()
    {
        RuleFor(x => x.Carbohydrates)
            .GreaterThanOrEqualTo(0.0f).WithMessage("Carbohydrates should be between 0 and 100")
            .LessThanOrEqualTo(100.0f).WithMessage("Carbohydrates should be between 0 and 100");

        RuleFor(x => x.Proteins)
            .GreaterThanOrEqualTo(0.0f).WithMessage("Proteins value should be between 0 and 100")
            .LessThanOrEqualTo(100.0f).WithMessage("Proteins value should be between 0 and 100");

        RuleFor(x => x.Fat)
            .GreaterThanOrEqualTo(0.0f).WithMessage("Fat value should be between 0 and 100")
            .LessThanOrEqualTo(100.0f).WithMessage("Fat value should be between 0 and 100");
    }
}

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientModel>().ReverseMap();
    }
}