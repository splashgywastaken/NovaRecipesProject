using System.Diagnostics.CodeAnalysis;

namespace NovaRecipesProject.Context.Entities;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

/// <inheritdoc />
public class User : IdentityUser<Guid>
{
    /// <summary>
    /// User's Id for relationships setup
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EntryId { get; set; }
    /// <summary>
    /// Full name
    /// </summary>
    public string FullName { get; set; } = null!;
    /// <summary>
    /// UserStatus 
    /// </summary>
    public UserStatus Status { get; set; }
    /// <summary>
    /// User's list of recipes
    /// </summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    public List<Recipe>? Recipes { get; set; }

    /// <summary>
    /// User's list for recipes subscriptions, used for email mailing
    /// </summary>
    public List<RecipesSubscription>? RecipesSubscriptions { get; set; } = null!;
    /// <summary>
    /// User's list for recipe's comments subscriptions, used for email mailing
    /// </summary>
    public List<RecipeCommentsSubscription>? RecipeCommentsSubscriptions { get; set; } = null!;
}
