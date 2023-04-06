using AutoMapper;
using NovaRecipesProject.Services.Recipes.Models.RecipeCommentModels;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models.RecipeCommentModels;

/// <summary>
/// DTO model for update method
/// </summary>
public class UpdateRecipeCommentRequest
{
    /// <summary>
    /// Comments text
    /// </summary>
    public string Text { get; set; } = null!;
}

/// <inheritdoc />
public class UpdateRecipeCommentRequestProfile : Profile
{
    /// <inheritdoc />
    public UpdateRecipeCommentRequestProfile()
    {
        CreateMap<UpdateRecipeCommentRequest, UpdateRecipeCommentModel>();
    }
}