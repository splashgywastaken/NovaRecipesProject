namespace NovaRecipesProject.Context.Factories;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Class for factory responsible for creating DbContext
/// </summary>
public class DbContextFactory
{
    private readonly DbContextOptions<MainDbContext> _options;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options"></param>
    public DbContextFactory(DbContextOptions<MainDbContext> options)
    {
        _options = options;
    }

    /// <summary>
    /// Create method (for creating)
    /// </summary>
    /// <returns></returns>
    public MainDbContext Create()
    {
        return new MainDbContext(_options);
    }
}
