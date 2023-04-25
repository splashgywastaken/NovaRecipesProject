using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using NovaRecipesProject.Api.Controllers.Accounts.Models;

namespace NovaRecipesProject.Api.Controllers.Accounts;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.UserAccount;

/// <summary>
/// Controller for workaround with user accounts. Such as registration, login, deleting account
/// </summary>
[Route("api/v{version:apiVersion}/accounts")]
[ApiController]
[ApiVersion("0.1")]
public class AccountsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<AccountsController> _logger;
    private readonly IUserAccountService _userAccountService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="userAccountService"></param>
    public AccountsController(IMapper mapper, ILogger<AccountsController> logger, IUserAccountService userAccountService)
    {
        _mapper = mapper;
        _logger = logger;
        _userAccountService = userAccountService;
    }

    /// <summary>
    /// Method for registration, uses model in args to register new user in database
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<UserAccountResponse> Register([FromQuery] RegisterUserAccountRequest request)
    {
        var user = await _userAccountService.Create(_mapper.Map<RegisterUserAccountModel>(request));

        var response = _mapper.Map<UserAccountResponse>(user);

        return response;
    }

    /// <summary>
    /// Method to use for email confirmation for user
    /// User should be logged in current session for method to add new request
    /// Also user should not have his email already sent for confirmation
    /// </summary>
    /// <returns></returns>
    [HttpPost("request-confirm-email")]
    public async Task<IActionResult> RequestEmailConfirmation([FromQuery] string email)
    {
        return await _userAccountService.RequestEmailConfirmation(email);
    }

    /// <summary>
    /// Method used to confirm email for some user using Id for user's email request confirmation entry in DB
    /// </summary>
    /// <param name="confirmationId"></param>
    /// <returns></returns>
    [HttpPut("confirm-email/{confirmationId:int}")]
    public async Task<IActionResult> ConfirmEmail([FromRoute] int confirmationId)
    {
        return await _userAccountService.ConfirmEmail(confirmationId);
    }
}