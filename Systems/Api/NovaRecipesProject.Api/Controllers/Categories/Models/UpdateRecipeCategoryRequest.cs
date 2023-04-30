using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities.MainData;
using NovaRecipesProject.Services.Categories.Models;

namespace NovaRecipesProject.Api.Controllers.Categories.Models;

/// <summary>
/// Model used for recipe's updates
/// Model is very light
/// </summary>
public class UpdateRecipeCategoryRequest
{
    /// <summary>
    /// Category's Id
    /// </summary>
    public int Id { get; set; }
}

/// <inheritdoc />
public class UpdateRecipeCategoryProfile : Profile
{
    /// <inheritdoc />
    public UpdateRecipeCategoryProfile()
    {
        CreateMap<UpdateRecipeCategoryRequest, Category>();
        CreateMap<UpdateRecipeCategoryRequest, UpdateRecipeCategoryModel>();
    }
}

/// <inheritdoc />
public class UpdateRecipeCategoryModelValidator : AbstractValidator<UpdateRecipeCategoryRequest>
{
    /// <inheritdoc />
    public UpdateRecipeCategoryModelValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Id can't be negative");
    }
}