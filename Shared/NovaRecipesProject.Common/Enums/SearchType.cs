namespace NovaRecipesProject.Common.Enums;

/// <summary>
/// Describes search types for filtered search
/// </summary>
public enum SearchType
{
    /// <summary>
    /// Full word match
    /// </summary>
    FullMatch,
    /// <summary>
    /// Partial word match
    /// </summary>
    PartialMatch,
    /// <summary>
    /// A complete match of the word considering the case of characters
    /// </summary>
    LetterCaseFullMatch,
    /// <summary>
    /// Partial match of a word considering the case of characters
    /// </summary>
    LetterCasePartialMatch
}