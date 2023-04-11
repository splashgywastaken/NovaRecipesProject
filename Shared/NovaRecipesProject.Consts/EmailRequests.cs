namespace NovaRecipesProject.Consts;

/// <summary>
/// Consts for email requests
/// </summary>
public static class EmailRequests
{
    /// <summary>
    /// Time which are used to check if request is expired or not 
    /// </summary>
    public static TimeSpan RequestExpireTime = TimeSpan.FromHours(24);
}