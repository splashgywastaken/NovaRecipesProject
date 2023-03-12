using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities;
#pragma warning disable CS1591

namespace NovaRecipesProject.Services.RecipeParagraphs.Models;

/// <summary>
/// DTO model for adding 
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class AddRecipeParagraphModel : BaseNameDescriptionModel
{
    public int OrderNumber { get; set; }
}

public class AddRecipeParagraphModelProfile : Profile
{
    public AddRecipeParagraphModelProfile()
    {
        CreateMap<AddRecipeParagraphModel, RecipeParagraph>();
    }
}

public class AddRecipeParagraphModelValidator : BaseNameDescriptionModelValidator<AddRecipeParagraphModel>
{
    public AddRecipeParagraphModelValidator()
    {
        RuleFor(x => x.OrderNumber)
            .GreaterThanOrEqualTo(0).WithMessage("Order number cannot be negative");
    }
}