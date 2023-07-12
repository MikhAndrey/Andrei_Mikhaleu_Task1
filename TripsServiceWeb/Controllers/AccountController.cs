using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Users;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly IUserService _userService;

    private readonly LoginUserCommand _loginUserCommand;

    public AccountController(IUserService userService, IMapper mapper, LoginUserCommand loginUserCommand)
    {
        _userService = userService;
        _mapper = mapper;
        _loginUserCommand = loginUserCommand;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserSignupDTO user)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _userService.TryToRegisterNewUserAsync(user);
                UserLoginDTO loginUser = _mapper.Map<UserLoginDTO>(user);
                return await Login(loginUser);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return BadRequest(ModelState);
            }
        }

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDTO user)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _loginUserCommand.ExecuteAsync(user);
                return Ok();
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
            }
        }

        return BadRequest(ModelState);
    }
    
    [HttpGet("username")]
    public ActionResult<string?> GetUserName()
    {
        return Ok(new { UserName = HttpContext?.User?.Identity?.Name});
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete(UtilConstants.JwtTokenCookiesAlias);
        return Ok();
    }
}
