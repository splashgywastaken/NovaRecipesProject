using System.Reflection;

namespace NovaRecipesProject.Common.Extensions;

/// <summary>
/// Extension methods for working with assembly's data
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Returns assembly's description from AssemblyDescriptionAttribute
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static string? GetAssemblyDescription(this Assembly assembly)
    {
        return assembly.GetAssemblyAttribute<AssemblyDescriptionAttribute>()?.Description;
    }

    /// <summary>
    /// Returns assembly's version from fully parsed assembly name
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static string? GetAssemblyVersion(this Assembly assembly)
    {
        return assembly.GetName().Version?.ToString();
    }

    /// <summary>
    /// Returns assembly's attributes
    /// </summary>
    /// <param name="assembly"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    // ReSharper disable once MemberCanBePrivate.Global
    public static T? GetAssemblyAttribute<T>(this Assembly assembly) where T : Attribute
    {
        var attributes = assembly.GetCustomAttributes(typeof(T), true);

        if (attributes.Length == 0)
            return null;

        return (T)attributes[0];
    }
}
