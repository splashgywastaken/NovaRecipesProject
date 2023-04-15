namespace NovaRecipesProject.Services.Cache;

/// <summary>
/// DTO to hold cache settings in
/// </summary>
public class CacheSettings
{
    /// <summary>
    /// Time of the cache keeping (in minutes), default value is 30 minutes
    /// </summary>
    public int CacheLifeTime { get; private set; } = TimeSpan.FromMinutes(30).Minutes;

    /// <summary>
    /// Redis connection string
    /// </summary>
    public string Uri { get; private set; } = null!;
}
