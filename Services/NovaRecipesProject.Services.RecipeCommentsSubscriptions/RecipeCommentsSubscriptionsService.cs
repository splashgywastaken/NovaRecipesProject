using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NovaRecipesProject.Common.Exceptions;
using NovaRecipesProject.Context;
using NovaRecipesProject.Context.Entities.MailingAndSubscriptions;
using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.EmailSender.Models;

namespace NovaRecipesProject.Services.RecipeCommentsSubscriptions;

/// <inheritdoc />
public class RecipeCommentsSubscriptionsService : IRecipeCommentsSubscriptionsService
{
    private readonly ILogger<RecipeCommentsSubscriptionsService> _logger;
    private readonly IMapper _mapper;
    private readonly IAction _action;
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="action"></param>
    /// <param name="dbContextFactory"></param>
    /// <param name="logger"></param>
    public RecipeCommentsSubscriptionsService(
        IMapper mapper,
        IAction action,
        IDbContextFactory<MainDbContext> dbContextFactory, 
        ILogger<RecipeCommentsSubscriptionsService> logger
        )
    {
        _mapper = mapper;
        _action = action;
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Subscribe(string subscriberId, int recipeId)
    {
        _logger.LogInformation(new EventId(101), $"Subscriber Id is {subscriberId}, recipe Id is {recipeId}");
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        // Checking if user with Id as subsriberId even exists
        var subscriber = await context
            .Users
            .FirstOrDefaultAsync(x => x.Id.ToString() == subscriberId);
        ProcessException.ThrowIf(
            () => subscriber is null,
            $"The user (subscriber Id: {subscriberId}) was not found"
        );

        var recipe = await context
            .Recipes
            .FirstOrDefaultAsync(x => x.Id == recipeId);
        ProcessException.ThrowIf(
            () => recipe is null,
            $"The recipe (recipe Id: {recipeId} was not found)"
        );

        var subscriptionInContext = await context
            .RecipeCommentsSubscriptions
            .FirstOrDefaultAsync(x =>
                x.SubscriptionRecipeId == recipeId && x.SubscriptionSubscriberId == subscriber!.EntryId);
        ProcessException.ThrowIf(
            () => subscriptionInContext is not null,
            $"User (subscriber Id: {subscriberId})" +
            $" is already subscribed for comments in recipe (recipe Id {recipeId})"
            );

        var subscription = new RecipeCommentsSubscription()
        {
            SubscriptionSubscriberId = subscriber!.EntryId,
            SubscriptionRecipeId = recipe!.Id
        };

        await context.RecipeCommentsSubscriptions.AddAsync(subscription);
        await context.SaveChangesAsync();

        await _action.SendRecipeInfoEmail(new EmailModel
        {
            Email = subscriber.Email!,
            Subject = "NovaRecipes notification",
            Message = $"You successfully subscribed to new comments for recipe {recipe.Name}"
        });
    }

    /// <inheritdoc />
    public async Task Unsubscribe(string subscriberId, int recipeId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        // Checking if user with Id as subsriberId even exists
        var subscriber = await context
            .Users
            .FirstOrDefaultAsync(x => x.Id.ToString() == subscriberId);
        ProcessException.ThrowIf(
            () => subscriber is null,
            $"The user (subscriber Id: {subscriberId}) was not found"
        );

        // Doing exact same thing for author
        var recipe = await context
            .Recipes    
            .FirstOrDefaultAsync(x => x.Id == recipeId);
        ProcessException.ThrowIf(
            () => recipe is null,
            $"The user (author Id: {recipeId} was not found)"
        );

        // Getting from DB data about subscription
        var subscription = await context.RecipeCommentsSubscriptions.SingleOrDefaultAsync(
            s => 
                s.SubscriptionSubscriberId == subscriber!.EntryId && s.SubscriptionRecipeId == recipeId);
        ProcessException.ThrowIf(
            () => subscription is null,
            $"Subscription for user (user Id: {subscriberId}) and author (author Id: {recipeId}) was not found");

        context.RecipeCommentsSubscriptions.Remove(subscription!);
        await context.SaveChangesAsync();


        await _action.SendRecipeInfoEmail(new EmailModel
        {
            Email = subscriber!.Email!,
            Subject = "NovaRecipes notification",
            Message = $"You successfully unsubscribed from new recipes from {recipe!.Name} author"
        });
    }

    /// <inheritdoc />
    public async Task NotifySubscribersAboutNewComment(int recipeId)
    {
        _logger.LogInformation($"Adding notification for subscribers about new comments to recipe {recipeId}");

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipe = await context
            .Recipes
            .FirstOrDefaultAsync(x => x.Id == recipeId);
        ProcessException.ThrowIf(() => recipe is null, $"The recipe (recipe Id {recipeId}) was not found");

        var subscriptions = await context
            .RecipeCommentsSubscriptions
            .Where(x => x.SubscriptionRecipeId == recipeId)
            .Include(x => x.SubscriptionRecipe)
            .Include(x => x.SubscriptionSubscriber)
            .ToListAsync();

        // If there is no subscriptions finish method preemptively
        if (subscriptions.IsNullOrEmpty())
        {
            _logger.LogInformation($"No subscriptions found, finishing notifications process");
            return;
        }

        ////// New way of doing things
        //// Else make new entries for notifications:
        //foreach
        //(
        //    var newCommentInRecipeNotification in subscriptions
        //        .Select(subscription => new NewCommentInRecipeNotification
        //        {
        //            SubscriptionId = subscription.Id
        //        })
        //)
        //{
        //    _logger.LogInformation($"Working with subscription {newCommentInRecipeNotification.SubscriptionId}");

        //    var notification = await context
        //        .NewCommentInRecipeNotifications
        //        .FirstOrDefaultAsync(
        //            x =>
        //                x.SubscriptionId == newCommentInRecipeNotification.SubscriptionId
        //        );
            
        //    if (notification is not null)
        //    {
        //        _logger.LogInformation("Notification is not null");
        //        continue;
        //    }
        //    _logger.LogInformation($"Adding new notification {newCommentInRecipeNotification}");
        //    // If there is no such notification for this exact subscription then add new one
        //    await context
        //        .NewCommentInRecipeNotifications
        //        .AddAsync(newCommentInRecipeNotification);
        //    await context.SaveChangesAsync();
        //}

        //// Old way of doing things, use this to make application send emails right at the moment when
        //// new comment is posted 
        // Else notify every user that subscribed to this
        foreach (var email in subscriptions.Select(subscription => new EmailModel
        {
            Email = subscription.SubscriptionSubscriber.Email!,
            Subject = "NovaRecipes notification",
            Message = $"Someone wrote new comment for \"{recipe!.Name}\" recipe. Go, check it out!"
        }))
        {
            await _action.SendNewRecipeCommentNotification(email);
        }

        _logger.LogInformation($"Successfully finishing notifications process");
    }
}