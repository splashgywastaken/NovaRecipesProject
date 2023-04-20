using Microsoft.VisualBasic.CompilerServices;
using NovaRecipesProject.Context.Entities.Common;

namespace NovaRecipesProject.Context.Entities;

/// <summary>
/// Entity used for managing user's subscription for certain recipes
/// </summary>
public class RecipeCommentsSubscription : BaseEntity
{
#pragma warning disable CS1591
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
    public int SubscriberId { get; set; }
    public User Subscriber { get; set; } = null!;
#pragma warning restore CS1591
}