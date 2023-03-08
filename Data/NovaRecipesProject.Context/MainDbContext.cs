using NovaRecipesProject.Context.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NovaRecipesProject.Context;

/// <summary>
/// Main db context of this app
/// </summary>
public class MainDbContext : IdentityDbContext<User, UserRole, Guid>
{
    /// <summary>
    /// DbSet of recipes entities, nothing less or more to say here
    /// </summary>
    public DbSet<Recipe> Recipes { get; set; } = null!;
    /// <summary>
    /// DbSet os categories entites
    /// </summary>
    public DbSet<Category> Categories { get; set; } = null!;

    /// <inheritdoc />
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.SetupUserRelatedEntities();
        modelBuilder.SetupRecipeEntity();
        modelBuilder.SetupCategoryEntity();
    }
}

/// <summary>
/// Extension class that will hold all methods used to configure ModelBuilder in OnModelCreating method in MainDbContext class
/// </summary>
internal static class ModelBuilderExtenstion
{
    /// <summary>
    /// Setting up entity to hold user account data in DB
    /// </summary>
    /// <param name="modelBuilder">Model builder himself</param>
    public static ModelBuilder SetupUserRelatedEntities(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<UserRole>().ToTable("user_roles");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_role_owners").HasKey(p => new { p.UserId, p.RoleId });
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("user_role_claims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");

        return modelBuilder;
    }

    /// <summary>
    /// Setting up entity to hold recipe data in DB
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static ModelBuilder SetupRecipeEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>().ToTable("recipes");
        modelBuilder.Entity<Recipe>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<Recipe>().Property(x => x.Name).HasMaxLength(128);

        return modelBuilder;
    }

    public static ModelBuilder SetupCategoryEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().ToTable("category");
        modelBuilder.Entity<Category>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<Category>().Property(x => x.Name).HasMaxLength(128);

        return modelBuilder;
    }
}