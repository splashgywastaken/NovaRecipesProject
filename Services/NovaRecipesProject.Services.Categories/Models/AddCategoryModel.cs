using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Categories.Models;

/// <summary>
/// Model for adding new category 
/// </summary>
public class AddCategoryModel : BaseNameDescriptionModel
{
    
}

/// <inheritdoc />
public class AddCategoryModelProfile : Profile
{
    /// <inheritdoc />
    public AddCategoryModelProfile()
    {
        CreateMap<AddCategoryModel, Category>();
    }
}

/// <inheritdoc />
public class AddCategoryModelValidator : BaseNameDescriptionModelValidator<AddCategoryModel>
{
    /// <inheritdoc />
    public AddCategoryModelValidator()
    {
    }
}