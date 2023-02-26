namespace NovaRecipesProject.Services.UserAccount;

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
