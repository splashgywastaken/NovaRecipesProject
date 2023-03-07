namespace NovaRecipesProject.API.Controllers.Models;

using AutoMapper;
using Services.UserAccount;
using FluentValidation;

/// <summary>
/// Model used for registring new users
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class RegisterUserAccountRequest
{
    /// <summary>
    /// User's name (login)
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// User's email
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// User's password
    /// </summary>
    public string Password { get; set; } = null!;
}

/// <inheritdoc />
public class RegisterUserAccountRequestValidator : AbstractValidator<RegisterUserAccountRequest>
{
    /// <inheritdoc />
    public RegisterUserAccountRequestValidator()
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

/// <inheritdoc />
public class RegisterUserAccountRequestProfile : Profile
{
    /// <inheritdoc />
    public RegisterUserAccountRequestProfile()
    {
        CreateMap<RegisterUserAccountRequest, RegisterUserAccountModel>();
    }
}

