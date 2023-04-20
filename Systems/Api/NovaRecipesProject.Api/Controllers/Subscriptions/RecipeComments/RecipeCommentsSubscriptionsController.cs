using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Api.Controllers.Subscriptions.Recipes;
using NovaRecipesProject.Common.Responses;
using NovaRecipesProject.Services.RecipeCommentsSubscriptions;
using NovaRecipesProject.Services.RecipesSubscriptions;

namespace NovaRecipesProject.Api.Controllers.Subscriptions.RecipeComments;

/// <summary>
/// Recipe comments subscription controller, used for subscribing 
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
public class RecipeCommentsSubscriptionsController : ControllerBase
{
    private readonly ILogger<RecipesSubscriptionController> _logger;
    private readonly IRecipeCommentsSubscriptionsService _recipeCommentsSubscriptionsService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="recipeCommentsSubscriptionsService"></param>
    public RecipeCommentsSubscriptionsController(
        ILogger<RecipesSubscriptionController> logger,
        IRecipeCommentsSubscriptionsService recipeCommentsSubscriptionsService
        )
    {
        _logger = logger;
        _recipeCommentsSubscriptionsService = recipeCommentsSubscriptionsService;
    }

    /// <summary>
    /// Method used to subscribe for specific author
    /// </summary>
    /// <param name="subscriberId"></param>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    [HttpPost("{recipeId:int}/comments/subscriptions")]
    public async Task<ActionResult> Subscribe(
        [FromQuery] int subscriberId,
        [FromRoute] int recipeId
    )
    {
        await _recipeCommentsSubscriptionsService.Subscribe(subscriberId, recipeId);

        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="subscriberId"></param>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    [HttpDelete("{recipeId:int}/comments/subscriptions")]
    public async Task<ActionResult> Unsubscribe(
        [FromQuery] int subscriberId,
        [FromRoute] int recipeId
    )
    {
        await _recipeCommentsSubscriptionsService.Unsubscribe(subscriberId, recipeId);

        return Ok();
    }
}