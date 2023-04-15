using NovaRecipesProject.Context.Entities.Common;

namespace NovaRecipesProject.Context.Entities;

/// <summary>
/// Entity for managing user's recipes subscription 
/// </summary>
public class RecipesSubscription : BaseEntity
{
#pragma warning disable CS1591
    public int SubscriberId { get; set; }
    public User Subscriber { get; set; } = null!;
    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
#pragma warning restore CS1591
}