using System.Diagnostics;

namespace NovaRecipesProject.Context.Factories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

/// <inheritdoc />
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
{
    private const string MigrationProjectPrefix = "NovaRecipesProject.Context.Migrations.";

    /// <inheritdoc />
    public MainDbContext CreateDbContext(string[] args)
    {
        var provider = (args?[0] ?? $"{Settings.DbType.MSSQL}").ToLower();

        var configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.context.json")
             .Build();


        DbContextOptions<MainDbContext> options;
        if (provider.Equals($"{Settings.DbType.MSSQL}".ToLower()))
        {
            options = new DbContextOptionsBuilder<MainDbContext>()
                    .UseSqlServer(
                        configuration.GetConnectionString(provider),
                        opts => opts
                            .MigrationsAssembly($"{MigrationProjectPrefix}{Settings.DbType.MSSQL}")
                    )
                    .Options;
        }
        else
        if (provider.Equals($"{Settings.DbType.PostgreSQL}".ToLower()))
        {
            options = new DbContextOptionsBuilder<MainDbContext>()
                    .UseNpgsql(
                        configuration.GetConnectionString(provider),
                        opts => opts
                            .MigrationsAssembly($"{MigrationProjectPrefix}{Settings.DbType.PostgreSQL}")
                    )
                    .Options;
        }
        else
        {
            throw new Exception($"Unsupported provider: {provider}");
        }

        var dbf = new DbContextFactory(options);
        return dbf.Create();
    }
}
