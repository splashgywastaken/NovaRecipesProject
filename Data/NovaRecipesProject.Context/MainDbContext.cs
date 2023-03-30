using System.Runtime.CompilerServices;
using NovaRecipesProject.Context.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
    /// <summary>
    /// DbSet of join entity for recipes and ingredients
    /// </summary>
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; } = null!;

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
            .SetupRecipeIngredientsEntity()
            // Relationships setup
            .SetupUser1ToRecipesNRelationShip()
            .SetupRecipesNToCategoriesNRelationship()
            .SetupRecipes1ToRecipeParagraphsNRelationship()
            .SetupRecipesNToIngredientsNRelationshipWithRecipeIngredientsEntity()
            ;
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
    internal static ModelBuilder SetupUserRelatedEntities(this ModelBuilder modelBuilder)
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
    internal static ModelBuilder SetupRecipeEntity(this ModelBuilder modelBuilder)
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
    internal static ModelBuilder SetupCategoryEntity(this ModelBuilder modelBuilder)
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
    internal static ModelBuilder SetupIngredientEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ingredient>().ToTable("ingredients");
        modelBuilder.Entity<Ingredient>().Property(x => x.Carbohydrates).IsRequired();
        modelBuilder.Entity<Ingredient>().Property(x => x.Fat).IsRequired();
        modelBuilder.Entity<Ingredient>().Property(x => x.Proteins).IsRequired();

        return modelBuilder;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <returns></returns>
    internal static ModelBuilder SetupRecipeParagraphEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeParagraph>().ToTable("recipeParagraphs");
        modelBuilder.Entity<RecipeParagraph>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<RecipeParagraph>().Property(x => x.Name).HasMaxLength(128);

        return modelBuilder;
    }

    internal static ModelBuilder SetupRecipeIngredientsEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeIngredient>().ToTable("recipeIngredients");
        modelBuilder.Entity<RecipeIngredient>().Property(x => x.Weight).IsRequired();
        modelBuilder.Entity<RecipeIngredient>().Property(x => x.Portion).HasMaxLength(16);

        return modelBuilder;
    }

    internal static ModelBuilder SetupUser1ToRecipesNRelationShip(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(x => x.Recipes)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.RecipeUserId)
            .HasPrincipalKey(x => x.EntryId)
            ;

        return modelBuilder;
    }

    internal static ModelBuilder SetupRecipesNToCategoriesNRelationship(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasMany(x => x.Categories)
            .WithMany(x => x.Recipes)
            .UsingEntity(j => j.ToTable("RecipeCategories"))
            ;

        return modelBuilder;
    }

    internal static ModelBuilder SetupRecipes1ToRecipeParagraphsNRelationship(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasMany(x => x.RecipeParagraphs)
            .WithOne(x => x.Recipe)
            .HasForeignKey(x => x.RecipeId)
            .HasPrincipalKey(x => x.Id)
            ;

        return modelBuilder;
    }

    internal static ModelBuilder SetupRecipesNToIngredientsNRelationshipWithRecipeIngredientsEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.RecipeIngredients)
            .HasForeignKey(ri => ri.RecipeId)
            ;

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientId)
            ;

        return modelBuilder;
    }
}