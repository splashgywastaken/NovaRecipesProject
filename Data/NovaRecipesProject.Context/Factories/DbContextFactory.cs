namespace NovaRecipesProject.Context.Factories;

using Microsoft.EntityFrameworkCore;

public class DbContextFactory
{
    private readonly DbContextOptions<MainDbContext> _options;

    public DbContextFactory(DbContextOptions<MainDbContext> options)
    {
        _options = options;
    }

    public MainDbContext Create()
    {
        return new MainDbContext(_options);
    }
}
