using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities;

#pragma warning disable CS1591

namespace NovaRecipesProject.Services.Ingredients.Models;

/// <summary>
/// Model for adding new entry in DB
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class AddIngredientModel : BaseNameDescriptionModel
{
    public int Id { get; set; }
    public float Carbohydrates { get; set; }
    public float Proteins { get; set; }
    public float Fat { get; set; }
}

/// <summary>
/// Validator for AddIngredientModel
/// </summary>
public class AddIngredientModelValidator : BaseNameDescriptionModelValidator<AddIngredientModel>
{
    public AddIngredientModelValidator()
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

/// <summary>
/// Profile mapper for AddIngredientModel
/// </summary>
public class AddIngredientModelProfile : Profile
{
    public AddIngredientModelProfile()
    {
        CreateMap<AddIngredientModel, Ingredient>();
    }
}