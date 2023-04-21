using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Api.Controllers.Ingredients.Models;
using NovaRecipesProject.Common.Responses;
using NovaRecipesProject.Services.Ingredients;
using NovaRecipesProject.Services.Ingredients.Models;

namespace NovaRecipesProject.Api.Controllers.Ingredients;

// TODO: Finish work with this controller

/// <summary>
/// Ingredient controller
/// </summary>
/// <response code="400"></response>
/// <response code="401"></response>
/// <response code="403"></response>
/// <response code="404"></response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/ingredients")]
[ApiController]
[ApiVersion("0.1")]
[ApiVersion("0.2")]
public class IngredientsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<IngredientsController> _logger;
    private readonly IIngredientService _ingredientService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="recipeService"></param>
    public IngredientsController(
        IMapper mapper,
        ILogger<IngredientsController> logger,
        IIngredientService recipeService)
    {
        _mapper = mapper;
        _logger = logger;
        _ingredientService = recipeService;
    }

    /// <summary>
    /// Basic get ingredients
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of ingredients responses</response>
    [ProducesResponseType(typeof(IEnumerable<IngredientResponse>), 200)]
    [HttpGet("")]
    [MapToApiVersion("0.1")]
    public async Task<IEnumerable<IngredientResponse>> GetIngredients(
        [FromQuery] int offset = 0, 
        [FromQuery] int limit = 10
        )
    {
        var categories = await _ingredientService.GetIngredients(offset, limit);
        var response = _mapper.Map<IEnumerable<IngredientResponse>>(categories);

        return response;
    }

    /// <summary>
    /// Basic get ingredients
    /// </summary>
    /// <param name="userId">Lazy implementation for caching for current user</param>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of ingredients responses</response>
    [ProducesResponseType(typeof(IEnumerable<IngredientResponse>), 200)]
    [HttpGet("")]
    [MapToApiVersion("0.2")]
    public async Task<IEnumerable<IngredientResponse>> GetIngredientsAndCacheForCurrentUser(
        [FromQuery] int userId,
        [FromQuery] int offset = 0, 
        [FromQuery] int limit = 10
        )
    {
        var categories = await _ingredientService
            .GetIngredientsAndCacheForUser(userId, offset, limit);
        var response = _mapper.Map<IEnumerable<IngredientResponse>>(categories);

        return response;
    }

    /// <summary>
    /// Gets ingredient by its id
    /// </summary>
    /// <param name="id">Ingredient id by which it returns correct data</param>
    /// <response code="200">Ingredient with corresponding id</response>
    [ProducesResponseType(typeof(IngredientResponse), 200)]
    [HttpGet("{id:int}")]
    [MapToApiVersion("0.1")]
    public async Task<IngredientResponse> GetIngredientById([FromRoute] int id)
    {
        var category = await _ingredientService.GetIngredientById(id);
        var response = _mapper.Map<IngredientResponse>(category);

        return response;
    }

    /// <summary>
    /// Method to add new data to DB
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Returns ingredient model which were made while adding new data do DB</response>
    [ProducesResponseType(typeof(IngredientResponse), 200)]
    [HttpPost("")]
    [MapToApiVersion("0.1")]
    public async Task<IngredientResponse> AddIngredient([FromBody] AddIngredientRequest request)
    {
        var model = _mapper.Map<AddIngredientModel>(request);
        var category = await _ingredientService.AddIngredient(model);
        var response = _mapper.Map<IngredientResponse>(category);

        return response;
    }

    /// <summary>
    /// Updates ingredient by its id and basic data
    /// </summary>
    /// <param name="id">Id of ingredient model</param>
    /// <param name="request">Ingredient model to update to</param>
    /// <response code="200"></response>
    [HttpPut("{id:int}")]
    [MapToApiVersion("0.1")]
    public async Task<IActionResult> UpdateIngredient([FromRoute] int id, [FromBody] UpdateIngredientRequest request)
    {
        var model = _mapper.Map<UpdateIngredientModel>(request);
        await _ingredientService.UpdateIngredient(id, model);

        return Ok();
    }

    /// <summary>
    /// Method to delete data in DB
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [MapToApiVersion("0.1")]
    public async Task<IActionResult> DeleteIngredient([FromRoute] int id)
    {
        await _ingredientService.DeleteIngredient(id);

        return Ok();
    }
}