using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities;
#pragma warning disable CS1591

namespace NovaRecipesProject.Services.RecipeParagraphs.Models;

/// <summary>
/// Main DTO model 
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class RecipeParagraphModel : BaseNameDescriptionModel
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public int OrderNumber { get; set; }
}

public class RecipeParagraphModelProfile : Profile
{
    public RecipeParagraphModelProfile()
    {
        CreateMap<RecipeParagraphModel, RecipeParagraph>().ReverseMap();
    }
}

public class RecipeParagraphModelValidator : BaseNameDescriptionModelValidator<RecipeParagraphModel>
{
    public RecipeParagraphModelValidator()
    {
        RuleFor(x => x.OrderNumber)
            .GreaterThanOrEqualTo(0).WithMessage("Order number cannot be negative");
    }
}