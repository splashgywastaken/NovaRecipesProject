using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Context.Entities.MainData;

namespace NovaRecipesProject.Context.Setup;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class that is used to populate DB with Data
/// </summary>
public static class DbSeeder
{
    private static IServiceScope ServiceScope(IServiceProvider serviceProvider) => serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();
    private static MainDbContext DbContext(IServiceProvider serviceProvider) => ServiceScope(serviceProvider).ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>().CreateDbContext();
    
    /// <summary>
    /// Basic method for launching all other methods of DbSeeder
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="addDemoData"></param>
    /// <param name="addAdmin"></param>
    public static void Execute(IServiceProvider serviceProvider, bool addDemoData, bool addAdmin = true)
    {
        using var scope = ServiceScope(serviceProvider);
        ArgumentNullException.ThrowIfNull(scope);

        if (addAdmin)
        {
            // ConfigureAdministrator(scope);
        }

        if (addDemoData)
        {
            Task.Run(async () =>
            {
                await ConfigureDemoData(serviceProvider);
            });
        }
    }

    //private static void ConfigureAdministrator(IServiceScope scope)
    //{
    //    var defaults = scope.ServiceProvider.GetService<IDefaultsSettings>();

    //    if (defaults != null)
    //    {
    //        var userService = scope.ServiceProvider.GetService<IUserService>();
    //        if (addAdmin && (!userService?.Any() ?? false))
    //        {
    //            var user = userService.Create(new CreateUserModel
    //                {
    //                    Type = UserType.ForPortal,
    //                    Status = UserStatus.Active,
    //                    Name = defaults.AdministratorName,
    //                    Password = defaults.AdministratorPassword,
    //                    Email = defaults.AdministratorEmail,
    //                    IsEmailConfirmed = !defaults.AdministratorEmail.IsNullOrEmpty(),
    //                    Phone = null,
    //                    IsPhoneConfirmed = false,
    //                    IsChangePasswordNeeded = true
    //                })
    //                .GetAwaiter()
    //                .GetResult();

    //            userService.SetUserRoles(user.Id, DbLoggerCategory.Infrastructure.User.UserRole.Administrator).GetAwaiter().GetResult();
    //        }
    //    }
    //}
    
    private static async Task ConfigureDemoData(IServiceProvider serviceProvider)
    {
        await AddRecipes(serviceProvider);
    }

    private static async Task AddRecipes(IServiceProvider serviceProvider)
    {
        await using var context = DbContext(serviceProvider);

        var a1 = new Recipe()
        {
            Name = "Мимоза",
            Description = "Вкусный такой салат короче"
        };
        context.Recipes.Add(a1);

        var a2 = new Recipe()
        {
            Name = "Оливье",
            Description = "Тоже вкусный такой салат короче"
        };
        context.Recipes.Add(a2);

        await context.SaveChangesAsync();
    }
}