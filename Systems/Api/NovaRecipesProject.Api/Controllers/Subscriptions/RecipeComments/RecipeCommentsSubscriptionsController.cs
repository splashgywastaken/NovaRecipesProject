using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Api.Controllers.Subscriptions.Recipes;
using NovaRecipesProject.Common.Responses;
using NovaRecipesProject.Common.Security;
using NovaRecipesProject.Services.RecipeCommentsSubscriptions;
using NovaRecipesProject.Services.RecipesSubscriptions;
using System.Security.Claims;

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
[Authorize]
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
    /// Method used to subscribe for specific recipe comments
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    [HttpPost("{recipeId:int}/comments/subscriptions")]
    [Authorize(Policy = AppScopes.UsersSubscriptions)]
    public async Task<ActionResult> Subscribe(
        [FromRoute] int recipeId
    )
    {
        var subscriberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _recipeCommentsSubscriptionsService.Subscribe(subscriberId!, recipeId);

        return Ok();
    }

    /// <summary>
    /// Method used to unsubscribe user from recipe's comments
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    [HttpDelete("{recipeId:int}/comments/subscriptions")]
    [Authorize(Policy = AppScopes.UsersSubscriptions)]
    public async Task<ActionResult> Unsubscribe(
        [FromRoute] int recipeId
    )
    {
        var subscriberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _recipeCommentsSubscriptionsService.Unsubscribe(subscriberId!, recipeId);

        return Ok();
    }
}