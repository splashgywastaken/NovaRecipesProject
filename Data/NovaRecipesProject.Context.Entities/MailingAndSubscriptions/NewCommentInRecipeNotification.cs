using NovaRecipesProject.Context.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NovaRecipesProject.Context.Entities.MailingAndSubscriptions;

/// <summary>
/// Entity used to track notifications for user;
/// Used in managing subscriptions and email mailing;
/// </summary>
public class NewCommentInRecipeNotification : BaseEntity
{
    /// <summary>
    /// Id of subscription that was used to notify user
    /// </summary>
    public int SubscriptionId { get; set; }
    /// <summary>
    /// Subscription itself
    /// </summary>
    public RecipeCommentsSubscription Subscription { get; set; } = null!;
    /// <summary>
    /// Date and time of creation
    /// </summary>
    [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime NotificationCreationDateTime { get; set; }
}