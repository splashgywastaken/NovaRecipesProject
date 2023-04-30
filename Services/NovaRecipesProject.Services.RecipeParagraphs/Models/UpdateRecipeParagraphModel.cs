using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities.MainData;

#pragma warning disable CS1591

namespace NovaRecipesProject.Services.RecipeParagraphs.Models;

/// <summary>
/// DTO model for updating
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class UpdateRecipeParagraphModel : BaseNameDescriptionModel
{
    public int OrderNumber { get; set; }
}

public class UpdateRecipeParagraphModelProfile : Profile
{
    public UpdateRecipeParagraphModelProfile()
    {
        CreateMap<UpdateRecipeParagraphModel, RecipeParagraph>();
    }
}

public class UpdateRecipeParagraphModelValidator : BaseNameDescriptionModelValidator<UpdateRecipeParagraphModel>
{
    public UpdateRecipeParagraphModelValidator()
    {
        RuleFor(x => x.OrderNumber)
            .GreaterThanOrEqualTo(0).WithMessage("Order number cannot be negative");
    }
}