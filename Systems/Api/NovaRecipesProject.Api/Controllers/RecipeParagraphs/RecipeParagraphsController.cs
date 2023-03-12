using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.Api.Controllers.RecipeParagraphs.Models;
using NovaRecipesProject.Common.Responses;
using NovaRecipesProject.Services.RecipeParagraphs.Models;
using NovaRecipesProject.Services.RecipeParagraphs;

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
[ApiVersion("1.0")]
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
    /// Basic get RecipeParagraph
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of RecipeParagraph responses</response>
    [ProducesResponseType(typeof(IEnumerable<RecipeParagraphResponse>), 200)]
    [HttpGet("")]
    public async Task<IEnumerable<RecipeParagraphResponse>> GetRecipeParagraphs([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var categories = await _recipeParagraphService.GetRecipeParagraphs(offset, limit);
        var response = _mapper.Map<IEnumerable<RecipeParagraphResponse>>(categories);

        return response;
    }

    /// <summary>
    /// Gets RecipeParagraph by its id
    /// </summary>
    /// <param name="id">RecipeParagraph id by which it returns correct data</param>
    /// <response code="200">RecipeParagraph with corresponding id</response>
    [ProducesResponseType(typeof(RecipeParagraphResponse), 200)]
    [HttpGet("{id:int}")]
    public async Task<RecipeParagraphResponse> GetRecipeParagraphById([FromRoute] int id)
    {
        var category = await _recipeParagraphService.GetRecipeParagraphById(id);
        var response = _mapper.Map<RecipeParagraphResponse>(category);

        return response;
    }

    /// <summary>
    /// Method to add new data to DB
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Returns RecipeParagraph model which were made while adding new data do DB</response>
    [ProducesResponseType(typeof(RecipeParagraphResponse), 200)]
    [HttpPost("")]
    public async Task<RecipeParagraphResponse> AddRecipeParagraph([FromBody] AddRecipeParagraphRequest request)
    {
        var model = _mapper.Map<AddRecipeParagraphModel>(request);
        var category = await _recipeParagraphService.AddRecipeParagraph(model);
        var response = _mapper.Map<RecipeParagraphResponse>(category);

        return response;
    }

    /// <summary>
    /// Updates RecipeParagraph by its id and basic data
    /// </summary>
    /// <param name="id">Id of RecipeParagraph model</param>
    /// <param name="request">RecipeParagraph model to update to</param>
    /// <response code="200"></response>
    [HttpPut("{id:int}")]
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
    public async Task<IActionResult> DeleteRecipeParagraph([FromRoute] int id)
    {
        await _recipeParagraphService.DeleteRecipeParagraph(id);

        return Ok();
    }
}