using AutoMapper;
using NovaRecipesProject.Common.Models;
using NovaRecipesProject.Services.Categories.Models;

namespace NovaRecipesProject.Api.Controllers.Categories.Models;

/// <summary>
/// Category response model which will be used in controllers
/// </summary>
public class CategoryResponse : BaseNameDescriptionModel
{
    /// <summary>
    /// Category's Id in DB
    /// </summary>
    public int Id { get; set; }
}

/// <inheritdoc />
public class CategoryResponseProfile : Profile
{
    /// <inheritdoc />
    public CategoryResponseProfile()
    {
        CreateMap<CategoryResponse, CategoryModel>().ReverseMap();
    }
}