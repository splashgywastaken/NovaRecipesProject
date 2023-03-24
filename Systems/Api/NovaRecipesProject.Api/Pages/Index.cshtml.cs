using NovaRecipesProject.Services.Settings;

namespace NovaRecipesProject.Api.Pages;

using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Common.Extensions;

using Settings;


/// <summary>
/// Index page model
/// </summary>
public class IndexModel : PageModel
{
    /// <summary>
    /// Toggle for OpenAPI to be enabled
    /// </summary>
    [BindProperty]
    public bool OpenApiEnabled => _settings.Enabled;

    /// <summary>
    /// Data about version
    /// </summary>
    [BindProperty]
    public string Version => Assembly.GetExecutingAssembly().GetAssemblyVersion()!;

    /// <summary>
    /// Just hello message, nothing more
    /// </summary>
    [BindProperty]
    public string HelloMessage => _apiSettings.HelloMessage;

    private readonly SwaggerSettings _settings;
    private readonly ApiSpecialSettings _apiSettings;

    /// <inheritdoc />
    public IndexModel(SwaggerSettings settings, ApiSpecialSettings apiSettings)
    {
        _settings = settings;
        _apiSettings = apiSettings;
    }

    /// <summary>
    /// Method used if something gets property data
    /// </summary>
    public void OnGet()
    {
    }
}
