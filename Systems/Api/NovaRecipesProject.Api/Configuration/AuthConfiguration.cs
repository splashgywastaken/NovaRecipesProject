namespace NovaRecipesProject.Api.Configuration;

using Common.Security;
using Context;
using Context.Entities;
using Services.Settings;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Class for authentication configuration
/// </summary>
public static class AuthConfiguration
{
    /// <summary>
    /// Extension method for adding authentication
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppAuth(this IServiceCollection services, IdentitySettings settings)
    {
        IdentityModelEventSource.ShowPII = true;

        services
            .AddIdentity<User, IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequiredLength = 0;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<MainDbContext>()
            .AddUserManager<UserManager<User>>()
            .AddDefaultTokenProviders();
        
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = settings.Url.StartsWith("https://");
                options.Authority = settings.Url;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "email",
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Audience = "api";
            });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                AppScopes.UsersView,
                policy => policy.RequireClaim("scope", AppScopes.UsersView)
            );
            options.AddPolicy(
                AppScopes.UsersModerate,
                policy => policy.RequireClaim("scope", AppScopes.UsersModerate)
                );
            options.AddPolicy(
                AppScopes.AllRead,
                policy => policy.RequireClaim("scope", AppScopes.AllRead)
            );
            options.AddPolicy(
                AppScopes.AllEdit,
                policy => policy.RequireClaim("scope", AppScopes.AllEdit)
            );
            options.AddPolicy(
                AppScopes.CategoriesRead,
                policy => policy.RequireClaim("scope", AppScopes.CategoriesRead)
            );
            options.AddPolicy(
                AppScopes.CategoriesEdit,
                policy => policy.RequireClaim("scope", AppScopes.CategoriesEdit)
            );
            options.AddPolicy(
                AppScopes.IngredientsRead,
                policy => policy.RequireClaim("scope", AppScopes.IngredientsRead)
            );
            options.AddPolicy(
                AppScopes.IngredientsEdit,
                policy => policy.RequireClaim("scope", AppScopes.IngredientsEdit)
            );
            options.AddPolicy(
                AppScopes.RecipeParagraphsRead,
                policy => policy.RequireClaim("scope", AppScopes.RecipeParagraphsRead)
            );
            options.AddPolicy(
                AppScopes.RecipeParagraphsEdit,
                policy => policy.RequireClaim("scope", AppScopes.RecipeParagraphsEdit)
            );
            options.AddPolicy(
                AppScopes.RecipesRead, 
                policy => policy.RequireClaim("scope", AppScopes.RecipesRead)
                );
            options.AddPolicy(
                AppScopes.RecipesEdit,
                policy => policy.RequireClaim("scope", AppScopes.RecipesEdit)
            );
        });

        return services;
    }

    /// <summary>
    /// Extension method for enabling authentication
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAppAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();

        app.UseAuthorization();
        
        return app;
    }
}