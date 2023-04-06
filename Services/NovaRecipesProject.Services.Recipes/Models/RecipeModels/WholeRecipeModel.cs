using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Categories.Models;
using NovaRecipesProject.Services.RecipeParagraphs.Models;
using NovaRecipesProject.Services.Recipes.Models.RecipeIngredientModels;

namespace NovaRecipesProject.Services.Recipes.Models.RecipeModels;

/// <summary>
/// 
/// </summary>
public class WholeRecipeModel : BaseNameDescriptionModel
{
    /// <summary>
    /// Recipe's Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// List of recipe's paragraphs, sorted by order number
    /// </summary>
    public List<RecipeParagraphModel> RecipeParagraphs { get; set; } = null!;
    /// <summary>
    /// List of recipe's categories by default sorted by category's Id
    /// </summary>
    public List<CategoryModel> RecipeCategories { get; set; } = null!;
    /// <summary>
    /// List of recipe's ingredients by default sorted by entry's Id
    /// </summary>
    public List<RecipeIngredientModel> RecipeIngredients { get; set; } = null!;
}

/// <inheritdoc />
public class WholeRecipeModelProfile : Profile
{
    /// <inheritdoc />
    public WholeRecipeModelProfile()
    {
        CreateMap<Recipe, WholeRecipeModel>().ReverseMap();
    }
}

/// <summary>
/// Validator for whole recipe model, inherits base NameDescription validator
/// </summary>
public class WholeRecipeModelValidator : BaseNameDescriptionModelValidator<WholeRecipeModel>
{
    /// <inheritdoc />
    public WholeRecipeModelValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Id can't be a negative value");
    }
}