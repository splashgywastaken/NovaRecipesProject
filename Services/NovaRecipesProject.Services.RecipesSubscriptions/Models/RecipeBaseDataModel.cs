using AutoMapper;
using Microsoft.VisualBasic.CompilerServices;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.RecipesSubscriptions.Models;

/// <summary>
/// Basic data for recipe to use it in email mailings
/// </summary>
public class RecipeBaseData : BaseNameDescriptionModel
{
    /// <summary>
    /// Recipe's id
    /// </summary>
    public int Id { get; set; }
}

/// <inheritdoc />
public class RecipeBaseDataProfile : Profile
{
    /// <inheritdoc />
    public RecipeBaseDataProfile()
    {
        CreateMap<RecipeBaseData, Recipe>().ReverseMap();
    }
}