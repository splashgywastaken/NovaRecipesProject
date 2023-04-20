namespace NovaRecipesProject.Services.RecipeCommentsSubscriptions;

/// <summary>
/// Service used to manage subscriptions for comment sections in certain recipes
/// </summary>
public interface IRecipeCommentsSubscriptionsService
{
    /// <summary>
    /// Method that subscribes user to certain author
    /// </summary>
    /// <param name="subscriberId"></param>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    public Task Subscribe(int subscriberId, int recipeId);
    /// <summary>
    /// Method that unsubscribes user from certain author
    /// </summary>
    /// <param name="subscriberId"></param>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    public Task Unsubscribe(int subscriberId, int recipeId);

    /// <summary>
    /// Method that notifies 
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    public Task NotifySubscribersAboutNewComment(int recipeId);
}