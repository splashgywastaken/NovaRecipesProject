using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models;

/// <summary>
/// Simple model to use as arg in requests in book controller
/// </summary>
public class UpdateRecipeRequest
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; } = string.Empty;
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