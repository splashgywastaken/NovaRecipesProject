namespace NovaRecipesProject.Consts;

/// <summary>
/// Consts for RabbitMq's queues
/// </summary>
public static class RabbitMqTaskQueueNames
{
    /// <summary>
    /// Queue name for sending emails about user's account actions such as registering,
    /// requesting confirmation and confirming email
    /// </summary>
    public const string SendUserAccountEmail = "NOVA_RECIPES_PROJECT_SEND_USER_ACCOUNT_EMAIL";
    /// <summary>
    /// Queue name for sending email about recipes info and all stuff related to them
    /// </summary>
    public const string SendRecipesInfoEmail = "NOVA_RECIPES_PROJECT_SEND_RECIPES_INFO_EMAIL";
    /// <summary>
    /// Queue name for sending notifications about posting new comment for some recipe
    /// </summary>
    public const string SendNewRecipeCommentNotification = "NOVA_RECIPES_PROJECT_SEND_NEW_RECIPE_COMMENT_NOTIFICATION";
}
