namespace NovaRecipesProject.Common.Tools;

/// <summary>
/// Static class for tools used to cache data
/// </summary>
public static class CachingTools
{
    /// <summary>
    /// Method used to get full context cache key using some arguments
    /// </summary>
    /// <returns></returns>
    public static string GetContextCacheKey(string contextCacheKey, string arg)
    {
        return $"{contextCacheKey}::{arg}";
    }
}