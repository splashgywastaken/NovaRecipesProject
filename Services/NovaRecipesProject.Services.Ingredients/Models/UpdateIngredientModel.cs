﻿using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities.MainData;

#pragma warning disable CS1591
namespace NovaRecipesProject.Services.Ingredients.Models;

/// <summary>
/// Model for updating entry in DB
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class UpdateIngredientModel : BaseNameDescriptionModel
{
    public float Carbohydrates { get; set; }
    public float Proteins { get; set; }
    public float Fat { get; set; }
}

/// <summary>
/// Validator for UpdateIngredientModel model 
/// </summary>
public class UpdateIngredientModelValidator : BaseNameDescriptionModelValidator<UpdateIngredientModel>
{
    public UpdateIngredientModelValidator()
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
/// Profile mapper for UpdateIngredientModel model
/// </summary>
public class UpdateIngredientModelProfile : Profile
{
    public UpdateIngredientModelProfile()
    {
        CreateMap<UpdateIngredientModel, Ingredient>();
    }
}