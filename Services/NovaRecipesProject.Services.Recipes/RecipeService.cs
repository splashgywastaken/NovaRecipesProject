using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NovaRecipesProject.Common.Exceptions;
using NovaRecipesProject.Common.Extensions;
using NovaRecipesProject.Common.Validator;
using NovaRecipesProject.Context;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Cache;
using NovaRecipesProject.Services.Recipes.Models;
using StackExchange.Redis;

namespace NovaRecipesProject.Services.Recipes;

/// <inheritdoc />
public class RecipeService : IRecipeService
{
    private const string ContextCacheKey = "recipes_cache_key";

    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly ICacheService? _cacheService;
    private readonly IModelValidator<AddRecipeModel> _addRecipeModelValidator;
    private readonly IModelValidator<UpdateRecipeModel> _updateRecipeModelValidator;
    private readonly IModelValidator<UpdateRecipeIngredientModel> _updateRecipeIngredientModelValidator;
    private readonly IModelValidator<AddRecipeIngredientModel> _addRecipeIngredientModelValidator;

    /// <summary>
    /// Constructor for this class
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <param name="mapper"></param>
    /// <param name="addRecipeModelValidator"></param>
    /// <param name="updateRecipeModelValidator"></param>
    /// <param name="addRecipeIngredientModelValidator"></param>
    /// <param name="updateRecipeIngredientModelValidator"></param>
    /// <param name="cacheService"></param>
    public RecipeService(
        IDbContextFactory<MainDbContext> dbContextFactory,
        IMapper mapper,
        IModelValidator<AddRecipeModel> addRecipeModelValidator, 
        IModelValidator<UpdateRecipeModel> updateRecipeModelValidator,
        IModelValidator<AddRecipeIngredientModel> addRecipeIngredientModelValidator, 
        IModelValidator<UpdateRecipeIngredientModel> updateRecipeIngredientModelValidator,
        ICacheService? cacheService = null
        )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _addRecipeModelValidator = addRecipeModelValidator;
        _updateRecipeModelValidator = updateRecipeModelValidator;
        _addRecipeIngredientModelValidator = addRecipeIngredientModelValidator;
        _updateRecipeIngredientModelValidator = updateRecipeIngredientModelValidator;
        _cacheService = cacheService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecipeModel>> GetRecipes(int offset = 0, int limit = 10)
    {
        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService.Get<IEnumerable<RecipeModel>?>(ContextCacheKey);
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
            await _cacheService.Put(ContextCacheKey, data, TimeSpan.FromSeconds(30));

        return data;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecipeModel>> GetUserRecipes(int userId, int offset = 0, int limit = 10)
    {
        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService.Get<IEnumerable<RecipeModel>?>(ContextCacheKey);
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

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipes = context
            .Recipes
            .Where(x => x.RecipeUserId == userId)
            .AsQueryable();

        recipes = recipes
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));

        var data =
            (await recipes.ToListAsync())
            .Select(_mapper.Map<RecipeModel>)
            .ToList();

        if (_cacheService != null)
            await _cacheService.Put(ContextCacheKey, data, TimeSpan.FromSeconds(30));

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
            catch (Exception ex)
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
    public async Task<RecipeModel> GetRecipeById(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipe = await context
            .Recipes
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        var data = _mapper.Map<RecipeModel>(recipe);

        return data;
    }

    /// <inheritdoc />
    public async Task<RecipeModel> AddRecipe(AddRecipeModel model)
    {
        _addRecipeModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipe = _mapper.Map<Recipe>(model);
        await context.Recipes.AddAsync(recipe);
        await context.SaveChangesAsync();

        return _mapper.Map<RecipeModel>(recipe);
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
}