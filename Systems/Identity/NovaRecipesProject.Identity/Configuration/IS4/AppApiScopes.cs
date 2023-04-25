namespace NovaRecipesProject.Identity.Configuration;

using NovaRecipesProject.Common.Security;
using Duende.IdentityServer.Models;

public static class AppApiScopes
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new (AppScopes.AllRead, "Access to the whole API - Read data"),
            new (AppScopes.AllEdit, "Access to the whole API - Write, edit or delete data"),
            new (AppScopes.UsersView, "Access to users API - Read data"),
            new (AppScopes.UsersModerate, "Access to users API - Write, edit or delete data"),
            new (AppScopes.CategoriesRead, "Access to categories API - Read data"),
            new (AppScopes.CategoriesEdit, "Access to categories API - Write, edit or delete data"),
            new (AppScopes.IngredientsRead, "Access to ingredients API - Read data"),
            new (AppScopes.IngredientsEdit, "Access to ingredients API - Write, edit or delete data"),
            new (AppScopes.RecipeParagraphsRead, "Access to recipe paragraphs API - Read data"),
            new (AppScopes.RecipeParagraphsEdit, "Access to recipe paragraphs API - Write, edit or delete data"),
            new (AppScopes.RecipesRead, "Access to recipes API - Read data"),
            new (AppScopes.RecipesEdit, "Access to recipes API - Write, edit or delete data"),
        };
}