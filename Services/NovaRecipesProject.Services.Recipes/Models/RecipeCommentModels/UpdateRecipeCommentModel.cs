using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models.RecipeCommentModels;

/// <summary>
/// DTO model for updating data in recipe's comment
/// </summary>
public class UpdateRecipeCommentModel
{
#pragma warning disable CS1591
    public string Text { get; set; } = null!;
    public int RecipeId { get; set; }
#pragma warning restore CS1591
}

/// <inheritdoc />
public class UpdateRecipeCommentModelProfile : Profile
{
    /// <summary>
    /// Mapping defenitions for recipe's comment
    /// </summary>
    public UpdateRecipeCommentModelProfile()
    {
        CreateMap<UpdateRecipeCommentModel, RecipeComment>();
    }
}

/// <inheritdoc />
public class UpdateRecipeCommentModelValidator : AbstractValidator<UpdateRecipeCommentModel>
{
    /// <summary>
    /// Validator defenitions for recipe's comment
    /// </summary>
    public UpdateRecipeCommentModelValidator()
    {
        RuleFor(x => x.Text)
            .MaximumLength(256)
            .WithMessage("Comment length is exceeded")
            .NotNull()
            .WithMessage("Comment should not be null");

        RuleFor(x => x.RecipeId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Id can't be a negative value");
    }
}