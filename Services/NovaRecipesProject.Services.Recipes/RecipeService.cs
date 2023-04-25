using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NovaRecipesProject.Common.Enums;
using NovaRecipesProject.Common.Exceptions;
using NovaRecipesProject.Common.Extensions;
using NovaRecipesProject.Common.Tools;
using NovaRecipesProject.Common.Validator;
using NovaRecipesProject.Context;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Cache;
using NovaRecipesProject.Services.RecipeCommentsSubscriptions;
using NovaRecipesProject.Services.Recipes.Models.RecipeCommentModels;
using NovaRecipesProject.Services.Recipes.Models.RecipeIngredientModels;
using NovaRecipesProject.Services.Recipes.Models.RecipeModels;
using NovaRecipesProject.Services.RecipesSubscriptions;
using NovaRecipesProject.Services.RecipesSubscriptions.Models;
using Npgsql;

namespace NovaRecipesProject.Services.Recipes;

/// <inheritdoc />
public class RecipeService : IRecipeService
{
    private const string ContextCacheKey = "recipes_cache_key";

    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly ICacheService? _cacheService;
    private readonly IRecipeSubscriptionsService _recipeSubscriptionsService;
    private readonly IRecipeCommentsSubscriptionsService _recipeCommentsSubscriptionsService;
    private readonly IModelValidator<AddRecipeModel> _addRecipeModelValidator;
    private readonly IModelValidator<UpdateRecipeModel> _updateRecipeModelValidator;
    private readonly IModelValidator<UpdateRecipeIngredientModel> _updateRecipeIngredientModelValidator;
    private readonly IModelValidator<AddRecipeIngredientModel> _addRecipeIngredientModelValidator;
    private readonly IModelValidator<AddRecipeCommentModel> _addRecipeCommentModelValidator;
    private readonly IModelValidator<UpdateRecipeCommentModel> _updateRecipeCommentModelValidator;

