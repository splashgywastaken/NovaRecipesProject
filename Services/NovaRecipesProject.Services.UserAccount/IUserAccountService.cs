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
}
