namespace NovaRecipesProject.Context.Entities;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = null!;
    public UserStatus Status { get; set; }
}
