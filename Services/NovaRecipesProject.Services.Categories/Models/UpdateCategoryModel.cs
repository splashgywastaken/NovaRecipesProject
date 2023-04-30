using AutoMapper;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities.MainData;

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
public class UpdateCategoryModelValidator : BaseNameDescriptionModelValidator<UpdateCategoryModel>
{
    /// <inheritdoc />
    public UpdateCategoryModelValidator()
    {
    }
}