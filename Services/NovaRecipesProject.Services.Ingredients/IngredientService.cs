using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NovaRecipesProject.Common.Exceptions;
using NovaRecipesProject.Common.Extensions;
using NovaRecipesProject.Common.Validator;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Context;
using NovaRecipesProject.Services.Cache;
using NovaRecipesProject.Services.Ingredients.Models;
using NovaRecipesProject.Common.Tools;

namespace NovaRecipesProject.Services.Ingredients;

/// <inheritdoc />
public class IngredientService : IIngredientService
{
    private const string ContextCacheKey = "ingredients_cache_key";

    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly ICacheService? _cacheService;
    private readonly IModelValidator<AddIngredientModel> _addIngredientModelValidator;
    private readonly IModelValidator<UpdateIngredientModel> _updateIngredientModelValidator;

    /// <summary>
    /// Constructor for this class
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <param name="mapper"></param>
    /// <param name="addIngredientModelValidator"></param>
    /// <param name="updateIngredientModelValidator"></param>
    /// <param name="cacheService"></param>
    public IngredientService(
        IDbContextFactory<MainDbContext> dbContextFactory,
        IMapper mapper,
        IModelValidator<AddIngredientModel> addIngredientModelValidator,
        IModelValidator<UpdateIngredientModel> updateIngredientModelValidator,
        ICacheService? cacheService = null
        )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _addIngredientModelValidator = addIngredientModelValidator;
        _updateIngredientModelValidator = updateIngredientModelValidator;
        _cacheService = cacheService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IngredientModel>> GetIngredients(int offset = 0, int limit = 10)
    {
        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService.Get<IEnumerable<IngredientModel>?>(ContextCacheKey);
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
                // ignored
            }
        }

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var ingredients = context
            .Ingredients
            .AsQueryable();

        ingredients = ingredients
            .SkipAndTake(offset, limit);

        var data =
            (await ingredients.ToListAsync())
            .Select(_mapper.Map<IngredientModel>)
            .ToList();

        if (_cacheService != null)
            await _cacheService.Put(ContextCacheKey, data, TimeSpan.FromSeconds(30));

        return data;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IngredientModel>> GetIngredientsAndCacheForUser(
        string userId,
        int offset = 0,
        int limit = 10
        )
    {
        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService
                    .Get<IEnumerable<IngredientModel>?>(CachingTools.GetContextCacheKey(ContextCacheKey, userId));
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
                // ignored
            }
        }

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var ingredients = context
            .Ingredients
            .AsQueryable();

        ingredients = ingredients
            .SkipAndTake(offset, limit);

        var data =
            (await ingredients.ToListAsync())
            .Select(_mapper.Map<IngredientModel>)
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
    public async Task<IngredientModel> GetIngredientById(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var ingredient = await context
            .Ingredients
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        var data = _mapper.Map<IngredientModel>(ingredient);

        return data;
    }

    /// <inheritdoc />
    public async Task<IngredientModel> AddIngredient(AddIngredientModel model)
    {
        _addIngredientModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var ingredient = _mapper.Map<Ingredient>(model);
        await context.Ingredients.AddAsync(ingredient);
        await context.SaveChangesAsync();

        return _mapper.Map<IngredientModel>(ingredient);
    }

    /// <inheritdoc />
    public async Task UpdateIngredient(int id, UpdateIngredientModel model)
    {
        _updateIngredientModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var ingredient = await context.Ingredients.FirstOrDefaultAsync(x => x.Id.Equals(id));

        // Copying data to make it so that data in "ingredient" variable wont be changed in the outer scope
        var ingredientCopy = ingredient;
        ProcessException.ThrowIf(() => ingredientCopy is null, $"The ingredient (id: {id}) was not found");

        ingredient = _mapper.Map(model, ingredient);

        context.Ingredients.Update(ingredient!);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteIngredient(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var ingredient = await context.Ingredients.FirstOrDefaultAsync(x => x.Id.Equals(id))
                       ?? throw new ProcessException(new Exception($"Ingredient (id: {id}) was not found"));

        context.Remove(ingredient);
        await context.SaveChangesAsync();
    }
}