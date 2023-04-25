using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Api.Controllers.RecipeParagraphs.Models;
using NovaRecipesProject.Common.Responses;
using NovaRecipesProject.Services.RecipeParagraphs.Models;
using NovaRecipesProject.Services.RecipeParagraphs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using NovaRecipesProject.Common.Security;

namespace NovaRecipesProject.Api.Controllers.RecipeParagraphs;

/// <summary>
/// Recipe paragraph controller
/// </summary>
/// <response code="400"></response>
/// <response code="401"></response>
/// <response code="403"></response>
/// <response code="404"></response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/recipeParagraphs")]
[ApiController]
[ApiVersion("0.1")]
[ApiVersion("0.2")]
[Authorize]
public class RecipeParagraphsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<RecipeParagraphsController> _logger;
    private readonly IRecipeParagraphService _recipeParagraphService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="recipeService"></param>
    public RecipeParagraphsController(
        IMapper mapper,
        ILogger<RecipeParagraphsController> logger,
        IRecipeParagraphService recipeService)
    {
        _mapper = mapper;
        _logger = logger;
        _recipeParagraphService = recipeService;
    }

    /// <summary>
    /// Gets all recipe paragraphs of a certain recipe using its Id
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns>List of RecipeParagraphResponse ordered by order number value</returns>
    [ProducesResponseType(typeof(IEnumerable<RecipeParagraphResponse>), 200)]
    [HttpGet("recipe/{recipeId:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipeParagraphsRead)]
    public async Task<IEnumerable<RecipeParagraphResponse>> GetRecipesParagraphs(
        [FromRoute] int recipeId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10
    )
    {
        var recipeParagraphsByRecipesId = 
            await _recipeParagraphService.GetRecipeParagraphsByRecipesId(recipeId, offset, limit);
        var response = _mapper.Map<IEnumerable<RecipeParagraphResponse>>(recipeParagraphsByRecipesId);

        return response;
    }

    /// <summary>
    /// Gets all recipe paragraphs of a certain recipe using its Id
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns>List of RecipeParagraphResponse ordered by order number value</returns>
    [ProducesResponseType(typeof(IEnumerable<RecipeParagraphResponse>), 200)]
    [HttpGet("recipe/{recipeId:int}")]
    [MapToApiVersion("0.2")]
    [Authorize(Policy = AppScopes.RecipeParagraphsRead)]
    public async Task<IEnumerable<RecipeParagraphResponse>> GetRecipesParagraphsAndCacheWithUserId(
        [FromRoute] int recipeId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var recipeParagraphsByRecipesId =
            await _recipeParagraphService.GetRecipeParagraphsByRecipesIdAndCacheForUser(userId!, recipeId, offset, limit);
        var response = _mapper.Map<IEnumerable<RecipeParagraphResponse>>(recipeParagraphsByRecipesId);

        return response;
    }



    /// <summary>
    /// Gets RecipeParagraph by its id
    /// </summary>
    /// <param name="id">RecipeParagraph id by which it returns correct data</param>
    /// <response code="200">RecipeParagraph with corresponding id</response>
    [ProducesResponseType(typeof(RecipeParagraphResponse), 200)]
    [HttpGet("{id:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipeParagraphsRead)]
    public async Task<RecipeParagraphResponse> GetRecipeParagraphById([FromRoute] int id)
    {
        var recipeParagraphById = await _recipeParagraphService.GetRecipeParagraphById(id);
        var response = _mapper.Map<RecipeParagraphResponse>(recipeParagraphById);

        return response;
    }

    /// <summary>
    /// Method to add new data to DB
    /// </summary>
    /// <param name="recipeId">Recipe's id to link to</param>
    /// <param name="request">Request model to use</param>
    /// <response code="200">Returns RecipeParagraph model which were made while adding new data do DB</response>
    [ProducesResponseType(typeof(RecipeParagraphResponse), 200)]
    [HttpPost("recipe/{recipeId:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipeParagraphsEdit)]
    public async Task<RecipeParagraphResponse> AddRecipeParagraph(
        [FromRoute] int recipeId,
        [FromBody] AddRecipeParagraphRequest request
        )
    {
        var model = _mapper.Map<AddRecipeParagraphModel>(request);
        var addRecipeParagraph = await _recipeParagraphService.AddRecipeParagraph(recipeId, model);
        var response = _mapper.Map<RecipeParagraphResponse>(addRecipeParagraph);

        return response;
    }

    /// <summary>
    /// Updates recipe paragraph order number
    /// </summary>
    /// <param name="orderNumber">Value to change entry's OrderNumber value to</param>
    /// <param name="id">RecipeParagraph model id to update</param>
    /// <response code="200"></response>
    [HttpPut("{id:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipeParagraphsEdit)]
    public async Task<IActionResult> ChangeRecipeParagraphOrderNumber([FromQuery] int orderNumber, [FromRoute] int id)
    {
        await _recipeParagraphService.ChangeRecipeParagraphOrderNumber(orderNumber, id);
        return Ok();
    }

    /// <summary>
    /// Updates RecipeParagraph by its id and basic data
    /// </summary>
    /// <param name="id">Id of RecipeParagraph model</param>
    /// <param name="request">RecipeParagraph model to update to</param>
    /// <response code="200"></response>
    [HttpPut("{id:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipeParagraphsEdit)]
    public async Task<IActionResult> UpdateRecipeParagraph([FromRoute] int id, [FromBody] UpdateRecipeParagraphRequest request)
    {
        var model = _mapper.Map<UpdateRecipeParagraphModel>(request);
        await _recipeParagraphService.UpdateRecipeParagraph(id, model);

        return Ok();
    }

    /// <summary>
    /// Mehtod to delete data in DB
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [MapToApiVersion("0.1")]
    [Authorize(Policy = AppScopes.RecipeParagraphsEdit)]
    public async Task<IActionResult> DeleteRecipeParagraph([FromRoute] int id)
    {
        await _recipeParagraphService.DeleteRecipeParagraph(id);

        return Ok();
    }
}