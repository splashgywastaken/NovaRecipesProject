using NovaRecipesProject.Services.RecipesSubscriptions.Models;

namespace NovaRecipesProject.Services.RecipesSubscriptions;

/// <summary>
/// Service used for managing subscriptions for users and recipes and for sending out mailing based on subscription
/// </summary>
public interface IRecipeSubscriptionsService
{
    /// <summary>
    /// Method that subscribes user to certain author
    /// </summary>
    /// <param name="subscriberId"></param>
    /// <param name="authorId"></param>
    /// <returns></returns>
    public Task Subscribe(int subscriberId, int authorId);
    /// <summary>
    /// Method that unsubscribes user from certain author
    /// </summary>
    /// <param name="subscriberId"></param>
    /// <param name="authorId"></param>
    /// <returns></returns>
    public Task Unsubscribe(int subscriberId, int authorId);
    /// <summary>
    /// Method used to send emails to all subscribers of a certain author about the fact that author posted new recipe
    /// </summary>
    /// <param name="authorId"></param>
    /// <param name="recipe"></param>
    /// <returns></returns>
    public Task NotifySubscribersAboutNewRecipe(int authorId, RecipeBaseData recipe);
}