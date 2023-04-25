using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using NovaRecipesProject.Services.Recipes.Models.RecipeCommentModels;

namespace NovaRecipesProject.Api.Controllers.Recipes.Models.RecipeCommentModels;

/// <summary>
/// DTO model for add method
/// </summary>
public class AddRecipeCommentRequest
{
    /// <summary>
    /// Comments text
    /// </summary>
    public string Text { get; set; } = null!;
}

/// <inheritdoc />
public class AddRecipeCommentRequestProfile : Profile
{
    /// <inheritdoc />
    public AddRecipeCommentRequestProfile()
    {
        CreateMap<AddRecipeCommentRequest, AddRecipeCommentModel>();
    }
}