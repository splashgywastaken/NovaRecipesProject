using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Api.Controllers.Categories.Models;
using NovaRecipesProject.Api.Controllers.Recipes;
using NovaRecipesProject.Common.Responses;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Categories;
using NovaRecipesProject.Services.Categories.Models;
using NovaRecipesProject.Services.Recipes;

namespace NovaRecipesProject.Api.Controllers.Categories;

/// <summary>
/// Category controller
/// </summary>
/// <response code="400"></response>
/// <response code="401"></response>
/// <response code="403"></response>
/// <response code="404"></response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/categories")]
[ApiController]
[ApiVersion("1.0")]
public class CategoriesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<CategoriesController> _logger;
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="recipeService"></param>
    public CategoriesController(
        IMapper mapper, 
        ILogger<CategoriesController> logger,
        ICategoryService recipeService)
    {
        _mapper = mapper;
        _logger = logger;
        _categoryService = recipeService;
    }

    /// <summary>
    /// Basic get categories
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of category responses</response>
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), 200)]
    [HttpGet("")]
    public async Task<IEnumerable<CategoryResponse>> GetCategories([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var categories = await _categoryService.GetCategories(offset, limit);
        var response = _mapper.Map<IEnumerable<CategoryResponse>>(categories);

        return response;
    }

    /// <summary>
    /// Gets category by its id
    /// </summary>
    /// <param name="id">Category id by which it returns correct data</param>
    /// <response code="200">Category with corresponding id</response>
    [ProducesResponseType(typeof(CategoryResponse), 200)]
    [HttpGet("{id:int}")]
    public async Task<CategoryResponse> GetCategoryById([FromRoute] int id)
    {
        var category = await _categoryService.GetCategoryById(id);
        var response = _mapper.Map<CategoryResponse>(category);

        return response;
    }

    /// <summary>
    /// Method to add new data to DB
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Returns category model which were made while adding new data do DB</response>
    [ProducesResponseType(typeof(CategoryResponse), 200)]
    [HttpPost("")]
    public async Task<CategoryResponse> AddCategory([FromBody] AddCategoryRequest request)
    {
        var model = _mapper.Map<AddCategoryModel>(request);
        var category = await _categoryService.AddCategory(model);
        var response = _mapper.Map<CategoryResponse>(category);

        return response;
    }

    /// <summary>
    /// Updates category by its id and basic data
    /// </summary>
    /// <param name="id">Id of category model</param>
    /// <param name="request">Category model to update to</param>
    /// <response code="200"></response>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryRequest request)
    {
        var model = _mapper.Map<UpdateCategoryModel>(request);
        await _categoryService.UpdateCategory(id, model);

        return Ok();
    }

    /// <summary>
    /// Method to delete data in DB
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        await _categoryService.DeleteCategory(id);

        return Ok();
    }
}