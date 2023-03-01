using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models;

/// <summary>
/// Basic model to use in recipe's service
/// </summary>
public class RecipeModel
{
    /// <summary>
    /// Recipe's Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Recipe's name
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Recipe's description
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <inheritdoc />
public class RecipeModelValidator : AbstractValidator<AddRecipeModel>
{
    /// <summary>
    /// Constructor to initialize all things
    /// </summary>
    public RecipeModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Recipe name is required")
            .MaximumLength(128).WithMessage("Name is too long");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description is too long");
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