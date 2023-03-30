using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Api.Controllers.Recipes.Models;
using NovaRecipesProject.Common.Responses;
using NovaRecipesProject.Services.Recipes;
using NovaRecipesProject.Services.Recipes.Models;

namespace NovaRecipesProject.Api.Controllers.Recipes;

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
[ApiVersion("0.1")]
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
    public RecipesController(IRecipeService recipeService, IMapper mapper, ILogger<RecipesController> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _recipeService = recipeService;
    }

    #region GetMethods
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
    /// Basic get recipes for some user
    /// </summary>
    /// <param name="userId">User's id to get data for</param>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of recipe responses</response>
    [ProducesResponseType(typeof(IEnumerable<RecipeResponse>), 200)]
    [HttpGet("user/{userId:int}")]
    public async Task<IEnumerable<RecipeResponse>> GetUserRecipes([FromRoute] int userId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var recipes = await _recipeService.GetUserRecipes(userId, offset, limit);
        var response = _mapper.Map<IEnumerable<RecipeResponse>>(recipes);

        return response;
    }

    /// <summary>
    /// Gets all data about recipes ingredients
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(RecipeIngredientResponse), 200)]
    [HttpGet("{recipeId:int}/ingredients")]
    public async Task<IEnumerable<RecipeIngredientResponse>> GetRecipesIngredients([FromRoute] int recipeId)
    {
        var ingredients = await _recipeService.GetRecipesIngredients(recipeId);
        var response = _mapper.Map<IEnumerable<RecipeIngredientResponse>>(ingredients);

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
    #endregion

    #region AddMethods
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
    /// Method to add new data to DB
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns recipe model which were made while adding new data do DB</response>
    [ProducesResponseType(typeof(RecipeResponse), 200)]
    [HttpPost("user/{userId:int}")]
    public async Task<RecipeResponse> AddRecipeWithUser([FromRoute] int userId, [FromBody] AddRecipeRequest request)
    {
        var model = _mapper.Map<AddRecipeModel>(request);
        var recipe = await _recipeService.AddRecipeWithUser(userId, model);
        var response = _mapper.Map<RecipeResponse>(recipe);

        return response;
    }
    
    /// <summary>
    /// Links ingredient to recipe from already existing one's
    /// </summary>
    /// <param name="recipeId">Recipe to ingredient add to</param>
    /// <param name="request">Data about ingredient in recipe</param>
    /// <returns>Recipe's ingredient that it added to recipe with weight, portion and nutrition data</returns>
    [ProducesResponseType(typeof(RecipeIngredientResponse), 200)]
    [HttpPost("{recipeId:int}/ingredients")]
    public async Task<RecipeIngredientResponse> AddIngredientToRecipe(
        [FromRoute] int recipeId,
        [FromBody] AddRecipeIngredientRequest request
        )
    {
        var model = _mapper.Map<AddRecipeIngredientModel>(request);
        model.RecipeId = recipeId;
        var recipeIngredient = await _recipeService.AddIngredientToRecipe(model);
        var response = _mapper.Map<RecipeIngredientResponse>(recipeIngredient);

        return response;
    }
    #endregion

    #region UpdateMethods
    /// <summary>
    /// Updates recipe by its id and basic data
    /// </summary>
    /// <param name="id">Id of recipe model</param>
    /// <param name="request">Recipe model to update to</param>
    /// <response code="200"></response>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRecipe([FromRoute] int id, [FromBody] UpdateRecipeRequest request)
    {
        var model = _mapper.Map<UpdateRecipeModel>(request);
        await _recipeService.UpdateRecipe(id, model);

        return Ok();
    }

    /// <summary>
    /// Updates only RecipeIngredient entry in DB.
    /// Can change weight and portion
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="ingredientId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{recipeId:int}/ingredients/{ingredientId:int}")]
    public async Task<IActionResult> UpdateRecipeIngredient(
        [FromRoute] int recipeId,
        [FromRoute] int ingredientId,
        [FromBody] UpdateRecipeIngredientRequest request
    )
    {
        var model = _mapper.Map<UpdateRecipeIngredientModel>(request);
        await _recipeService.UpdateRecipeIngredient(recipeId, ingredientId, model);

        return Ok();
    }
    #endregion

    #region DeleteMethods
    /// <summary>
    /// Method to delete data in DB
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRecipe([FromRoute] int id)
    {
        await _recipeService.DeleteRecipe(id);

        return Ok();
    }

    /// <summary>
    /// Method used to delete connection between some recipe and ingredient
    /// </summary>
    /// <param name="recipeId">Recipe id to search through recipes with</param>
    /// <param name="ingredientId">Ingredient id to search through recipes with</param>
    /// <returns></returns>
    [HttpDelete("{recipeId:int}/ingredients/{ingredientId:int}")]
    public async Task<IActionResult> DeleteRecipeIngredient([FromRoute] int recipeId, [FromRoute] int ingredientId)
    {
        await _recipeService.DeleteRecipeIngredient(recipeId, ingredientId);

        return Ok();
    }
    #endregion
}