using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models;

/// <summary>
/// Basic model to use in recipe's service
/// </summary>
public class RecipeModel : BaseNameDescriptionModel
{
    /// <summary>
    /// Recipe's Id
    /// </summary>
    public int Id { get; set; }
}

/// <inheritdoc />
public class RecipeModelValidator : BaseNameDescriptionModelValidator<RecipeModel>
{
    /// <summary>
    /// Constructor to initialize all things
    /// </summary>
    public RecipeModelValidator()
    {
    }
}

/// <inheritdoc />
public class RecipeModelProfile : Profile
{
    /// <inheritdoc />
    public RecipeModelProfile()
    {
        CreateMap<Recipe, RecipeModel>().ReverseMap();
    }
}