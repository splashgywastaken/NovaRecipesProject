using AutoMapper;
using NovaRecipesProject.Services.UserAccount;

namespace NovaRecipesProject.Api.Controllers.Accounts.Models;

/// <summary>
/// Model that controllers returns as the result of their actions
/// </summary>
public class UserAccountResponse
{
    /// <summary>
    /// User's Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// User's name which is basically login
    /// </summary> 
    public string Name { get; set; } = null!;

    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; } = null!;
}

/// <inheritdoc />
public class UserAccountResponseProfile : Profile
{
    /// <inheritdoc />
    public UserAccountResponseProfile()
    {
        CreateMap<UserAccountModel, UserAccountResponse>();
    }
}
