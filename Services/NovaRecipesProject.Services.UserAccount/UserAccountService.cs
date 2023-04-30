using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Context.Entities.MailingAndSubscriptions;
using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.EmailSender.Models;
using Npgsql;

namespace NovaRecipesProject.Services.UserAccount;

using AutoMapper;
using Azure.Core;
using Common.Exceptions;
using Common.Validator;
using Context.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovaRecipesProject.Context;

/// <inheritdoc />
public class UserAccountService : IUserAccountService
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IModelValidator<RegisterUserAccountModel> _registerUserAccountModelValidator;
    private readonly IAction _action;
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="userManager"></param>
    /// <param name="registerUserAccountModelValidator"></param>
    /// <param name="action"></param>
    /// <param name="dbContextFactory"></param>
    public UserAccountService(
        IMapper mapper,
        UserManager<User> userManager, 
        IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator,
        IAction action,
        IDbContextFactory<MainDbContext> dbContextFactory
        )
    {
        _mapper = mapper;
        _userManager = userManager;
        _registerUserAccountModelValidator = registerUserAccountModelValidator;
        _action = action;
        _dbContextFactory = dbContextFactory;
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
            EmailConfirmed = false,
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

    /// <inheritdoc />
    public async Task<IActionResult> RequestEmailConfirmation(string email)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        
        var user = await _userManager
            .Users
            .FirstOrDefaultAsync(x => x.Email == email);

        ProcessException.ThrowIf(() => user is null, $"User (email: {email}) was not found");

        // If email is already confirmed
        if (user!.EmailConfirmed) return new OkObjectResult("Email is confirmed");

        var confirmationRequest = await context
            .EmailConfirmationRequests
            .FirstOrDefaultAsync(x => x.Email == email);

        // If confirmationRequest exists
        if (confirmationRequest is not null)
        {
            return new OkObjectResult("Request is already exists");
        }

        // If there is nothing for current user's confirmation request and email is not confirmed
        // than adding new entry for request in DB
        var data = new EmailConfirmationRequest
        {
            Email = email
        };

        var request = (await context.EmailConfirmationRequests.AddAsync(data)).Entity;
        await context.SaveChangesAsync();

        // TODO: Send email
        // TODO: change this so that there will be real domain, not what is used in debug project version
        await _action.SendEmail(new EmailModel
        {
            Email = email,
            Subject = "Email confirmation",
            Message = "Please, confirm email using this link: " +
                      $"http://localhost/10000/account/confirm-email/{request.Id}"
        });

        return new OkObjectResult("Email confirmation request created");
    }

    /// <inheritdoc />
    public async Task<IActionResult> ConfirmEmail(int confirmationId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var request = await context
            .EmailConfirmationRequests
            .FirstOrDefaultAsync(x => x.Id == confirmationId);

        ProcessException
            .ThrowIf(
                () => request is null,
                $"Email confirmation request (id: {confirmationId}) was not found"
                );

        var user = await _userManager
            .Users
            .FirstOrDefaultAsync(x => x.Email == request!.Email);

        ProcessException
            .ThrowIf(
                () => user is null,
                $"User from email confirmation request (email: {user!.Email}) was not found"
                );

        // If time from creating request do not exceeds limit
        if (request!.RequestCreationDateTime + Consts.EmailRequests.RequestExpireTime > DateTime.Now)
        {
            // Setting email confirmed as true
            user.EmailConfirmed = true;
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        else
        {
            return new BadRequestObjectResult("Request is too old to be confirmed, please renew it");
        }

        // Removing email confirmation request
        context.EmailConfirmationRequests.Remove(request);
        await context.SaveChangesAsync();
        
        // Sends email
        await _action.SendEmail(new EmailModel
        {
            Email = request.Email,
            Subject = "Email confirmation",
            Message = "Email was confirmed successfully"
        });

        return new OkObjectResult("Email was confirmed");
    }
}
