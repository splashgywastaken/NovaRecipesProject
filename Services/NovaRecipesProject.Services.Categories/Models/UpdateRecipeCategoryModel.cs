using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Categories.Models;

/// <summary>
/// Light model for updating data about recipe's categories
/// </summary>
public class UpdateRecipeCategoryModel
{
    /// <summary>
    /// Category's id
    /// </summary>
    public int Id { get; set; }
}

/// <inheritdoc />
public class UpdateRecipeCategoryModelProfile : Profile
{
    /// <inheritdoc />
    public UpdateRecipeCategoryModelProfile()
    {
        CreateMap<UpdateRecipeCategoryModel, Category>();
        CreateMap<UpdateRecipeCategoryModel, UpdateCategoryModel>();
    }
}

/// <inheritdoc />
public class UpdateRecipeCategoryModelValidator : AbstractValidator<UpdateRecipeCategoryModel>
{
    /// <inheritdoc />
    public UpdateRecipeCategoryModelValidator()
    {
    }
}