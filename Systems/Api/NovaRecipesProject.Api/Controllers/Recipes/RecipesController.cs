using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Api.Controllers.Recipes.Models;
using NovaRecipesProject.Common.Responses;
using NovaRecipesProject.Services.Recipes;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.Recipes;

// TODO: Fix mapping errors

/// <summary>
/// Recipes controller
/// </summary>
/// <response code="400"></response>
/// <response code="401"></response>
/// <response code="403"></response>
/// <response code="404"></response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/recipes")]
[ApiController]
[ApiVersion("1.0")]
public class RecipesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<RecipesController> _logger;
    private readonly IRecipeService _recipeService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="recipeService"></param>
    public RecipesController(IMapper mapper, ILogger<RecipesController> logger, IRecipeService recipeService)
    {
        _mapper = mapper;
        _logger = logger;
        _recipeService = recipeService;
    }

    /// <summary>
    /// Basic get recipes 
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of recipe responses</response>
    [ProducesResponseType(typeof(IEnumerable<RecipeResponse>), 200)]
    [HttpGet("")]
    public async Task<IEnumerable<RecipeResponse>> GetRecipes([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var recipes = await _recipeService.GetRecipes(offset, limit);
        var response = _mapper.Map<IEnumerable<RecipeResponse>>(recipes);

        return response;
    }

    /// <summary>
    /// Gets recipe by its id
    /// </summary>
    /// <param name="id">Recipe id by which it returns correct data</param>
    /// <response code="200">Recipe with corresponding id</response>
    [ProducesResponseType(typeof(RecipeResponse), 200)]
    [HttpGet("{id:int}")]
    public async Task<RecipeResponse> GetRecipeById([FromRoute] int id)
    {
        var recipe = await _recipeService.GetRecipeById(id);
        var response = _mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    /// <summary>
    /// Method to add new data to DB
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Returns recipe model which were made while adding new data do DB</response>
    [ProducesResponseType(typeof(RecipeResponse), 200)]
    [HttpPost("")]
    public async Task<RecipeResponse> AddRecipe([FromBody] AddRecipeRequest request)
    {
        var model = _mapper.Map<AddRecipeModel>(request);
        var recipe = await _recipeService.AddRecipe(model);
        var response = _mapper.Map<RecipeResponse>(recipe);

        return response;
    }

    /// <summary>
    /// Updates recipe by its id and basic data
    /// </summary>
    /// <param name="id">Id of recipe model</param>
    /// <param name="updateRecipeRequest">Recipe model to update to</param>
    /// <response code="200"></response>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRecipe([FromRoute] int id, [FromBody] UpdateRecipeRequest updateRecipeRequest)
    {
        var model = _mapper.Map<UpdateRecipeModel>(updateRecipeRequest);
        await _recipeService.UpdateRecipe(id, model);

        return Ok();
    }

    /// <summary>
    /// Mehtod to delete data in DB
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRecipe([FromRoute] int id)
    {
        await _recipeService.DeleteRecipe(id);

        return Ok();
    }
}