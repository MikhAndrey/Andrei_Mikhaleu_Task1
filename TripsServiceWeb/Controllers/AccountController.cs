using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Users;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace Andrei_Mikhaleu_Task1.Controllers;

public class AccountController : Controller
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

	[HttpGet]
	public IActionResult Register()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
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
				return View(user);
			}
		}

		return View(user);
	}

	[HttpGet]
	public IActionResult Login(string? returnUrl = null)
	{
		ViewData["ReturnUrl"] = returnUrl;
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(UserLoginDTO user, string? returnUrl = null)
	{
		ViewData["ReturnUrl"] = returnUrl;

		if (ModelState.IsValid)
		{
			try
			{
				await _loginUserCommand.ExecuteAsync(user);
				return RedirectToLocal(returnUrl);
			}
			catch (ValidationException ex)
			{
				ModelState.AddModelError(ex.Property, ex.Message);
			}
		}

		return View(user);
	}

	[HttpGet]
	public IActionResult Logout()
	{
		HttpContext.Response.Cookies.Delete(UtilConstants.JwtTokenCookiesAlias);
		return RedirectToAction("Index", "Home");
	}

	private IActionResult RedirectToLocal(string returnUrl)
	{
		return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
	}
}