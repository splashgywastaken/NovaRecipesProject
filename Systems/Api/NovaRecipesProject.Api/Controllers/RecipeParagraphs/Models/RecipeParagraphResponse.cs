using AutoMapper;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Services.RecipeParagraphs.Models;

#pragma warning disable CS1591

namespace NovaRecipesProject.Api.Controllers.RecipeParagraphs.Models;

/// <summary>
/// DTO response model 
/// </summary>
public class RecipeParagraphResponse : BaseNameDescriptionModel
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
}

public class RecipeParagraphResponseProfile : Profile
{
    public RecipeParagraphResponseProfile()
    {
        CreateMap<RecipeParagraphResponse, RecipeParagraphModel>().ReverseMap();
    }
}