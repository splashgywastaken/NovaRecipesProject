using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Api.Controllers.Recipes;
using NovaRecipesProject.Common.Responses;
using NovaRecipesProject.Services.Recipes;
using NovaRecipesProject.Services.RecipesSubscriptions;

namespace NovaRecipesProject.Api.Controllers.Subscriptions.Recipes;

/// <summary>
/// Recipes controller
/// </summary>
/// <response code="400"></response>
/// <response code="401"></response>
/// <response code="403"></response>
/// <response code="404"></response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/recipes/subscriptions")]
[ApiController]
[ApiVersion("0.1")]
public class RecipesSubscriptionController : ControllerBase
{
    private readonly ILogger<RecipesSubscriptionController> _logger;
    private readonly IRecipeSubscriptionsService _recipeSubscriptionsService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="recipeSubscriptionsService"></param>
    public RecipesSubscriptionController(
        ILogger<RecipesSubscriptionController> logger, 
        IRecipeSubscriptionsService recipeSubscriptionsService
        )
    {
        _logger = logger;
        _recipeSubscriptionsService = recipeSubscriptionsService;
    }

    /// <summary>
    /// Method used to subscribe for specific author
    /// </summary>
    /// <param name="subscriberId"></param>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<ActionResult> Subscribe(
        [FromQuery] int subscriberId,
        [FromQuery] int recipeId
        )
    {
        await _recipeSubscriptionsService.Subscribe(subscriberId, recipeId);

        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="subscriberId"></param>
    /// <param name="authorId"></param>
    /// <returns></returns>
    [HttpDelete("")]
    public async Task<ActionResult> Unsubscribe(
        [FromQuery] int subscriberId,
        [FromQuery] int authorId
        )
    {
        await _recipeSubscriptionsService.Unsubscribe(subscriberId, authorId);

        return Ok();
    }
}