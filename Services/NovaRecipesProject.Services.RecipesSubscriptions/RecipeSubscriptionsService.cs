using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovaRecipesProject.Common.Exceptions;
using NovaRecipesProject.Common.Validator;
using NovaRecipesProject.Context;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.EmailSender.Models;
using NovaRecipesProject.Services.RecipesSubscriptions.Models;

namespace NovaRecipesProject.Services.RecipesSubscriptions;

/// <inheritdoc />
public class RecipeSubscriptionsService : IRecipeSubscriptionsService
{
    private readonly IMapper _mapper;
    private readonly IAction _action;
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

    /// <summary>
    /// Constructor for DI
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="action"></param>
    /// <param name="dbContextFactory"></param>
    public RecipeSubscriptionsService(
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
            Message = $"You successfully subscribed to new recipes from {author!.UserName} author",
            From = "nickdur@yandex.ru"
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
            Message = $"You successfully unsubscribed from new recipes from {author!.UserName} author",
            From = "nickdur@yandex.ru"
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

        var subscribers = await context.RecipesSubscriptions
            .Where(rs => rs.AuthorId == authorId)
            .Select(s => s.Subscriber)
            .ToListAsync();

        foreach (var subscriber in subscribers)
        {
            await _action.SendRecipeInfoEmail(new EmailModel
            {
                Email = subscriber.Email!,
                Subject = "NovaRecipes notification",
                // TODO: replace here with working link or some usefull data about recipe
                Message = $"Author {author!.UserName} posted new recipe http://localhost:10000/recipes/{recipe.Id}, go check it out!",
                From = "nickdur@yandex.ru"
            });
        }
    }
}