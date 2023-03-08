using AutoMapper;
using NovaRecipesProject.Common.Models;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models;

/// <summary>
/// Recipe response model which will be used in controllers
/// </summary>
public class RecipeResponse : BaseNameDescriptionModel
{
    /// <summary>
    /// Recipe's id in DB
    /// </summary>
    public int Id { get; set; }
}

/// <inheritdoc />
public class RecipeResponseProfile : Profile
{
    /// <summary>
    /// Configures mapping for this exact class
    /// </summary>
    public RecipeResponseProfile()
    {
        CreateMap<RecipeResponse, RecipeModel>();
    }
}