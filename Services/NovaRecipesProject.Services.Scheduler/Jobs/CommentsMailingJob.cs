using System.Runtime.InteropServices.ComTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NovaRecipesProject.Context;
using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.EmailSender.Models;
using NovaRecipesProject.Services.RecipesSubscriptions.Models;
using Quartz;

namespace NovaRecipesProject.Services.RecipesSubscriptions.Jobs;

/// <summary>
/// Job for sending out data about new comments
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class CommentsMailingJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// Constructor. Uses IServiceScopeFactory to get all service while main Job method is working
    /// </summary>
    public CommentsMailingJob(
        IServiceScopeFactory serviceScopeFactory
        )
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>
    /// Executes mailing for all subscribers about new comments for recipes that they are subscribed to 
    /// </summary>
    /// <param name="jobContext"></param>
    public async Task Execute(IJobExecutionContext jobContext)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var dbContextFactory = scope.ServiceProvider.GetService<IDbContextFactory<MainDbContext>>()!;
        var action = scope.ServiceProvider.GetService<IAction>()!;
        var logger = scope.ServiceProvider.GetService<ILogger<CommentsMailingJob>>()!;

        logger.LogInformation("Job CommentsMailingJob started");

        await using var context = await dbContextFactory.CreateDbContextAsync();

        // Getting data for notifications
        var notificationSubscriptions = await context
            .RecipeCommentsSubscriptions
            .Include(rcs => rcs.NewCommentNotifications)
            .ToListAsync();

        // Finishes method work preemptivly if there is no notifications found
        if (notificationSubscriptions.IsNullOrEmpty())
        {
            logger.LogInformation("No pending notifications detected, CommentsMailingJob job finished");
            return;
        }
        else
        {
            logger.LogInformation(
                $"{notificationSubscriptions.Count} notification(s) found, started getting mailing data"
                );
        }

        // Getting data for emails mailing
        var mailingData = await context
            .RecipeCommentsSubscriptions
            // Gets only that data which Ids are in active notifications Ids
            .Where(x =>
                notificationSubscriptions.Any(ns => ns.Id == x.Id))
            .Include(x => x.SubscriptionRecipe)
            .Include(x => x.SubscriptionSubscriber)
            .Select(x => new UserNewRecipeCommentNotificationModel
            {
                UserName = x.SubscriptionSubscriber.FullName,
                UserEmail = x.SubscriptionSubscriber.Email!,
                RecipeName = x.SubscriptionRecipe.Name
            })
            .GroupBy(x => x.UserName)
            .Select(g => new UserNewCommentRecipesMailingModel
            {
                UserName = g.Key,
                UserEmail = g.First().UserEmail,
                RecipeNames = g.Select(x => x.RecipeName).ToList()
            })
            .ToListAsync();
        
        logger.LogInformation($"{mailingData.Count} entry(ies) found, started email sending");

        foreach (var email in mailingData.Select(data => new EmailModel
                 {
                     Email = data.UserEmail,
                     Subject = "Novarecipes notification. New comments",
                     Message = $"There is new comments in recipes:\n•{string.Join("\n•", data.RecipeNames)}"
                 }))
        {
            logger.LogInformation($"Added new email to sent to {email.Email}", email);
            await action.SendNewRecipeCommentNotification(email);
        }

        // After all steps - clear existing notifications data
        foreach (var notification in notificationSubscriptions
                     .SelectMany(subscription => subscription.NewCommentNotifications))
        {
            logger.LogInformation($"Removed notification {notification.SubscriptionId}", notification);
            context
                .NewCommentInRecipeNotifications
                .Remove(notification);
        }

        await context.SaveChangesAsync();

        logger.LogInformation("Job CommentsMailingJob finished");
    }
}