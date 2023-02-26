using NovaRecipesProject.Context.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NovaRecipesProject.Context;

public class MainDbContext : IdentityDbContext<User, UserRole, Guid>
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Setting up entity to hold user account data in DB
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<UserRole>().ToTable("user_roles");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_role_owners").HasKey(p => new { p.UserId, p.RoleId });
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("user_role_claims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
    }
}