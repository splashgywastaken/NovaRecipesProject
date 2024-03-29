﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Api.Controllers.Recipes.Models.RecipeCommentModels;
using NovaRecipesProject.Api.Controllers.Recipes.Models.RecipeIngredientModels;
using NovaRecipesProject.Api.Controllers.Recipes.Models.RecipeModels;
using NovaRecipesProject.Common.Enums;
using NovaRecipesProject.Common.Responses;
using NovaRecipesProject.Common.Security;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.Recipes;
using NovaRecipesProject.Services.Recipes.Models.RecipeCommentModels;
using NovaRecipesProject.Services.Recipes.Models.RecipeIngredientModels;
using NovaRecipesProject.Services.Recipes.Models.RecipeModels;

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
[ApiVersion("0.2")]
[Authorize]
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
    /// Basic get recipes. Used to get list of recipes to use them then in other methods
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of recipe responses</response>
    [ProducesResponseType(typeof(IEnumerable<RecipeResponse>), 200)]
    [HttpGet("")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesRead)]
    public async Task<IEnumerable<RecipeResponse>> GetRecipes([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var recipes = await _recipeService.GetRecipes(offset, limit);
        var response = _mapper.Map<IEnumerable<RecipeResponse>>(recipes);

        return response;
    }

    /// <summary>
    /// Get recipes. Uses filters to get list of recipes,
    /// caches data using user's Id, which is temporarly given through query
    /// </summary>
    /// <param name="nameSearchString">Search string which used to search through recipes and their names</param>
    /// <param name="searchType">Type of search
    /// (describes should it include search string, be a full search string e.t.c.). By default its partial search</param>
    /// <param name="sortType">Type of sort</param>
    /// <param name="categoriesList">List of categories to search to</param>
    /// <param name="offset">Offset for data</param>
    /// <param name="limit">Limit for data</param>
    /// <returns></returns>
    [ProducesResponseType(typeof(IEnumerable<RecipeResponse>), 200)]
    [HttpGet("filtered")]
    [MapToApiVersion("0.2")]
    [Authorize(Policy = AppScopes.RecipesRead)]
    public async Task<IEnumerable<RecipeResponse>> GetRecipesWithParameters(
        [FromQuery] string? nameSearchString,
        [FromQuery] SearchType searchType = SearchType.PartialMatch,
        [FromQuery] SortType? sortType = null,
        [FromQuery] List<string>? categoriesList = null,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10
        )
    {
        var recipes = await _recipeService.GetRecipesFiltered(
            nameSearchString,
            searchType,
            sortType,
            categoriesList,
            offset, limit
            );
        var response = _mapper.Map<IEnumerable<RecipeResponse>>(recipes);

        return response;
    }

    /// <summary>
    /// Basic get recipes. Used to get list of recipes to use them then in other methods
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of recipe responses</response>
    [ProducesResponseType(typeof(IEnumerable<RecipeResponse>), 200)]
    [HttpGet("")]
    [MapToApiVersion("0.2")]
    [Authorize(Policy = AppScopes.RecipesRead)]
    public async Task<IEnumerable<RecipeResponse>> GetRecipesAndCacheForCurrentUser(
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10
        )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var recipes = await _recipeService.GetRecipesAndCacheForUser(userId!, offset, limit);
        var response = _mapper.Map<IEnumerable<RecipeResponse>>(recipes);

        return response;
    }

    /// <summary>
    /// Basic get recipes for some user and cache data using user's data
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of recipe responses</response>
    [ProducesResponseType(typeof(IEnumerable<RecipeResponse>), 200)]
    [HttpGet("user/{userId:int}")]
    [MapToApiVersion("0.2")]
    [Authorize(Policy = AppScopes.RecipesRead)]
    public async Task<IEnumerable<RecipeResponse>> GetUserRecipes(
        [FromQuery] int offset = 0, 
        [FromQuery] int limit = 10
        )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var recipes = await _recipeService.GetUserRecipes(userId!, offset, limit);
        var response = _mapper.Map<IEnumerable<RecipeResponse>>(recipes);

        return response;
    }

    /// <summary>
    /// Gets all data about recipes ingredients
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(IEnumerable<RecipeIngredientResponse>), 200)]
    [HttpGet("{recipeId:int}/ingredients")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesRead)]
    public async Task<IEnumerable<RecipeIngredientResponse>> GetRecipesIngredients([FromRoute] int recipeId)
    {
        var ingredients = await _recipeService.GetRecipesIngredients(recipeId);
        var response = _mapper.Map<IEnumerable<RecipeIngredientResponse>>(ingredients);

        return response;
    }

    /// <summary>
    /// Gets all data about recipes ingredients and caches data for current user
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(IEnumerable<RecipeIngredientResponse>), 200)]
    [HttpGet("{recipeId:int}/ingredients")]
    [MapToApiVersion("0.2")]
    [Authorize(Policy = AppScopes.RecipesRead)]
    public async Task<IEnumerable<RecipeIngredientResponse>> GetRecipesIngredientsAndCacheForCurrentUser(
        [FromRoute] int recipeId
        )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var ingredients = await _recipeService
            .GetRecipesIngredientsAndCacheForUser(userId!, recipeId);
        var response = _mapper.Map<IEnumerable<RecipeIngredientResponse>>(ingredients);

        return response;
    }

    /// <summary>
    /// Gets all comments of certain recipe 
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(IEnumerable<RecipeCommentResponse>), 200)]
    [HttpGet("{recipeId:int}/comments")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesRead)]
    public async Task<IEnumerable<RecipeCommentResponse>> GetRecipeComments(
        [FromRoute] int recipeId, 
        [FromQuery] int offset = 0, 
        [FromQuery] int limit = 10
        )
    {
        var recipeComments = await _recipeService.GetRecipeComments(recipeId, offset, limit);
        var response = _mapper.Map<IEnumerable<RecipeCommentResponse>>(recipeComments);

        return response;
    }

    /// <summary>
    /// Gets all comments of certain recipe 
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(IEnumerable<RecipeCommentResponse>), 200)]
    [HttpGet("{recipeId:int}/comments")]
    [MapToApiVersion("0.2")]
    [Authorize(Policy = AppScopes.RecipesRead)]
    public async Task<IEnumerable<RecipeCommentResponse>> GetRecipeCommentsAndCacheForCurrentUser(
        [FromRoute] int recipeId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var recipeComments = await _recipeService
            .GetRecipeCommentsAndCacheForUser(userId!, recipeId, offset, limit);
        var response = _mapper.Map<IEnumerable<RecipeCommentResponse>>(recipeComments);

        return response;
    }

    /// <summary>
    /// Returns recipe with it's categories, ingredients and paragraphs
    /// </summary>
    /// <param name="id">Recipe id by which it returns correct data</param>
    /// <response code="200">Recipe with corresponding id</response>
    [ProducesResponseType(typeof(WholeRecipeResponse), 200)]
    [HttpGet("{id:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesRead)]
    public async Task<WholeRecipeResponse> GetRecipeById([FromRoute] int id)
    {
        var recipe = await _recipeService.GetRecipeById(id);
        var response = _mapper.Map<WholeRecipeResponse>(recipe);

        return response;
    }
    #endregion

    #region AddMethods
    /// <summary>
    /// Method used for adding new recipe to DB. Gets userId from currently signed in user
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Returns recipe model which were made while adding new data do DB</response>
    [ProducesResponseType(typeof(RecipeResponse), 200)]
    [HttpPost("user")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
    public async Task<RecipeResponse> AddRecipeWithUser([FromBody] AddRecipeRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
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
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
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

    /// <summary>
    /// Links category to recipe
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(RecipeIngredientResponse), 200)]
    [HttpPost("{recipeId:int}/categories/{categoryId:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
    public async Task<IActionResult> AddIngredientToRecipe(
        [FromRoute] int recipeId,
        [FromRoute] int categoryId
    )
    {
        await _recipeService.AddCategoryToRecipe(recipeId, categoryId);

        return Ok();
    }

    /// <summary>
    /// Adds new entry for recipe's comment
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(RecipeCommentResponse), 200)]
    [HttpPost("{recipeId:int}/comments")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
    public async Task<RecipeCommentResponse> AddCommentToRecipe(
        [FromRoute] int recipeId,
        [FromBody] AddRecipeCommentRequest request
    )
    {
        var model = _mapper.Map<AddRecipeCommentModel>(request);
        model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var recipeComment = await _recipeService.AddCommentToRecipe(recipeId, model);
        var response = _mapper.Map<RecipeCommentResponse>(recipeComment);

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
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
    public async Task<IActionResult> UpdateRecipe(
        [FromRoute] int id,
        [FromBody] UpdateRecipeRequest request
        )
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
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
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

    /// <summary>
    /// Updates certain comment from recipe
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="commentId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{recipeId:int}/comments/{commentId:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
    public async Task<IActionResult> UpdateRecipeComment(
        [FromRoute] int recipeId,
        [FromRoute] int commentId,
        [FromBody] UpdateRecipeCommentRequest request
    )
    {
        var model = _mapper.Map<UpdateRecipeCommentModel>(request);
        await _recipeService.UpdateRecipeComment(commentId, recipeId, model);

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
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
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
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
    public async Task<IActionResult> DeleteRecipeIngredient([FromRoute] int recipeId, [FromRoute] int ingredientId)
    {
        await _recipeService.DeleteRecipeIngredient(recipeId, ingredientId);

        return Ok();
    }

    /// <summary>
    /// Method used to delete connection between some recipe and category
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpDelete("{recipeId:int}/categories/{categoryId:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
    public async Task<IActionResult> DeleteRecipeCategory([FromRoute] int recipeId, [FromRoute] int categoryId)
    {
        await _recipeService.DeleteRecipeCategory(recipeId, categoryId);

        return Ok();
    }

    /// <summary>
    /// Method used to delete comment from some recipe
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpDelete("{recipeId:int}/comments/{commentId:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipesEdit)]
    public async Task<IActionResult> DeleteCommentFromIngredient(
        [FromRoute] int recipeId,
        [FromRoute] int commentId
    )
    {
        await _recipeService.DeleteRecipeComment(commentId, recipeId);

        return Ok();
    }
    #endregion
}