using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NovaRecipesProject.Services.UserAccount;

/// <summary>
/// Main service for working with user's data
/// </summary>
public interface IUserAccountService
{
    /// <summary>
    /// Create user account
    /// </summary>
    /// <param name="model">
    /// Contains Email, Name and Password to register user with
    /// </param>
    /// <returns></returns>
    Task<UserAccountModel> Create(RegisterUserAccountModel model);
    /// <summary>
    /// Method used to add new request for email confirmation
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<IActionResult> RequestEmailConfirmation(string email);
    /// <summary>
    /// Method used to confirm email
    /// (aka delete confirmation request from database and generally work with them)
    /// </summary>
    /// <param name="confirmationId"></param>
    /// <returns></returns>
    Task<IActionResult> ConfirmEmail(int confirmationId);
}
