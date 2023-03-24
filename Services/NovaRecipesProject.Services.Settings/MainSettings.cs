namespace NovaRecipesProject.Services.Settings;

/// <summary>
/// Main settings DTO model
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class MainSettings
{
    /// <summary>
    /// Url for main API
    /// </summary>
    public string MainUrl { get; private set; } = null!;
}
