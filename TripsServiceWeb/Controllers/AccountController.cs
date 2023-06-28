using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Infrastructure.Exceptions;
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
					await _userService.TryToRegisterNewUserAsync(user);
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
					string jwtToken = await _userService.GetJWTTokenAsync(user);
					DateTime? cookieExpiresUTC = user.RememberMe ? DateTime.UtcNow.AddDays(UtilConstants.AuthorizationExpirationInDays) : null;
					HttpContext.Response.Cookies.Append(UtilConstants.JwtTokenCookiesAlias, jwtToken, new CookieOptions
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
			HttpContext.Response.Cookies.Delete(UtilConstants.JwtTokenCookiesAlias);
			return RedirectToAction("Index", "Home");
		}

		private IActionResult RedirectToLocal(string returnUrl)
		{
			return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
		}
	}
}
