using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities.MainData;

namespace NovaRecipesProject.Services.Categories.Models;

/// <summary>
/// Basic model to use in category's service
/// </summary>
public class CategoryModel : BaseNameDescriptionModel
{
    /// <summary>
    /// Category's Id
    /// </summary>
    public int Id { get; set; }
}

/// <inheritdoc />
public class CategoryModelValidator : BaseNameDescriptionModelValidator<CategoryModel>
{
    /// <summary>
    /// Constructor to initialize all things
    /// </summary>
    public CategoryModelValidator()
    {
    }
}

/// <inheritdoc />
public class CategoryModelProfile : Profile
{
    /// <inheritdoc />
    public CategoryModelProfile()
    {
        CreateMap<CategoryModel, Category>().ReverseMap();
    }
}