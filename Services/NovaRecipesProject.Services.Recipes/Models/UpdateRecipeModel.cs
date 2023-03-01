using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models;

/// <summary>
/// DTO for updating data in DB
/// </summary>
public class UpdateRecipeModel
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
public class UpdateRecipeModelValidator : AbstractValidator<UpdateRecipeModel>
{
    /// <summary>
    /// Constructor 
    /// </summary>
    public UpdateRecipeModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(128).WithMessage("Name is too long");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description is too long");
    }
}

/// <inheritdoc />
public class UpdateRecipeProfile : Profile
{
    /// <summary>
    /// Constructor which describes mapping for this DTO
    /// </summary>
    public UpdateRecipeProfile()
    {
        CreateMap<UpdateRecipeModel, Recipe>();
    }
}