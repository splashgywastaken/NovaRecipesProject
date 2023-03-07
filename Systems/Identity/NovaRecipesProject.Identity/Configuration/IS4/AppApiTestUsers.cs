namespace NovaRecipesProject.Identity.Configuration;

using Duende.IdentityServer.Test;

/// <summary>
/// Class for test users
/// </summary>
public static class AppApiTestUsers
{
    /// <summary>
    /// List of test users
    /// </summary>
    public static List<TestUser> ApiUsers =>
        new ()
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "alice@tst.com",
                Password = "password"
            },
            new TestUser
            {
                SubjectId = "2",
                Username = "bob@tst.com",
                Password = "password"
            }
        };
}