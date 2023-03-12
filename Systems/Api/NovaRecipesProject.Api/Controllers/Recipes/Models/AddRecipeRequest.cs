using AutoMapper;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models;

/// <summary>
/// DTO for adding new data to DB
/// </summary>
public class AddRecipeRequest : BaseNameDescriptionModel
{
}

/// <inheritdoc />
public class AddRecipeRequestProfile : Profile
{
    /// <summary>
    /// Constructor which describes mapping for this DTO
    /// </summary>
    public AddRecipeRequestProfile()
    {
        CreateMap<AddRecipeRequest, AddRecipeModel>();
    }
}