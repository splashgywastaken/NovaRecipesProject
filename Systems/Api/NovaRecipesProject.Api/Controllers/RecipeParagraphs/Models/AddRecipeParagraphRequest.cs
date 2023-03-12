#pragma warning disable CS1591
using AutoMapper;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Services.RecipeParagraphs.Models;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.RecipeParagraphs.Models;

/// <summary>
/// Add DTO request-model
/// </summary>
public class AddRecipeParagraphRequest : BaseNameDescriptionRequest
{
    public int OrderNumber { get; set; }
}

public class AddRecipeParagraphRequestProfile : Profile
{
    public AddRecipeParagraphRequestProfile()
    {
        CreateMap<AddRecipeParagraphRequest, AddRecipeParagraphModel>();
    }
}