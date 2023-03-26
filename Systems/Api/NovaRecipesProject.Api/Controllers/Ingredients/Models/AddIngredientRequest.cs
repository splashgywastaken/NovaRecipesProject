using AutoMapper;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Services.Ingredients.Models;

#pragma warning disable CS1591

namespace NovaRecipesProject.Api.Controllers.Ingredients.Models;

/// <summary>
/// Model used in controllers;
/// Used as argument for POST methods
/// </summary>
public class AddIngredientRequest : BaseNameDescriptionModel
{
    public float Carbohydrates { get; set; }
    public float Proteins { get; set; }
    public float Fat { get; set; }
    public float Weight { get; set; }
    public string Portion { get; set; } = null!;
}

public class AddIngredientRequestProfile : Profile
{
    public AddIngredientRequestProfile()
    {
        CreateMap<AddIngredientRequest, AddIngredientModel>();
    }
}