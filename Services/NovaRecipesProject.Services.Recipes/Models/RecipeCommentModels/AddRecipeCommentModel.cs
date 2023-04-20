﻿using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models.RecipeCommentModels;

/// <summary>
/// DTO for adding new comment for recipe
/// </summary>
public class AddRecipeCommentModel
{
#pragma warning disable CS1591
    public int UserId { get; set; }
    public string Text { get; set; } = null!;
    public int RecipeId { get; set; }
#pragma warning restore CS1591
}

/// <inheritdoc />
public class AddRecipeCommentModelProfile : Profile
{
    /// <summary>
    /// Mapper defenitions
    /// </summary>
    public AddRecipeCommentModelProfile()
    {
        CreateMap<AddRecipeCommentModel, RecipeComment>();
    }
}

/// <inheritdoc />
public class AddRecipeCommentModelValidator : AbstractValidator<AddRecipeCommentModel>
{
    /// <summary>
    /// Validator description
    /// </summary>
    public AddRecipeCommentModelValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("UserId can't be negative")
            .NotNull()
            .WithMessage("UserId can't be null");

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