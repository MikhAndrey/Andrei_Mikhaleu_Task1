﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Users;
using TripsServiceBLL.DTO.Chats;
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
    private readonly INotificationsService _notificationsService;

    private readonly LoginUserCommand _loginUserCommand;

    public AccountController(
        IUserService userService, 
        INotificationsService notificationsService, 
        IMapper mapper, 
        LoginUserCommand loginUserCommand)
    {
        _userService = userService;
        _notificationsService = notificationsService;
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
    
    [HttpGet("userinfo")]
    public ActionResult<string?> GetUserInfo()
    {
        try
        {
            UserListDTO userInfo = _userService.GetCurrentUserMainInfo();
            return Ok(userInfo);
        }
        catch (ArgumentNullException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("isAuthenticated")]
    public ActionResult IsAuthenticated()
    {
        return Ok(HttpContext?.User?.Identity?.IsAuthenticated);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete(UtilConstants.JwtTokenCookiesAlias);
        return Ok();
    }

    [HttpGet("notifications/{userId}")]
    public IActionResult GetNotifications(string userId)
    {
        try
        {
            List<ChatNotificationMessageDTO> notifications = _notificationsService.GetCurrentNotifications(userId);
            return Ok(notifications);
        }
        catch (ArgumentNullException)
        {
            return Unauthorized();
        }
    }
}
