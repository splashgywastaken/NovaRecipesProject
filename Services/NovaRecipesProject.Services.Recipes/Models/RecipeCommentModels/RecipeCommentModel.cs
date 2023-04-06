using AutoMapper;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Recipes.Models.RecipeIngredientModels;

namespace NovaRecipesProject.Services.Recipes.Models.RecipeCommentModels;

/// <summary>
/// DTO for recipe's comments
/// </summary>
public class RecipeCommentModel
{
    /// <summary>
    /// Id of the comment
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of user that left comment
    /// </summary>
    public string UserName { get; set; } = null!;
    /// <summary>
    /// Comments text
    /// </summary>
    public string Text { get; set; } = null!;
    /// <summary>
    /// Time when comment was created
    /// </summary>
    public DateTime CreatedDateTime { get; set; }
    /// <summary>
    /// Id of recipe to which this comment is related to 
    /// </summary>
    public int RecipeId { get; set; }
}

/// <inheritdoc />
public class RecipeCommentsModelProfile : Profile
{
    /// <inheritdoc />
    public RecipeCommentsModelProfile()
    {
        CreateMap<RecipeComment, RecipeCommentModel>().ReverseMap();
    }
}