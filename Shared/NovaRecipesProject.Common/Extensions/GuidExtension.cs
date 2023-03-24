namespace NovaRecipesProject.Common.Extensions;

/// <summary>
/// Extension class for Guid
/// </summary>
public static class GuidExtension
{
    /// <summary>
    /// Replaces "-" and " " with "" in string
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static string Shrink(this Guid guid)
    {
        return guid.ToString().Replace("-", "").Replace(" ", "");
    }
}
