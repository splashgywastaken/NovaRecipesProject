using AutoMapper;
using NovaRecipesProject.Api.Controllers.Categories.Models;
using NovaRecipesProject.Api.Controllers.RecipeParagraphs.Models;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities.Common;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models;

/// <summary>
/// DTO model for recipe 
/// </summary>
public class WholeRecipeResponse : BaseNameDescription
{
    /// <summary>
    /// List of recipe's paragraphs, sorted by order number
    /// </summary>
    public List<RecipeParagraphResponse> RecipeParagraphs { get; set; } = null!;
    /// <summary>
    /// List of recipe's categories by default sorted by category's Id
    /// </summary>
    public List<CategoryResponse> RecipeCategories { get; set; } = null!;
    /// <summary>
    /// List of recipe's ingredients by default sorted by entry's Id
    /// </summary>
    public List<RecipeIngredientResponse> RecipeIngredients { get; set; } = null!;
}

/// <inheritdoc />
public class WholeRecipeResponseProfile : Profile
{
    /// <inheritdoc />
    public WholeRecipeResponseProfile()
    {
        CreateMap<WholeRecipeModel, WholeRecipeResponse>();
    }
}