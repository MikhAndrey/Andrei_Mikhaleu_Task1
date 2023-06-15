﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Users;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace Andrei_Mikhaleu_Task1.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUserService _userService;

		private readonly IMapper _mapper;

		public AccountController(IUserService userService, IMapper mapper)
		{
			_userService = userService;
			_mapper = mapper;
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
					await new RegisterUserCommand(_userService, user).ExecuteAsync();
				}
				catch (ValidationException ex)
				{
					ModelState.AddModelError(ex.Property, ex.Message);
					return View(user);
				}

				return await Login(_mapper.Map<UserLoginDTO>(user), null);
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
					string jwtToken = await new GetLoginJWTTokenCommand(_userService, user).ExecuteAsync();
					DateTime? cookieExpiresUTC = user.RememberMe ? DateTime.UtcNow.AddDays(Constants.AuthorizationExpirationInDays) : null;
					HttpContext.Response.Cookies.Append(Constants.JwtTokenCookiesAlias, jwtToken, new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						Expires = cookieExpiresUTC,
						SameSite = SameSiteMode.Strict
					});

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
			HttpContext.Response.Cookies.Delete(Constants.JwtTokenCookiesAlias);
			return RedirectToAction("Index", "Home");
		}

		private IActionResult RedirectToLocal(string returnUrl)
		{
			return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
		}
	}
}
