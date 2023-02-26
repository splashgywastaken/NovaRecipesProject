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
    [BindProperty]
    public bool OpenApiEnabled => _settings.Enabled;

    [BindProperty]
    public string Version => Assembly.GetExecutingAssembly().GetAssemblyVersion();

    [BindProperty]
    public string HelloMessage => _apiSettings.HelloMessage;

    private readonly SwaggerSettings _settings;
    private readonly ApiSpecialSettings _apiSettings;

    public IndexModel(SwaggerSettings settings, ApiSpecialSettings apiSettings)
    {
        _settings = settings;
        _apiSettings = apiSettings;
    }

    public void OnGet()
    {
    }
}
