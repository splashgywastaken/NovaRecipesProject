namespace NovaRecipesProject.Common.Security;

/// <summary>
/// Class with API's scopes to use in identity
/// </summary>
public static class AppScopes
{
    /// <summary>
    /// Scope used to read recipes (as guest)
    /// </summary>
    public const string RecipesRead = "recipes_read";
    /// <summary>
    /// Scope used to publish new recipes,
    /// also includes possibilities to edit and delete your recipes
    /// </summary>
    public const string RecipesPublish = "recipes_publish";
    /// <summary>
    /// Scope used to view users pages
    /// </summary>
    public const string UsersView = "users_view";
    /// <summary>
    /// Scope used to moderate any user page, or any comments
    /// </summary>
    public const string UsersModerate = "users_moderate";
}