    /// <summary>
    /// Constructor for this class
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <param name="mapper"></param>
    /// <param name="recipeSubscriptionsService"></param>
    /// <param name="addRecipeModelValidator"></param>
    /// <param name="updateRecipeModelValidator"></param>
    /// <param name="addRecipeIngredientModelValidator"></param>
    /// <param name="updateRecipeIngredientModelValidator"></param>
    /// <param name="updateRecipeCommentModelValidator"></param>
    /// <param name="addRecipeCommentModelValidator"></param>
    /// <param name="recipeCommentsSubscriptionsService"></param>
    /// <param name="cacheService"></param>
    public RecipeService(
        IDbContextFactory<MainDbContext> dbContextFactory,
        IMapper mapper,
        IRecipeSubscriptionsService recipeSubscriptionsService,
        IModelValidator<AddRecipeModel> addRecipeModelValidator, 
        IModelValidator<UpdateRecipeModel> updateRecipeModelValidator,
        IModelValidator<AddRecipeIngredientModel> addRecipeIngredientModelValidator, 
        IModelValidator<UpdateRecipeIngredientModel> updateRecipeIngredientModelValidator,
        IModelValidator<UpdateRecipeCommentModel> updateRecipeCommentModelValidator, 
        IModelValidator<AddRecipeCommentModel> addRecipeCommentModelValidator, 
        IRecipeCommentsSubscriptionsService recipeCommentsSubscriptionsService,
        ICacheService? cacheService = null
        )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _recipeSubscriptionsService = recipeSubscriptionsService;
        _addRecipeModelValidator = addRecipeModelValidator;
        _updateRecipeModelValidator = updateRecipeModelValidator;
        _addRecipeIngredientModelValidator = addRecipeIngredientModelValidator;
        _updateRecipeIngredientModelValidator = updateRecipeIngredientModelValidator;
        _updateRecipeCommentModelValidator = updateRecipeCommentModelValidator;
        _addRecipeCommentModelValidator = addRecipeCommentModelValidator;
        _recipeCommentsSubscriptionsService = recipeCommentsSubscriptionsService;
        _cacheService = cacheService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecipeModel>> GetRecipes(int offset = 0, int limit = 10)
    {
        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService.Get<IEnumerable<RecipeModel>?>(ContextCacheKey + "_basic_get");
                // If there are some cached data
                if (cachedData != null)
                {
                    // Enumerating cachedData to evade multiple enumerations
                    var enumeratedCachedData = cachedData.ToList();
                    // If there are less or equal stored cached data than what the limit is 
                    if (enumeratedCachedData.Count <= limit)
                    {
                        return enumeratedCachedData;
                    }
                }
            }
            catch
            {
                // Ignored
            } 
        }

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipes = context
            .Recipes
            .AsQueryable();

        recipes = recipes
            .SkipAndTake(offset, limit);

        var data = 
            (await recipes.ToListAsync())
            .Select(_mapper.Map<RecipeModel>)
            .ToList();

        if (_cacheService != null)
            await _cacheService.Put(
                ContextCacheKey,
                data, 
                TimeSpan.FromSeconds(30)
                );

        return data;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecipeModel>> GetRecipesFiltered(
        string? nameSearchString, 
        SearchType searchType, 
        SortType? sortType,
        List<string>? categoriesList,
        int offset, int limit)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        List<Recipe>? recipes;
        if (nameSearchString != null)
        {
            // Searching for needed recipes using search type value and name of searched recipe
            // Getting data about recipes and categories
            recipes = await (searchType switch
                {
                    SearchType.FullMatch => context.Recipes
                        .Where(p =>
                            string.Equals(p.Name.ToLower(), nameSearchString.ToLower())
                        ),
                    SearchType.LetterCaseFullMatch => context.Recipes
                        .Where(p =>
                            string.Equals(p.Name, nameSearchString)
                        ),
                    SearchType.LetterCasePartialMatch => context.Recipes
                        .Where(p =>
                            p.Name.Contains(nameSearchString)
                        ),
                    SearchType.PartialMatch => context.Recipes
                        .Where(p =>
                            p.Name.ToLower().Contains(nameSearchString.ToLower())
                        ),
                    _ => throw new ArgumentOutOfRangeException(
                        nameof(SearchType),
                        searchType,
                        $"Value for search type is not supported (searchType: {searchType})"
                    )
                })
                .Include(p => p.Categories)
                .ToListAsync();
        }
        else
        {
            recipes = await context
                .Recipes
                .Include(p => p.Categories)
                .ToListAsync();
        }
        
        // Use data about categories to search through recipes
        recipes = recipes
            .Where(
                x =>
                {
                    // If categories of recipe is not null and categories for search also not null
                    // then search could be performed
                    if (!x.Categories.IsNullOrEmpty() && !categoriesList.IsNullOrEmpty())
                        return x.Categories!.Select(c => c.Name).Intersect(categoriesList!).Any();
                    // If there is no categories used for search then accept every data for recipe 
                    // either categories from recipe is null but not the categories used for search
                    // then just decline every data for recipe
                    return categoriesList.IsNullOrEmpty();
                }).ToList();

        // Finally sort data and also take and skip some data
        recipes = (sortType switch
        {
            SortType.NameAsc => recipes.OrderBy(p => p.Name),
            SortType.NameDesc => recipes.OrderByDescending(p => p.Name),
            SortType.IdAsc => recipes.OrderBy(p => p.Id),
            SortType.IdDesc => recipes.OrderByDescending(p => p.Id),
            null => recipes.OrderBy(p => p.Name),
            _ => throw new ArgumentOutOfRangeException(
                nameof(sortType),
                sortType,
                $"Value for sort type is not supported (sortType: {sortType})"
                )
        })
            .SkipAndTake(offset, limit)
            .ToList();
        
        var data = _mapper.Map<IEnumerable<RecipeModel>>(recipes);

        return data;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecipeModel>> GetRecipesAndCacheForUser(string userId, int offset = 0, int limit = 10)
    {
        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService
                    .Get<IEnumerable<RecipeModel>?>(CachingTools.GetContextCacheKey(ContextCacheKey, userId));
                // If there are some cached data
                if (cachedData != null)
                {
                    // Enumerating cachedData to evade multiple enumerations
                    var enumeratedCachedData = cachedData.ToList();
                    // If there are less or equal stored cached data than what the limit is 
                    if (enumeratedCachedData.Count <= limit)
                    {
                        return enumeratedCachedData;
                    }
                }
            }
            catch
            {
                // Ignored
            }
        }

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipes = context
            .Recipes
            .AsQueryable();

        recipes = recipes
            .SkipAndTake(offset, limit);

        var data =
            (await recipes.ToListAsync())
            .Select(_mapper.Map<RecipeModel>)
            .ToList();

        if (_cacheService != null)
            await _cacheService.Put(
                CachingTools.GetContextCacheKey(ContextCacheKey, userId), 
                data,
                TimeSpan.FromSeconds(30)
                );

        return data;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecipeModel>> GetUserRecipes(string userGuid, int offset = 0, int limit = 10)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var userId = (await context
            .Users
            .FirstOrDefaultAsync(x => x.Id.ToString() == userGuid))!.EntryId;

        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService
                    .Get<IEnumerable<RecipeModel>?>(CachingTools.GetContextCacheKey(ContextCacheKey, userGuid));
                // If there are some cached data
                if (cachedData != null)
                {
                    // Enumerating cachedData to evade multiple enumerations
                    var enumeratedCachedData = 
                        cachedData
                            .Where(x => x.RecipeUserId == userId)
                            .ToList();
                    // If there are less or equal stored cached data than what the limit is 
                    if (enumeratedCachedData.Count <= limit)
                    {
                        return enumeratedCachedData;
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        var recipes = context
            .Recipes
            .Where(x => x.RecipeUserId == userId)
            .AsQueryable();

        recipes = recipes
            .SkipAndTake(offset, limit);

        var data =
            (await recipes.ToListAsync())
            .Select(_mapper.Map<RecipeModel>)
            .ToList();

        if (_cacheService != null)
            await _cacheService.Put(
                CachingTools.GetContextCacheKey(ContextCacheKey, userGuid),
                data,
                TimeSpan.FromSeconds(30)
                );

        return data;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecipeCommentLightModel>> GetRecipeComments(int recipeId, int offset = 0, int limit = 10)
    {
        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService.Get<IEnumerable<RecipeCommentLightModel>?>(ContextCacheKey);
                if (cachedData != null)
                {
                    var enumeratedData =
                        cachedData
                            .Where(x => x.RecipeId == recipeId)
                            .ToList();
                    if (enumeratedData.Count <= limit)
                    {
                        return enumeratedData;
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeComments = context
            .RecipeComments
            .Where(x => x.RecipeId == recipeId)
            .OrderBy(x => x.CreatedDateTime)
            .AsQueryable();

        recipeComments = recipeComments
            .SkipAndTake(offset, limit);

        var data = 
            (await recipeComments.ToListAsync())
            .Select(_mapper.Map<RecipeCommentLightModel>)
            .ToList();

        if (_cacheService != null)
            await _cacheService.Put(ContextCacheKey, data, TimeSpan.FromSeconds(30));

        return data;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecipeCommentLightModel>> GetRecipeCommentsAndCacheForUser(
        string userGuid,
        int recipeId,
        int offset = 0, 
        int limit = 10
        )
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var userId = (await context
            .Users
            .FirstOrDefaultAsync(x => x.Id.ToString() == userGuid))!.EntryId;


        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService
                    .Get<IEnumerable<RecipeCommentLightModel>?>(CachingTools.GetContextCacheKey(ContextCacheKey, userGuid));
                if (cachedData != null)
                {
                    var enumeratedData =
                        cachedData
                            .Where(x => x.RecipeId == recipeId)
                            .ToList();
                    if (enumeratedData.Count <= limit)
                    {
                        return enumeratedData;
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
        
        var recipeComments = context
            .RecipeComments
            .Where(x => x.RecipeId == recipeId)
            .OrderBy(x => x.CreatedDateTime)
            .AsQueryable();

        recipeComments = recipeComments
            .SkipAndTake(offset, limit);

        var data =
            (await recipeComments.ToListAsync())
            .Select(_mapper.Map<RecipeCommentLightModel>)
            .ToList();

        if (_cacheService != null)
            await _cacheService.Put(
                CachingTools.GetContextCacheKey(ContextCacheKey, userGuid), 
                data, 
                TimeSpan.FromSeconds(30)
                );

        return data;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecipeIngredientModel>> GetRecipesIngredients(int recipeId)
    {
        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService.Get<IEnumerable<RecipeIngredientModel>?>(ContextCacheKey);
                if (cachedData != null)
                {
                    var enumeratedCachedData =
                        cachedData
                            // Checking that recipes in cache is really what we need
                            .Where(x => x.RecipeId == recipeId)
                            .ToList();
                    if (enumeratedCachedData.Count >= 2)
                        return enumeratedCachedData;
                }
            }
            catch (Exception)
            {
                // Ignored
            }
        }

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeIngredientWithIngredients =
            context.RecipeIngredients
                .Where(x => x.RecipeId == recipeId)
                .Include(x => x.Ingredient)
                .AsQueryable();

        var data = 
                (await recipeIngredientWithIngredients.ToListAsync())
                .Select(_mapper.Map<RecipeIngredientModel>)
                .ToList();

        if (_cacheService != null)
            await _cacheService.Put(ContextCacheKey, data, TimeSpan.FromSeconds(30));

        return data;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecipeIngredientModel>> GetRecipesIngredientsAndCacheForUser(string userGuid, int recipeId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var userId = (await context
            .Users
            .FirstOrDefaultAsync(x => x.Id.ToString() == userGuid))!.EntryId;

        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService
                    .Get<IEnumerable<RecipeIngredientModel>?>(CachingTools.GetContextCacheKey(ContextCacheKey, userGuid));
                if (cachedData != null)
                {
                    var enumeratedCachedData =
                        cachedData
                            // Checking that recipes in cache is really what we need
                            .Where(x => x.RecipeId == recipeId)
                            .ToList();
                    if (enumeratedCachedData.Count >= 2)
                        return enumeratedCachedData;
                }
            }
            catch (Exception)
            {
                // Ignored
            }
        }

        var recipeIngredientWithIngredients =
            context.RecipeIngredients
                .Where(x => x.RecipeId == recipeId)
                .Include(x => x.Ingredient)
                .AsQueryable();

        var data =
            (await recipeIngredientWithIngredients.ToListAsync())
            .Select(_mapper.Map<RecipeIngredientModel>)
            .ToList();

        if (_cacheService != null)
            await _cacheService.Put(
                CachingTools.GetContextCacheKey(ContextCacheKey, userGuid),
                data,
                TimeSpan.FromSeconds(30)
                );

        return data;
    }

    /// <inheritdoc />
    public async Task<WholeRecipeModel> GetRecipeById(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        // Getting recipe with it's categories and paragraphs
        var recipe = await context
            .Recipes
            .Include(x => x.Categories)
            .Include(x => x.RecipeParagraphs)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        ProcessException.ThrowIf(() => recipe is null, $"Recipe was not found (id: {id})");

        // Sort categories
        if (recipe!.Categories is not null)
        {
            recipe!.Categories = recipe!
                .Categories
                .OrderBy(x => x.Id)
                .ToList();
        }
        // Sort recipe's paragraphs
        recipe.RecipeParagraphs = recipe
            .RecipeParagraphs
            .OrderBy(x => x.OrderNumber)
            .ToList();

        var ingredients = 
            (await GetRecipesIngredients(id))
            .OrderBy(x => x.IngredientId)
            .ToList();

        var data = _mapper.Map<WholeRecipeModel>(recipe);
        data.RecipeIngredients = ingredients;

        return data;
    }

    /// <inheritdoc />
    public async Task<RecipeModel> AddRecipeWithUser(int userId, AddRecipeModel model)
    {
        _addRecipeModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        // Checking if that user even exists in the first place:
        var user = await context.Users.FirstOrDefaultAsync(x => x.EntryId.Equals(userId));
        ProcessException.ThrowIf(() => user is null, $"The user (id: {userId}) was not found");

        // Forming data about entity
        var recipe = _mapper.Map<Recipe>(model);
        recipe.RecipeUserId = userId;

        await context.Recipes.AddAsync(recipe);
        await context.SaveChangesAsync();

        var recipeEmailData = new RecipeBaseData
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description
        };

        await _recipeSubscriptionsService
            .NotifySubscribersAboutNewRecipe(recipe.RecipeUserId, recipeEmailData);

        return _mapper.Map<RecipeModel>(recipe);
    }

    /// <inheritdoc />
    public async Task<RecipeIngredientModel> AddIngredientToRecipe(AddRecipeIngredientModel model)
    {
        _addRecipeIngredientModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        // Checking recipe by that Id even exists in the first place:
        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id == model.RecipeId);
        ProcessException.ThrowIf(() => recipe is null, $"The recipe (id: {model.RecipeId}) was not found");

        // Checking ingredient by that Id even exists in the first place:
        var ingredient = await context.Ingredients.FirstOrDefaultAsync(x => x.Id == model.IngredientId);
        ProcessException.ThrowIf(() => ingredient is null, $"The ingredient (id: {model.RecipeId}) was not found");

        // Forming data for new entry
        var recipeIngredient = _mapper.Map<RecipeIngredient>(model);

        await context.RecipeIngredients.AddAsync(recipeIngredient);
        await context.SaveChangesAsync();

        // Setting data for Ingredient property so that mapping would work correctly
        recipeIngredient.Ingredient = ingredient!;
        // Forming data from DB to return
        // Getting weight and portion data from what was added to DB
        var data = _mapper.Map<RecipeIngredientModel>(recipeIngredient);
        
        return data;
    }

    /// <inheritdoc />
    public async Task<RecipeCommentLightModel> AddCommentToRecipe(int recipeId, AddRecipeCommentModel model)
    {
        model.RecipeId = recipeId;
        _addRecipeCommentModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id == recipeId);
        ProcessException.ThrowIf(() => recipe is null, $"The recipe (id: {model.RecipeId}) was not found");
        
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == model.UserId);
        ProcessException.ThrowIf(() => user is null, $"The user (username: {model.UserId}) was not found");

        var recipeComment = _mapper.Map<RecipeComment>(model);
        recipeComment.UserName = user!.FullName;

        await context.RecipeComments.AddAsync(recipeComment);
        await context.SaveChangesAsync();

        var data = _mapper.Map<RecipeCommentLightModel>(recipeComment);

        await _recipeCommentsSubscriptionsService
            .NotifySubscribersAboutNewComment(recipeId);

        return data;
    }

    /// <inheritdoc />
    public async Task UpdateRecipe(int id, UpdateRecipeModel model)
    {
        _updateRecipeModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipe = await context
            .Recipes
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        // Copying data to make it so that data in "recipe" variable wont be changed in the outer scope
        var recipeCopy = recipe;
        ProcessException.ThrowIf(() => recipeCopy is null, $"The recipe (id: {id}) was not found");

        recipe = _mapper.Map(model, recipe);

        // Getting all category Id's from model
        var categoryIds = model.CategoryIds!.Select(x => x.Id);
        // Getting all needed categories from context
        var categories = context
            .Categories
            .Where(c => categoryIds.Contains(c.Id)).ToList();
        // Applying changes
        recipe!.Categories = categories;

        context.Recipes.Update(recipe!);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateRecipeIngredient(int recipeId, int ingredientId, UpdateRecipeIngredientModel model)
    {
        model.RecipeId = recipeId;
        model.IngredientId = ingredientId;
        _updateRecipeIngredientModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeIngredient = await context
            .RecipeIngredients
            .FirstOrDefaultAsync(x => x.RecipeId == recipeId && x.IngredientId == ingredientId);

        var ingredientCopy = recipeIngredient;
        ProcessException.ThrowIf(() => ingredientCopy is null, $"The ingredient (id: {ingredientId}) was not found in recipe (id: {recipeId})");

        recipeIngredient = _mapper.Map(model, recipeIngredient);

        context.RecipeIngredients.Update(recipeIngredient!);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateRecipeComment(int commentId, int recipeId, UpdateRecipeCommentModel model)
    {
        model.RecipeId = recipeId;
        _updateRecipeCommentModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeComment = await context
            .RecipeComments
            .FirstOrDefaultAsync(x => x.Id == commentId);

        var recipeCommentCopy = recipeComment;
        ProcessException.ThrowIf(() => recipeCommentCopy is null, $"The recipe comment (id: {commentId})");

        recipeComment = _mapper.Map(model, recipeComment);

        context.RecipeComments.Update(recipeComment!);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteRecipe(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(id));
        ProcessException.ThrowIf(() => recipe is null, "Recipe (id: {id}) was not found");

        context.Remove(recipe!);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteRecipeIngredient(int recipeId, int ingredientId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeIngredient =
            await context.RecipeIngredients.FirstOrDefaultAsync(x =>
                x.RecipeId == recipeId && x.IngredientId == ingredientId);
        ProcessException.ThrowIf(
            () => recipeIngredient is null,
            $"The ingredient (id: {ingredientId}) was not found in recipe (id: {recipeId})"
            );

        context.RecipeIngredients.Remove(recipeIngredient!);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteRecipeComment(int commentId, int recipeId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeComment =
            await context
                .RecipeComments
                .FirstOrDefaultAsync(x => x.Id == commentId);
        ProcessException.ThrowIf(
            () => recipeComment is null,
            $"The recipe comment (id: {commentId}) was not found"
            );

        context.RecipeComments.Remove(recipeComment!);
        await context.SaveChangesAsync();
    }
}