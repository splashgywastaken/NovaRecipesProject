using AutoMapper;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models;

/// <summary>
/// DTO for adding new data to DB
/// </summary>
public class AddRecipeRequest
{
    /// <summary>
    /// Name of recipe
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Recipe description
    /// </summary>
    public string Description { get; set; } = string.Empty;
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