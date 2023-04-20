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
    /// <summary>
    /// DbSet of join entity for recipes and ingredients
    /// </summary>
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; } = null!;
    /// <summary>
    /// DbSet of comments for bottom recipe's page
    /// </summary>
    public DbSet<RecipeComment> RecipeComments { get; set; } = null!;
    /// <summary>
    /// DbSet for managing subscriptions for certain comment sections
    /// </summary>
    public DbSet<RecipeCommentsSubscription> RecipeCommentsSubscriptions { get; set; } = null!;
    /// <summary>
    /// DbSet of email confirmation requests used to work with user's account to confirm user's email
    /// </summary>
    public DbSet<EmailConfirmationRequest> EmailConfirmationRequests { get; set; } = null!;
    /// <summary>
    /// DbSet of subscriptions of some user for some author
    /// </summary>
    public DbSet<RecipesSubscription> RecipesSubscriptions { get; set; } = null!;

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
            // In next two methods I used "now()" to get current date,
            // which means that this works only for PostgreSQL
            // to make it work with other DBMS you need to implement usage of dbcontext settings
            .SetupRecipeCommentsEntity()
            .SetupRecipeCommentsSubscriptionEntity()
            .SetupEmailConfirmationRequests()
            .SetupRecipesSubscription()
            // Relationships setup
            .SetupUser1ToRecipesNRelationShip()
            .SetupRecipesNToCategoriesNRelationship()
            .SetupRecipes1ToRecipeParagraphsNRelationship()
            .SetupRecipesNToIngredientsNRelationshipWithRecipeIngredientsEntity()
            .SetupRecipe1ToRecipeCommentsNRelationship()
            .SetupRecipeCommentsSubscriptionNtoNRelationship()
            .SetupRecipesSubscription1ToNRelationship()
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
    /// Setting up entity to hold data for recipe's paragraphs (aka text of recipe divided into sections)
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

    /// <summary>
    /// Setting up entity to hold data for recipe's ingredients (aka joins for recipe and ingredients table)
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <returns></returns>
    internal static ModelBuilder SetupRecipeIngredientsEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeIngredient>().ToTable("recipeIngredients");
        modelBuilder.Entity<RecipeIngredient>().Property(x => x.Weight).IsRequired();
        modelBuilder.Entity<RecipeIngredient>().Property(x => x.Portion).HasMaxLength(16);

        return modelBuilder;
    }

    /// <summary>
    /// Setting up entity to hold data for recipe's comments (comments in the bottom of recipe page)
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <returns></returns>
    internal static ModelBuilder SetupRecipeCommentsEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeComment>().ToTable("recipeComments");
        modelBuilder.Entity<RecipeComment>()
            .Property(x => x.CreatedDateTime)
            .HasDefaultValueSql("now()");

        return modelBuilder;
    }

    internal static ModelBuilder SetupRecipeCommentsSubscriptionEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeCommentsSubscription>().ToTable("recipeCommentsSubscription");
        modelBuilder.Entity<RecipeCommentsSubscription>()
            .HasIndex(s => new {s.RecipeId, s.SubscriberId})
            .IsUnique();

        return modelBuilder;
    }

    internal static ModelBuilder SetupEmailConfirmationRequests(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailConfirmationRequest>().ToTable("emailConfirmationRequests");
        modelBuilder.Entity<EmailConfirmationRequest>()
            .Property(x => x.RequestCreationDataTime)
            .HasDefaultValueSql("now()");
        
        return modelBuilder;
    }

    internal static ModelBuilder SetupRecipesSubscription(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipesSubscription>().ToTable("userRecipesSubscription");
        modelBuilder.Entity<RecipesSubscription>()
            .HasIndex(s => new {s.SubscriberId, s.AuthorId})
            .IsUnique();

        return modelBuilder;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <returns></returns>
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
            .UsingEntity(j => j.ToTable("recipeCategories"))
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

    internal static ModelBuilder SetupRecipe1ToRecipeCommentsNRelationship(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeComment>()
            .HasOne(rc => rc.Recipe)
            .WithMany(r => r.RecipeComments)
            .HasForeignKey(rc => rc.RecipeId)
            .HasPrincipalKey(r => r.Id)
            ;

        return modelBuilder;
    }

    internal static ModelBuilder SetupRecipesSubscription1ToNRelationship(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipesSubscription>()
            .HasOne(rs => rs.Subscriber)
            .WithMany(u => u.RecipesSubscriptions)
            .HasForeignKey(rs => rs.SubscriberId)
            .HasPrincipalKey(u => u.EntryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipesSubscription>()
            .HasOne(rs => rs.Author)
            .WithMany()
            .HasForeignKey(rs => rs.AuthorId)
            .HasPrincipalKey(u => u.EntryId)
            .OnDelete(DeleteBehavior.Cascade);

        return modelBuilder;
    }

    internal static ModelBuilder SetupRecipeCommentsSubscriptionNtoNRelationship(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeCommentsSubscription>()
            .HasOne(rcs => rcs.Subscriber)
            .WithMany(u => u.RecipeCommentsSubscriptions)
            .HasForeignKey(rcs => rcs.SubscriberId)
            .HasPrincipalKey(u => u.EntryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeCommentsSubscription>()
            .HasOne(rcs => rcs.Recipe)
            .WithMany()
            .HasForeignKey(rcs => rcs.RecipeId)
            .HasPrincipalKey(r => r.Id)
            .OnDelete(DeleteBehavior.Cascade);

        return modelBuilder;
    }
}