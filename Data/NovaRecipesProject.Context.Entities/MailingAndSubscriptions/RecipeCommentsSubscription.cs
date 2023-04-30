using Microsoft.VisualBasic.CompilerServices;
using NovaRecipesProject.Context.Entities.Common;
using NovaRecipesProject.Context.Entities.MainData;

namespace NovaRecipesProject.Context.Entities.MailingAndSubscriptions;

/// <summary>
/// Entity used for managing user's subscription for certain recipes
/// </summary>
public class RecipeCommentsSubscription : BaseEntity
{
#pragma warning disable CS1591
    public int SubscriptionRecipeId { get; set; }
    public Recipe SubscriptionRecipe { get; set; } = null!;
    public int SubscriptionSubscriberId { get; set; }
    public User SubscriptionSubscriber { get; set; } = null!;
    /// <summary>
    /// List of notifications, gets deleted every time notification mailing is done
    /// </summary>
    public virtual List<NewCommentInRecipeNotification> NewCommentNotifications { get; set; } = null!;
#pragma warning restore CS1591
}