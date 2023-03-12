using AutoMapper;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Services.Categories.Models;

namespace NovaRecipesProject.Api.Controllers.Categories.Models;

/// <summary>
/// Model for adding new category to DB
/// </summary>
public class AddCategoryRequest : BaseNameDescriptionModel
{
}

/// <inheritdoc />
public class AddCategoryRequestProfile : Profile
{
    /// <inheritdoc />
    public AddCategoryRequestProfile()
    {
        CreateMap<AddCategoryRequest, AddCategoryModel>();
    }
}