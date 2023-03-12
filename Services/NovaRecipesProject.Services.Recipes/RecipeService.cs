using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NovaRecipesProject.Common.Exceptions;
using NovaRecipesProject.Common.Validator;
using NovaRecipesProject.Context;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Cache;
using NovaRecipesProject.Services.Recipes.Models;

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

    /// <summary>
    /// Constructor for this class
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <param name="mapper"></param>
    /// <param name="addRecipeModelValidator"></param>
    /// <param name="updateRecipeModelValidator"></param>
    /// <param name="cacheService"></param>
    public RecipeService(
        IDbContextFactory<MainDbContext> dbContextFactory,
        IMapper mapper,
        IModelValidator<AddRecipeModel> addRecipeModelValidator, 
        IModelValidator<UpdateRecipeModel> updateRecipeModelValidator,
        ICacheService? cacheService = null
        )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _addRecipeModelValidator = addRecipeModelValidator;
        _updateRecipeModelValidator = updateRecipeModelValidator;
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
                if (cachedData != null)
                    return cachedData;
            }
            catch
            {
                // ignored
            } 
        }

        await Task.Delay(500);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipes = context
            .Recipes
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
    public async Task UpdateRecipe(int id, UpdateRecipeModel model)
    {
        _updateRecipeModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(id));

        // Copying data to make it so that data in "recipe" variable wont be changed in the outer scope
        var recipeCopy = recipe;
        ProcessException.ThrowIf(() => recipeCopy is null, $"The recipe (id: {id}) was not found");

        recipe = _mapper.Map(model, recipe);

        context.Recipes.Update(recipe!);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteRecipe(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(id))
                     ?? throw new ProcessException(new Exception($"Recipe (id: {id}) was not found"));

        context.Remove(recipe);
        await context.SaveChangesAsync();
    }
}