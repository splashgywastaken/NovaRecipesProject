using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NovaRecipesProject.Common.Exceptions;
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
    public async Task<IEnumerable<RecipeParagraphModel>> GetRecipeParagraphs(int offset = 0, int limit = 10)
    {
        if (_cacheService != null)
        {
            try
            {
                var cachedData = await _cacheService.Get<IEnumerable<RecipeParagraphModel>?>(ContextCacheKey);
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

        var recipeParagraphs = context
            .RecipeParagraphs
        .AsQueryable();

        recipeParagraphs = recipeParagraphs
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));

        var data =
            (await recipeParagraphs.ToListAsync())
            .Select(_mapper.Map<RecipeParagraphModel>)
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
    public async Task<RecipeParagraphModel> AddRecipeParagraph(AddRecipeParagraphModel model)
    {
        _addIngredientModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();
        
        var recipeParagraph = _mapper.Map<RecipeParagraph>(model);
        await context.RecipeParagraphs.AddAsync(recipeParagraph);
        await context.SaveChangesAsync();

        return _mapper.Map<RecipeParagraphModel>(recipeParagraph);
    }

    /// <inheritdoc />
    public async Task UpdateRecipeParagraph(int id, UpdateRecipeParagraphModel model)
    {
        _updateIngredientModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var recipeParagraph = await context.RecipeParagraphs.FirstOrDefaultAsync(x => x.Id.Equals(id));

        // Copying data to make it so that data in "ingredient" variable wont be changed in the outer scope
        var recipeParagraphCopy = recipeParagraph;
        ProcessException.ThrowIf(() => recipeParagraphCopy is null, $"The recipe paragraph (id: {id}) was not found");

        recipeParagraph = _mapper.Map(model, recipeParagraph);

        context.RecipeParagraphs.Update(recipeParagraph!);
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