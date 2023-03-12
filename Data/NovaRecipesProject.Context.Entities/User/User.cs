namespace NovaRecipesProject.Context.Entities;

using Microsoft.AspNetCore.Identity;

/// <inheritdoc />
public class User : IdentityUser<Guid>
{
    /// <summary>
    /// Full name
    /// </summary>
    public string FullName { get; set; } = null!;
    /// <summary>
    /// UserStatus 
    /// </summary>
    public UserStatus Status { get; set; }
}
