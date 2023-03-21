namespace NovaRecipesProject.Api.Controllers.Accounts;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NovaRecipesProject.API.Controllers.Models;
using Services.UserAccount;

/// <summary>
/// Controller for workaround with user accounts. Such as registration, login, deleting account
/// </summary>
[Route("api/v{version:apiVersion}/accounts")]
[ApiController]
[ApiVersion("0.1")]
public class AccountsController
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
}