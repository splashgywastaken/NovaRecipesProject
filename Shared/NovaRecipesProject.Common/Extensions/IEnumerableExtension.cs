namespace NovaRecipesProject.Common.Extensions;

/// <summary>
/// Extensions for IEnumerables
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IEnumerableExtension
{
    /// <summary>
    /// Method that skips a takes certain amount of items in query
    /// </summary>
    /// <param name="query">Query itself</param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public static IEnumerable<T> SkipAndTake<T>(
        this IEnumerable<T> query,
        int offset,
        int limit
    )
    {
        return query
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));
    }
}