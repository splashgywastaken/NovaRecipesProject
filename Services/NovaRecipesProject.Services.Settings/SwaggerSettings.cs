namespace NovaRecipesProject.Services.Settings;

/// <summary>
/// Swagger settings DTO model
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class SwaggerSettings
{
    /// <summary>
    /// Boolean check to know if swagger need to be enabled or not
    /// </summary>
    public bool Enabled { get; private set; }

    /// <summary>
    /// Client Id to use in OAuth
    /// </summary>
    public string OAuthClientId { get; private set; } = null!;

    /// <summary>
    /// Client secret to use in OAuth
    /// </summary>
    public string OAuthClientSecret { get; private set; } = null!;
    
    /// <summary>
    /// Default constructor that sets check as false
    /// </summary>
    public SwaggerSettings()
    {
        Enabled = false;
    }
}
