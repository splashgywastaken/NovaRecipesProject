using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace NovaRecipesProject.Context.Factories; 

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Factory to configure db context
/// </summary>
public static class DbContextOptionsFactory
{
    private const string MigrationProjectPrefix = "NovaRecipesProject.Context.Migrations.";

    /// <summary>
    /// Static method used to create DB context for application
    /// </summary>
    /// <param name="connectionString">
    /// Connection string for DB
    /// </param>
    /// <param name="dbType"></param>
    /// <returns></returns>
    public static DbContextOptions<MainDbContext> Create(string connectionString, Settings.DbType dbType)
    {
        var builder = new DbContextOptionsBuilder<MainDbContext>();

        Configure(connectionString, dbType).Invoke(builder);

        return builder.Options;
    }

    /// <summary>
    /// Static method used to configure DB context
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="dbType"></param>
    /// <returns></returns>
    public static Action<DbContextOptionsBuilder> Configure(string connectionString, Settings.DbType dbType)
    {
        return builder =>
        {
            switch (dbType)
            {
                case Settings.DbType.MSSQL:
                    //builder.UseSqlServer(connectionString,
                    //    opts => opts
                    //        .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                    //        .MigrationsHistoryTable("_EFMigrationsHistory", "public")
                    //        .MigrationsAssembly($"{MigrationProjectPrefix}{Settings.DbType.MSSQL}")
                    //);
                    ConfigureMssql(ref builder, connectionString);
                    break;

                case Settings.DbType.PostgreSQL:
                    //builder.UseNpgsql(connectionString,
                    //    opts => opts
                    //        .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                    //        .MigrationsHistoryTable("_EFMigrationsHistory", "public")
                    //        .MigrationsAssembly($"{MigrationProjectPrefix}{Settings.DbType.PostgreSQL}")
                    //);
                    ConfigurePostgreSql(ref builder, connectionString);
                    break;

                default:
                    //builder.UseNpgsql(connectionString,
                    //    opts => opts
                    //        .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                    //        .MigrationsHistoryTable("_EFMigrationsHistory", "public")
                    //        .MigrationsAssembly($"{MigrationProjectPrefix}{Settings.DbType.PostgreSQL}")
                    //);
                    ConfigurePostgreSql(ref builder, connectionString);
                    break;
            }

            builder.ConfigureWarnings(warnings => warnings
                .Ignore(CoreEventId.RedundantIndexRemoved)
            );
            builder.EnableSensitiveDataLogging();
            builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        };
    }

    /// <summary>
    /// Method used to simplify Configure method in switch block
    /// Configures PostgreSQL for current DbContextOptionsBuilder
    /// </summary>
    /// <param name="builder">
    /// Param used to configure DB in
    /// </param>
    /// <param name="connectionString">
    /// Simply connection string for DB
    /// </param>
    private static void ConfigurePostgreSql(ref DbContextOptionsBuilder builder, string connectionString)
    {
        builder.UseNpgsql(connectionString,
            opts => opts
                .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                .MigrationsHistoryTable("_EFMigrationsHistory", "public")
                .MigrationsAssembly($"{MigrationProjectPrefix}{Settings.DbType.PostgreSQL}")
        );
    }

    /// <summary>
    /// Method used to simplify Configure method in switch block
    /// Configures MSSQL for current DbContextOptionsBuilder
    /// </summary>
    /// <param name="builder">
    /// Param used to configure DB in
    /// </param>
    /// <param name="connectionString">
    /// Simply connection string for DB
    /// </param>
    private static void ConfigureMssql(ref DbContextOptionsBuilder builder, string connectionString)
    {
        builder.UseSqlServer(connectionString,
            opts => opts
                .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                .MigrationsHistoryTable("_EFMigrationsHistory", "public")
                .MigrationsAssembly($"{MigrationProjectPrefix}{Settings.DbType.MSSQL}")
        );
    }
}
