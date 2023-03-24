namespace NovaRecipesProject.Common.Extensions;

/// <summary>
/// Extension methods for IQueryable class
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IQueryableExtension
{
    /// <summary>
    /// Method that skips a takes certain amount of items in query
    /// </summary>
    /// <param name="query">Query itself</param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public static IQueryable<T> SkipAndTake<T>(
        this IQueryable<T> query,
        int offset,
        int limit
        )
    {
        return query
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));
    }
}