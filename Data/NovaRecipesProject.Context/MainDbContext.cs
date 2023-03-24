using System.Runtime.CompilerServices;
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
    /// <summary>
    /// DbSet of ingredients entities
    /// </summary>
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    /// <summary>
    /// DbSet of recipe paragraphs entities
    /// </summary>
    public DbSet<RecipeParagraph> RecipeParagraphs { get; set; } = null!;

    /// <inheritdoc />
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            // Entities setup
            .SetupUserRelatedEntities()
            .SetupRecipeEntity()
            .SetupCategoryEntity()
            .SetupIngredientEntity()
            .SetupRecipeParagraphEntity()
            // Relationships setup
            .SetupUser1ToRecipesNRelationShip()
            .SetupRecipesNToCategoriesNRelationship()
            .SetupRecipes1ToRecipeParagraphsNRelationship()
            ;
        // Add-Migration AddedSetupUser1ToRecipesNRelationShip -args PostgreSQL
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

    /// <summary>
    /// Setting up entity to hold category data in DB
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <returns></returns>
    public static ModelBuilder SetupCategoryEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Category>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<Category>().Property(x => x.Name).HasMaxLength(128);

        return modelBuilder;
    }

    /// <summary>
    /// Setting up entity to hold ingredient data in DB
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <returns></returns>
    public static ModelBuilder SetupIngredientEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ingredient>().ToTable("ingredients");
        modelBuilder.Entity<Ingredient>().Property(x => x.Carbohydrates).IsRequired();
        modelBuilder.Entity<Ingredient>().Property(x => x.Fat).IsRequired();
        modelBuilder.Entity<Ingredient>().Property(x => x.Proteins).IsRequired();
        modelBuilder.Entity<Ingredient>().Property(x => x.Weight).IsRequired();
        modelBuilder.Entity<Ingredient>().Property(x => x.Portion).HasMaxLength(64);

        return modelBuilder;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <returns></returns>
    public static ModelBuilder SetupRecipeParagraphEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeParagraph>().ToTable("recipeParagraphs");
        modelBuilder.Entity<RecipeParagraph>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<RecipeParagraph>().Property(x => x.Name).HasMaxLength(128);

        return modelBuilder;
    }

    public static ModelBuilder SetupUser1ToRecipesNRelationShip(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(x => x.Recipes)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.RecipeUserId)
            .HasPrincipalKey(x => x.EntryId)
            ;

        return modelBuilder;
    }

    public static ModelBuilder SetupRecipesNToCategoriesNRelationship(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasMany(x => x.Categories)
            .WithMany(x => x.Recipes)
            .UsingEntity(j => j.ToTable("RecipeCategories"))
            ;

        return modelBuilder;
    }

    public static ModelBuilder SetupRecipes1ToRecipeParagraphsNRelationship(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasMany(x => x.RecipeParagraphs)
            .WithOne(x => x.Recipe)
            .HasForeignKey(x => x.RecipeId)
            .HasPrincipalKey(x => x.Id)
            ;

        return modelBuilder;
    }
}