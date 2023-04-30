namespace NovaRecipesProject.Identity.Configuration;

using Common.Security;
using Duende.IdentityServer.Models;

/// <summary>
/// Class to setup config for clients of API
/// </summary>
public static class AppClients
{
    /// <summary>
    /// List of possible clients of API and their config
    /// </summary>
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new ()
            {
                ClientId = "swagger",
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                AccessTokenLifetime = 3600, // 1 hour

                AllowedScopes = {
                    AppScopes.AllRead,
                    AppScopes.AllEdit,
                    AppScopes.UsersView,
                    AppScopes.UsersModerate,
                    AppScopes.UsersSubscriptions,
                    AppScopes.CategoriesRead,
                    AppScopes.CategoriesEdit,
                    AppScopes.IngredientsRead,
                    AppScopes.IngredientsEdit,
                    AppScopes.RecipeParagraphsRead,
                    AppScopes.RecipeParagraphsEdit,
                    AppScopes.RecipesRead,
                    AppScopes.RecipesEdit
                }
            }
            ,
            new ()
            {
                ClientId = "frontend",
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                AllowOfflineAccess = true,
                AccessTokenType = AccessTokenType.Jwt,

                AccessTokenLifetime = 3600, // 1 hour

                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AbsoluteRefreshTokenLifetime = 2592000, // 30 days
                SlidingRefreshTokenLifetime = 1296000, // 15 days

                AllowedScopes = {
                    AppScopes.AllRead,
                    AppScopes.AllEdit,
                    AppScopes.UsersView,
                    AppScopes.UsersModerate,
                    AppScopes.UsersSubscriptions,
                    AppScopes.CategoriesRead,
                    AppScopes.CategoriesEdit,
                    AppScopes.IngredientsRead,
                    AppScopes.IngredientsEdit,
                    AppScopes.RecipeParagraphsRead,
                    AppScopes.RecipeParagraphsEdit,
                    AppScopes.RecipesRead,
                    AppScopes.RecipesEdit
                }
            }
        };
}