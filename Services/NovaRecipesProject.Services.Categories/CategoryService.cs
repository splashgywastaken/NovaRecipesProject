﻿using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NovaRecipesProject.Common.Exceptions;
using NovaRecipesProject.Common.Validator;
using NovaRecipesProject.Context;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Categories.Models;

namespace NovaRecipesProject.Services.Categories;

/// <inheritdoc />
public class CategoryService : ICategoryService
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly IModelValidator<AddCategoryModel> _addCategoryModelValidator;
    private readonly IModelValidator<UpdateCategoryModel> _updateCategoryModelValidator;

    /// <summary>
    /// Constructor for this class
    /// </summary>
    /// <param name="dbContextFactory"></param>
    /// <param name="mapper"></param>
    /// <param name="addCategoryModelValidator"></param>
    /// <param name="updateCategoryModelValidator"></param>
    public CategoryService(
        IDbContextFactory<MainDbContext> dbContextFactory, 
        IMapper mapper, 
        IModelValidator<AddCategoryModel> addCategoryModelValidator, 
        IModelValidator<UpdateCategoryModel> updateCategoryModelValidator
        )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _addCategoryModelValidator = addCategoryModelValidator;
        _updateCategoryModelValidator = updateCategoryModelValidator;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CategoryModel>> GetCategories(int offset = 0, int limit = 10)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var categories = context
            .Categories
            .AsQueryable();

        categories = categories
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));

        var data = 
            (await categories.ToListAsync())
            .Select(category => _mapper.Map<CategoryModel>(category));

        return data;
    }

    /// <inheritdoc />
    public async Task<CategoryModel> GetCategoryById(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var category = await context
            .Categories
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        var data = _mapper.Map<CategoryModel>(category);

        return data;
    }

    /// <inheritdoc />
    public async Task<CategoryModel> AddCategory(AddCategoryModel model)
    {
        _addCategoryModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var category = _mapper.Map<Category>(model);
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        return _mapper.Map<CategoryModel>(category);
    }

    /// <inheritdoc />
    public async Task UpdateCategory(int id, UpdateCategoryModel model)
    {
        _updateCategoryModelValidator.Check(model);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id));

        // Copying data to make it so that data in "category" variable wont be changed in the outer scope
        var categoryCopy = category;
        ProcessException.ThrowIf(() => categoryCopy is null, $"The category (id: {id}) was not found");

        category = _mapper.Map(model, category);

        context.Categories.Update(category!);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteCategory(int id)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id))
                       ?? throw new ProcessException(new Exception($"Recipe (id: {id}) was not found"));

        context.Remove(category);
        await context.SaveChangesAsync();
    }
}