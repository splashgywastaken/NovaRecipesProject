using System.Globalization;

namespace NovaRecipesProject.Common.Security;

/// <summary>
/// Class with API's scopes to use in identity
/// </summary>
public static class AppScopes
{
    /// <summary>
    /// Allows user to subscribe to any notifications
    /// </summary>
    public const string UsersSubscriptions = "users_subscriptions";
    /// <summary>
    /// Scope used to view users pages
    /// </summary>
    public const string UsersView = "users_view";
    /// <summary>
    /// Scope used to moderate any user page, or any comments
    /// </summary>
    public const string UsersModerate = "users_moderate";
    /// <summary>
    /// Scope which allows user to read all main entries with [GET] methods
    /// </summary>
    public const string AllRead = "read_all";
    /// <summary>
    /// Scope which allows user to edit (delete and update) all main entries
    /// </summary>
    public const string AllEdit = "edit_all";
    /// <summary>
    /// Scope used to read categories with [GET] methods
    /// </summary>
    public const string CategoriesRead = "categories_read";
    /// <summary>
    /// Scope which gives access to users to edit (update and delete) entries for categories
    /// </summary>
    public const string CategoriesEdit = "categories_edit";
    /// <summary>
    /// Scope which gives access to users to read ingredients with [GET] methodss
    /// </summary>
    public const string IngredientsRead = "ingredients_read";
    /// <summary>
    /// Scope which gives access to users to edit (update and delete) entries for ingredients
    /// </summary>
    public const string IngredientsEdit = "ingredients_edit";
    /// <summary>
    /// Scope which gives access to users to read recipe paragraphs with [GET] methodss
    /// </summary>
    public const string RecipeParagraphsRead = "recipe_paragraphs_read";
    /// <summary>
    /// Scope which gives access to users to edit (update and delete) entries for recipe paragraphs
    /// </summary>
    public const string RecipeParagraphsEdit = "recipe_paragraphs_edit";
    /// <summary>
    /// Scope which gives access to users to read recipe paragraphs with [GET] methodss
    /// </summary>
    public const string RecipesRead = "recipes_read";
    /// <summary>
    /// Scope which gives access to users to edit (update and delete) entries for recipe paragraphs
    /// </summary>
    public const string RecipesEdit = "recipes_edit";
}