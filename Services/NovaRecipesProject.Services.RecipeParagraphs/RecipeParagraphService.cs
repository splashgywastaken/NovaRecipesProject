using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NovaRecipesProject.Common.Exceptions;
using NovaRecipesProject.Common.Extensions;
using NovaRecipesProject.Common.Validator;
using NovaRecipesProject.Context;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Cache;
using NovaRecipesProject.Services.RecipeParagraphs.Models;

namespace NovaRecipesProject.Services.RecipeParagraphs;

/// <inheritdoc />
public class RecipeParagraphService : IRecipeParagraphService
{
    private const string ContextCacheKey = "recipe_paragraph_cache_key";

    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly ICacheService? _cacheService;
    private readonly IModelValidator<AddRecipeParagraphModel> _addIngredientModelValidator;
    private readonly IModelValidator<UpdateRecipeParagraphModel> _updateIngredientModelValidator;

    /// <summary>
    /// Constructor for this class
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <param name="mapper"></param>
    /// <param name="addIngredientModelValidator"></param>
    /// <param name="updateIngredientModelValidator"></param>
    /// <param name="cacheService"></param>
    public RecipeParagraphService(
        IDbContextFactory<MainDbContext> dbContextFactory,
        IMapper mapper,
        IModelValidator<AddRecipeParagraphModel> addIngredientModelValidator,
        IModelValidator<UpdateRecipeParagraphModel> updateIngredientModelValidator,
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
    public async Task<IEnumerable<RecipeParagraphModel>> GetRecipeParagraphsByRecipesId(
        int recipeId, 
        int offset = 0,
        int limit = 10
        )
    {
        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService.Get<IEnumerable<RecipeParagraphModel>?>(ContextCacheKey);
                if (cachedData != null)
                    return cachedData.OrderBy(x => x.OrderNumber);
            }
            catch
            {
                // Ignored
            }
        }

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeParagraphs = context
            .RecipeParagraphs
            .AsQueryable();

        recipeParagraphs = recipeParagraphs
            .Where(x => x.RecipeId == recipeId)
            .SkipAndTake(offset, limit);

        var data =
            (await recipeParagraphs.ToListAsync())
            .Select(_mapper.Map<RecipeParagraphModel>)
            .OrderBy(x => x.OrderNumber)
            .ToList();

        if (_cacheService != null)
            await _cacheService.Put(ContextCacheKey, data, TimeSpan.FromSeconds(30));

        return data;
    }

    /// <inheritdoc />
    public async Task<RecipeParagraphModel> GetRecipeParagraphById(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeParagraph = await context
            .RecipeParagraphs
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        var data = _mapper.Map<RecipeParagraphModel>(recipeParagraph);

        return data;
    }

    /// <inheritdoc />
    public async Task<RecipeParagraphModel> AddRecipeParagraph(int recipeId, AddRecipeParagraphModel model)
    {
        _addIngredientModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeParagraph = _mapper.Map<RecipeParagraph>(model);
        // Adding link to recipe in DB
        recipeParagraph.RecipeId = recipeId;
        await context.RecipeParagraphs.AddAsync(recipeParagraph);
        await context.SaveChangesAsync();

        return _mapper.Map<RecipeParagraphModel>(recipeParagraph);
    }

    /// <inheritdoc />
    public async Task UpdateRecipeParagraph(int id, UpdateRecipeParagraphModel model)
    {
        _updateIngredientModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeParagraph = await context
            .RecipeParagraphs
            .Include(x => x.RecipeId)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        // Copying data to make it so that data in "ingredient" variable wont be changed in the outer scope
        var recipeParagraphCopy = recipeParagraph;
        ProcessException.ThrowIf(() => recipeParagraphCopy is null, $"The recipe paragraph (id: {id}) was not found");

        recipeParagraph = _mapper.Map(model, recipeParagraph);
        // Making sure that relationship was not updated
        recipeParagraph!.RecipeId = recipeParagraphCopy!.RecipeId;

        context.RecipeParagraphs.Update(recipeParagraph);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task ChangeRecipeParagraphOrderNumber(int orderNumber, int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        // Getting all data about recipeParagraph
        var recipeParagraphToUpdate = await context
            .RecipeParagraphs
            .Include(x => x.RecipeId)
            .FirstOrDefaultAsync(x => x.Id == id);

        ProcessException.ThrowIf(() => recipeParagraphToUpdate is null, $"The recipe paragraph (id: {id}) was not found");

        // Applying changes and saving them
        recipeParagraphToUpdate!.OrderNumber = orderNumber;
        context.RecipeParagraphs.Update(recipeParagraphToUpdate);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteRecipeParagraph(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeParagraph = await context.RecipeParagraphs.FirstOrDefaultAsync(x => x.Id.Equals(id))
                         ?? throw new ProcessException(new Exception($"Recipe paragraph (id: {id}) was not found"));

        context.Remove(recipeParagraph);
        await context.SaveChangesAsync();
    }
}