using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NovaRecipesProject.Common.Exceptions;
using NovaRecipesProject.Context;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.EmailSender.Models;

namespace NovaRecipesProject.Services.RecipeCommentsSubscriptions;

/// <inheritdoc />
public class RecipeCommentsSubscriptionsService : IRecipeCommentsSubscriptionsService
{
    private readonly IMapper _mapper;
    private readonly IAction _action;
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="action"></param>
    /// <param name="dbContextFactory"></param>
    public RecipeCommentsSubscriptionsService(
        IMapper mapper,
        IAction action,
        IDbContextFactory<MainDbContext> dbContextFactory
        )
    {
        _mapper = mapper;
        _action = action;
        _dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public async Task Subscribe(int subscriberId, int recipeId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        // Checking if user with Id as subsriberId even exists
        var subscriber = await context
            .Users
            .FirstOrDefaultAsync(x => x.EntryId == subscriberId);
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
            .FirstOrDefaultAsync(x => x.RecipeId == recipeId && x.SubscriberId == subscriberId);
        ProcessException.ThrowIf(
            () => subscriptionInContext is not null,
            $"User (subscriber Id: {subscriberId})" +
            $" is already subscribed for comments in recipe (recipe Id {recipeId})"
            );

        var subscription = new RecipeCommentsSubscription()
        {
            SubscriberId = subscriberId,
            RecipeId = recipeId
        };

        await context.RecipeCommentsSubscriptions.AddAsync(subscription);
        await context.SaveChangesAsync();

        await _action.SendRecipeInfoEmail(new EmailModel
        {
            Email = subscriber!.Email!,
            Subject = "NovaRecipes notification",
            Message = $"You successfully subscribed to new comments for recipe {recipe!.Name}",
            From = "nickdur@yandex.ru"
        });
    }

    /// <inheritdoc />
    public async Task Unsubscribe(int subscriberId, int recipeId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        // Checking if user with Id as subsriberId even exists
        var subscriber = await context
            .Users
            .FirstOrDefaultAsync(x => x.EntryId == subscriberId);
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
            s => s.SubscriberId == subscriberId && s.RecipeId == recipeId);
        ProcessException.ThrowIf(
            () => subscription is null,
            $"Subscription for user (user Id: {subscriberId}) and author (author Id: {recipeId}) was not found");

        context.RecipeCommentsSubscriptions.Remove(subscription!);
        await context.SaveChangesAsync();


        await _action.SendRecipeInfoEmail(new EmailModel
        {
            Email = subscriber!.Email!,
            Subject = "NovaRecipes notification",
            Message = $"You successfully unsubscribed from new recipes from {recipe!.Name} author",
            From = "nickdur@yandex.ru"
        });
    }

    /// <inheritdoc />
    public async Task NotifySubscribersAboutNewComment(int recipeId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipe = await context
            .Recipes
            .FirstOrDefaultAsync(x => x.Id == recipeId);
        ProcessException.ThrowIf(() => recipe is null, $"The recipe (recipe Id {recipeId}) was not found");

        var subscriptions = await context
            .RecipeCommentsSubscriptions
            .Where(x => x.RecipeId == recipeId)
            .Include(x => x.Recipe)
            .Include(x => x.Subscriber)
            .ToListAsync();

        // If there is no subscriptions finish method preemptively
        if (subscriptions.IsNullOrEmpty())
        {
            return;
        }
        // Else notify every user that subscribed to this
        foreach (var email in subscriptions.Select(subscription => new EmailModel
                 {
                     Email = subscription.Subscriber.Email!,
                     Subject = "NovaRecipes notification",
                     Message = $"Someone wrote new comment for \"{recipe!.Name}\" recipe. Go, check it out!",
                     From = "nickdur@yandex.ru"
                 }))
        {
            await _action.SendNewRecipeCommentNotification(email);
        }
    }
}