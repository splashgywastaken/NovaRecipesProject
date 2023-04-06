using AutoMapper;
using NovaRecipesProject.Services.Recipes.Models.RecipeCommentModels;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models.RecipeCommentModels;

/// <summary>
/// DTO model for response
/// </summary>
public class RecipeCommentResponse
{
    /// <summary>
    /// Comment's Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Name of user that posted comment
    /// </summary>
    public string UserName { get; set; } = null!;
    /// <summary>
    /// Comments text
    /// </summary>
    public string Text { get; set; } = null!;
    /// <summary>
    /// Value for when was comment generated
    /// </summary>
    public DateTime CreatedDateTime { get; set; }
}

/// <inheritdoc />
public class RecipeCommentResponseProfile : Profile
{
    /// <inheritdoc />
    public RecipeCommentResponseProfile()
    {
        CreateMap<RecipeCommentModel, RecipeCommentResponse>();
    }
}