using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.EmailSender.Models;

namespace NovaRecipesProject.Services.UserAccount;

using AutoMapper;
using Common.Exceptions;
using Common.Validator;
using Context.Entities;
using Microsoft.AspNetCore.Identity;

/// <inheritdoc />
public class UserAccountService : IUserAccountService
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IModelValidator<RegisterUserAccountModel> _registerUserAccountModelValidator;
    private readonly IAction _action;

    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="userManager"></param>
    /// <param name="registerUserAccountModelValidator"></param>
    /// <param name="action"></param>
    public UserAccountService(
        IMapper mapper,
        UserManager<User> userManager, 
        IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator,
        IAction action
        )
    {
        _mapper = mapper;
        _userManager = userManager;
        _registerUserAccountModelValidator = registerUserAccountModelValidator;
        _action = action;
    }

    /// <inheritdoc />
    public async Task<UserAccountModel> Create(RegisterUserAccountModel model)
    {
        _registerUserAccountModelValidator.Check(model);

        // Find user by email
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
            throw new ProcessException("401", $"User account with email {model.Email} already exist.");

        // Create user account
        user = new User()
        {
            Status = UserStatus.Active,
            FullName = model.Name,
            UserName = model.Email, // login = email
            Email = model.Email,
            // TODO: as for now leave it like this, later you will need to implement real email confirmation
            EmailConfirmed = true,
            PhoneNumber = null,
            PhoneNumberConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            throw new ProcessException("401", $"Creating user account is wrong. {string.Join(", ", result.Errors.Select(s => s.Description))}");

        await _action.SendEmail(new EmailModel
        {
            Email = model.Email,
            Subject = "NovaRecipes notification",
            Message = "You are registered"
        });

        // Returning the created user
        return _mapper.Map<UserAccountModel>(user);
    }
}
