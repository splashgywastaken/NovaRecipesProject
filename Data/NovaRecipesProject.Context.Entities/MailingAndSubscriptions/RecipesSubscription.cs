using System.ComponentModel.DataAnnotations.Schema;
using NovaRecipesProject.Context.Entities.Common;

namespace NovaRecipesProject.Context.Entities.MailingAndSubscriptions;

/// <summary>
/// Entity for managing user's recipes subscription 
/// </summary>
public class RecipesSubscription : BaseEntity
{
#pragma warning disable CS1591
    public int SubscriberId { get; set; }
    [ForeignKey("Author")]
    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
#pragma warning restore CS1591
}