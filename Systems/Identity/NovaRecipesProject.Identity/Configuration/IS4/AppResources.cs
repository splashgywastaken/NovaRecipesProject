namespace NovaRecipesProject.Identity.Configuration;

using Duende.IdentityServer.Models;

/// <summary>
/// Class that contains resources for API
/// </summary>
public static class AppResources
{
    /// <summary>
    /// Resources variable, which contains description of
    /// </summary>
    public static IEnumerable<ApiResource> Resources => new List<ApiResource>
    {
        new ("api")
    };
}
