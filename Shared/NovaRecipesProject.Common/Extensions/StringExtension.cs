using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NovaRecipesProject.Common.Extensions;

/// <summary>
/// Extension class for string
/// </summary>
public static partial class StringExtension
{
    /// <summary>
    /// Checks if string null or empty
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string data)
    {
        return string.IsNullOrEmpty(data);
    }

    /// <summary>
    /// Checks if string null/empty/consists only of whitespace characters
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool IsNullOrWhiteSpace(this string data)
    {
        return string.IsNullOrWhiteSpace(data);
    }

    /// <summary>
    /// Sets string to title case
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ToTitleCase(this string text)
    {
        if (text.IsNullOrEmpty())
            return string.Empty;

        var textInfo = new CultureInfo("en-US", false).TextInfo;
        text = textInfo.ToTitleCase(text.ToLower());
        return text;
    }

    /// <summary>
    /// Sets string to lower case
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string str)
    {
        return JsonNamingPolicy.CamelCase.ConvertName(str);
    }

    /// <summary>
    /// Splits string in chunks
    /// </summary>
    /// <param name="str">string itself</param>
    /// <param name="chunkSize">string chunk size to split to</param>
    /// <returns></returns>
    public static IEnumerable<string> ToSplit(this string str, int chunkSize)
    {
        if (string.IsNullOrEmpty(str))
            return new List<string>();

        if (str.Length < chunkSize)
            return new List<string>
            {
                str
            };

        return Enumerable.Range(0, str.Length / chunkSize)
            .Select(i => str.Substring(i * chunkSize, chunkSize));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string SquashWhiteSpaces(this string str)
    {
        str = str.Trim();
        return SquashWhiteSpacesRegex().Replace(str, " ");
    }

    /// <summary>
    /// Regexpr generated at compile-time
    /// </summary>
    [GeneratedRegex("\\s+")]
    private static partial Regex SquashWhiteSpacesRegex();
}