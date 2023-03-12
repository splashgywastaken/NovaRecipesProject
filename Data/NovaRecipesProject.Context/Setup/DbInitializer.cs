using System.Diagnostics;

namespace NovaRecipesProject.Context.Setup;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

/// <summary>
/// Class for initializing DB
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Main execute method which does all the things
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void Execute(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);
        
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>();
        using var context = dbContextFactory.CreateDbContext();
        context.Database.Migrate();
    }
}
