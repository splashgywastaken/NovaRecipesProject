using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models;

/// <summary>
/// Model for adding new recipe 
/// </summary>
public class AddRecipeModel
{
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
public class AddRecipeModelProfile : Profile
{
    /// <inheritdoc />
    public AddRecipeModelProfile()
    {
        CreateMap<AddRecipeModel, Recipe>();
    }
}

/// <inheritdoc />
public class AddRecipeModelValidator : AbstractValidator<AddRecipeModel>
{
    /// <inheritdoc />
    public AddRecipeModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Recipe name is required")
            .MaximumLength(128).WithMessage("Name is too long");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description is too long");
    }
}