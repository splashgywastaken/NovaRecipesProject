namespace NovaRecipesProject.Common.Helpers;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

/// <summary>
/// Extension methods for AutoMapper registration
/// </summary>
public static class AutoMappersRegisterHelper
{
    /// <summary>
    /// Registers AutoMapper in IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    public static void Register(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null && s.FullName.ToLower().StartsWith("novarecipesproject."));

        services.AddAutoMapper(assemblies);
    }
}
