using AutoMapper;
using NovaRecipesProject.Api.Controllers.Categories.Models;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models;

/// <summary>
/// Simple model to use as arg in requests in book controller
/// </summary>
public class UpdateRecipeRequest : BaseNameDescriptionModel
{
    /// <summary>
    /// Recipe's categories
    /// </summary>
    public ICollection<UpdateRecipeCategoryRequest>? CategoryIds { get; set; }
}

/// <inheritdoc />
public class UpdateRecipeRequestProfile : Profile
{
    /// <summary>
    /// Constructor which describes mapping profiles for this DTO
    /// </summary>
    public UpdateRecipeRequestProfile()
    {
        CreateMap<UpdateRecipeRequest, UpdateRecipeModel>();
    }
}