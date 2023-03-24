namespace NovaRecipesProject.Services.UserAccount;

using FluentValidation;

/// <summary>
/// DTO model for registering user with account data
/// </summary>
public class RegisterUserAccountModel
{
#pragma warning disable CS1591
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
#pragma warning restore CS1591
}

/// <inheritdoc />
public class RegisterUserAccountModelValidator : AbstractValidator<RegisterUserAccountModel>
{
    /// <inheritdoc />
    public RegisterUserAccountModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("User name is required.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(50).WithMessage("Password is long.");
    }
}