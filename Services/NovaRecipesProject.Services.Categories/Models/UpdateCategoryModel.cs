using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Categories.Models;

/// <summary>
/// Model used to update data in categories
/// </summary>
public class UpdateCategoryModel : BaseNameDescriptionModel
{
}

/// <inheritdoc />
public class UpdateCategoryModelProfile : Profile
{
    /// <inheritdoc />
    public UpdateCategoryModelProfile()
    {
        CreateMap<UpdateCategoryModel, Category>();
    }
}

/// <inheritdoc />
public class UpdateCategoryModelValidator : AbstractValidator<UpdateCategoryModel>
{
    /// <inheritdoc />
    public UpdateCategoryModelValidator()
    {
    }
}