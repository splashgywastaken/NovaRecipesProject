using AutoMapper;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models;

/// <summary>
/// Recipe response model which will be used in controllers
/// </summary>
public class RecipeResponse
{
    /// <summary>
    /// Recipe's id in DB
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Recipe name
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Recipe description
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <inheritdoc />
public class RecipeResponseProfile : Profile
{
    /// <summary>
    /// Configures mapping for this exact class
    /// </summary>
    public RecipeResponseProfile()
    {
        CreateMap<RecipeModel, RecipeResponse>().ReverseMap();
    }
}