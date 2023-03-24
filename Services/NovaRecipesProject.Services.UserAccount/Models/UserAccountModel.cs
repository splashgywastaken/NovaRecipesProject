namespace NovaRecipesProject.Services.UserAccount;

using AutoMapper;
using Context.Entities;

/// <summary>
/// User account data DTO model
/// </summary>
public class UserAccountModel
{
#pragma warning disable CS1591
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
#pragma warning restore CS1591
}

/// <inheritdoc />
public class UserAccountModelProfile : Profile
{
    /// <inheritdoc />
    public UserAccountModelProfile()
    {
        CreateMap<User, UserAccountModel>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
            ;
    }
}
