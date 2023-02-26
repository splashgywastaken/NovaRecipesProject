using System.Reflection;

namespace NovaRecipesProject.Common.Extensions;

public static class AssemblyExtensions
{
    public static string GetAssemblyDescription(this Assembly assembly)
    {
        return assembly.GetAssemblyAttribute<AssemblyDescriptionAttribute>()?.Description;
    }

    public static string GetAssemblyVersion(this Assembly assembly)
    {
        return assembly.GetName().Version?.ToString();
    }

    public static T GetAssemblyAttribute<T>(this Assembly assembly) where T : Attribute
    {
        var attributes = assembly.GetCustomAttributes(typeof(T), true);

        if (attributes.Length == 0)
            return null;

        return (T)attributes[0];
    }
}
