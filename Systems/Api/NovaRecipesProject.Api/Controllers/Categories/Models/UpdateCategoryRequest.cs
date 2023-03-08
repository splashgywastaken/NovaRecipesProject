using AutoMapper;
using NovaRecipesProject.Common.Models;
using NovaRecipesProject.Services.Categories.Models;

namespace NovaRecipesProject.Api.Controllers.Categories.Models;

/// <summary>
/// DTO model used as arg in update controller methods
/// </summary>
public class UpdateCategoryRequest : BaseNameDescriptionModel
{
}

/// <inheritdoc />
public class UpdateCategoryRequestProfile : Profile
{
    /// <inheritdoc />
    public UpdateCategoryRequestProfile()
    {
        CreateMap<UpdateCategoryRequest, UpdateCategoryModel>();
    }
}