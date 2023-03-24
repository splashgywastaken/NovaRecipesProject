#pragma warning disable CS1591
using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Services.RecipeParagraphs.Models;

namespace NovaRecipesProject.Api.Controllers.RecipeParagraphs.Models;

/// <summary>
/// Add DTO request-model
/// </summary>
public class AddRecipeParagraphRequest : BaseNameDescriptionRequest
{
    public int OrderNumber { get; set; }
}

public class AddRecipeParagraphRequestValidator : AbstractValidator<AddRecipeParagraphRequest>
{
    public AddRecipeParagraphRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(64)
            .WithMessage("Recipe paragraph name is too long");
        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Recipe paragraph description is too long");
        RuleFor(x => x.OrderNumber)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Recipe paragraph number of order (orderNumber) can't be negative");
    }
}


public class AddRecipeParagraphRequestProfile : Profile
{
    public AddRecipeParagraphRequestProfile()
    {
        CreateMap<AddRecipeParagraphRequest, AddRecipeParagraphModel>();
    }
}