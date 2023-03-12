using FluentValidation;

namespace NovaRecipesProject.Common.Models.BaseModels;

/// <summary>
/// Basic model for defining other models for controller methods
/// </summary>
public class BaseNameDescriptionRequest
{
    /// <summary>
    /// Category's name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Category's description
    /// </summary>
    public string? Description { get; set; } = string.Empty;
}

/// <summary>
/// Base validator for all others validators (who include this model) to use
/// </summary>
public class BaseNameDescriptionRequestValidator<T> : AbstractValidator<T> where T : BaseNameDescriptionModel
{
    /// <inheritdoc />
    protected BaseNameDescriptionRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Recipe name is required")
            .MinimumLength(2).WithMessage("Name is too short")
            .MaximumLength(128).WithMessage("Name is too long");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description is too long");
    }
}