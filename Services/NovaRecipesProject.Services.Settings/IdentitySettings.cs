namespace NovaRecipesProject.Services.Settings;

/// <summary>
/// Identity settings DTO model
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class IdentitySettings
{
    /// <summary>
    /// URL for Identity server
    /// </summary>
    public string Url { get; private set; } = null!;
}
