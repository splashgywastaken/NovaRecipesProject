namespace NovaRecipesProject.Identity.Configuration;

using NovaRecipesProject.Common.Security;
using Duende.IdentityServer.Models;

public static class AppApiScopes
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new (AppScopes.RecipesRead, "Access to recipes API - Read data"),
            new (AppScopes.RecipesPublish, "Access to recipes API - Write, edit or delete data"),
            new (AppScopes.UsersView, "Access to users API - Read data"),
            new (AppScopes.UsersModerate, "Access to users API - Write, edit or delete data")
        };
}