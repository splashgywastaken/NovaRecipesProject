using FluentValidation;

namespace NovaRecipesProject.Common.Models;

/// <summary>
/// Basic model for defining other models of certain services
/// </summary>
public abstract class BaseNameDescriptionModel
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
public class BaseNameDescriptionModelValidator<T> : AbstractValidator<T> where T : BaseNameDescriptionModel
{
    /// <inheritdoc />
    public BaseNameDescriptionModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Recipe name is required")
            .MaximumLength(128).WithMessage("Name is too long");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description is too long");
    }
}