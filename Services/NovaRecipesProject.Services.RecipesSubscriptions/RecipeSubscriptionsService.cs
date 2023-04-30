using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using NovaRecipesProject.Common.Exceptions;
using NovaRecipesProject.Common.Validator;
using NovaRecipesProject.Context;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Context.Entities.MailingAndSubscriptions;
using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.EmailSender.Models;
using NovaRecipesProject.Services.RecipesSubscriptions.Models;

namespace NovaRecipesProject.Services.RecipesSubscriptions;

/// <inheritdoc />
public class RecipeSubscriptionsService : IRecipeSubscriptionsService
{
    private readonly ILogger<RecipeSubscriptionsService> _logger;
    private readonly IMapper _mapper;
    private readonly IAction _action;
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

    /// <summary>
    /// Constructor for DI
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="action"></param>
    /// <param name="dbContextFactory"></param>
    /// <param name="logger"></param>
    public RecipeSubscriptionsService(
        IMapper mapper,
        IAction action, 
        IDbContextFactory<MainDbContext> dbContextFactory, 
        ILogger<RecipeSubscriptionsService> logger
        )
    {
        _mapper = mapper;
        _action = action;
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Subscribe(int subscriberId, int authorId)
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

        var author = await context
            .Users
            .FirstOrDefaultAsync(x => x.EntryId == authorId);
        ProcessException.ThrowIf(
            () => author is null,
            $"The user (author Id: {authorId} was not found)"
            );

        var subscriptionInContext = await context
            .RecipesSubscriptions
            .FirstOrDefaultAsync(x => x.AuthorId == authorId && x.SubscriberId == subscriberId);
        ProcessException.ThrowIf(
            () => subscriptionInContext is not null,
            $"User (subscriber Id: {subscriberId}) " +
            $"is already subscribed for new recipes from author (author Id {authorId})"
        );

        var subscription = new RecipesSubscription()
        {
            SubscriberId = subscriberId,
            AuthorId = authorId
        };

        await context.RecipesSubscriptions.AddAsync(subscription);
        await context.SaveChangesAsync();

        await _action.SendRecipeInfoEmail(new EmailModel
        {
            Email = subscriber!.Email!,
            Subject = "NovaRecipes notification",
            Message = $"You successfully subscribed to new recipes from {author!.FullName} author"
        });
    }

    /// <inheritdoc />
    public async Task Unsubscribe(int subscriberId, int authorId)
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
        var author = await context
            .Users
            .FirstOrDefaultAsync(x => x.EntryId == authorId);
        ProcessException.ThrowIf(
            () => author is null,
            $"The user (author Id: {authorId} was not found)"
        );

        // Getting from DB data about subscription
        var subscription = await context.RecipesSubscriptions.SingleOrDefaultAsync(
            s => s.SubscriberId == subscriberId && s.AuthorId == authorId);
        ProcessException.ThrowIf(
            () => subscription is null,
            $"Subscription for user (user Id: {subscriberId}) and author (author Id: {authorId}) was not found");
        
        context.RecipesSubscriptions.Remove(subscription!);
        await context.SaveChangesAsync();
    
        
        await _action.SendRecipeInfoEmail(new EmailModel
        {
            Email = subscriber!.Email!,
            Subject = "NovaRecipes notification",
            Message = $"You successfully unsubscribed from new recipes from {author!.FullName} author"
        });
    }

    /// <inheritdoc />
    public async Task NotifySubscribersAboutNewRecipe(int authorId, RecipeBaseData recipe)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        // Checking if author exists
        var author = await context
            .Users
            .FirstOrDefaultAsync(x => x.EntryId == authorId);

        ProcessException.ThrowIf(
            () => author is null,
            $"The user (author Id: {authorId} was not found)"
        );

        var subscribersIds = await context
            .RecipesSubscriptions
            .Where(rs => rs.AuthorId == authorId)
            .Select(rs => rs.SubscriberId)
            .ToListAsync();
        var subscribers = await context
            .Users
            .Where(u => subscribersIds.Contains(u.EntryId))
            .ToListAsync();

        foreach (var subscriber in subscribers)
        {
            _logger.LogInformation($"Sending email about new recipe to subscriber {subscriber.FullName}");
            await _action.SendRecipeInfoEmail(new EmailModel
            {
                Email = subscriber.Email!,
                Subject = "NovaRecipes notification",
                Message = $"Author {author!.UserName} posted new recipe {recipe.Name}, go check it out!"
            });
        }
    }
}