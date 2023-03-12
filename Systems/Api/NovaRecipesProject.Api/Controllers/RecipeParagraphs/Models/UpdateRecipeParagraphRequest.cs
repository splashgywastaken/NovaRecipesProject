using AutoMapper;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Services.RecipeParagraphs.Models;

#pragma warning disable CS1591
namespace NovaRecipesProject.Api.Controllers.RecipeParagraphs.Models;

/// <summary>
/// Update DTO request-model
/// </summary>
public class UpdateRecipeParagraphRequest : BaseNameDescriptionRequest
{
    public int OrderNumber { get; set; }
}

public class UpdateRecipeParagraphRequestProfile : Profile
{
    public UpdateRecipeParagraphRequestProfile()
    {
        CreateMap<UpdateRecipeParagraphRequest, UpdateRecipeParagraphModel>();
    }
}