using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Commands.Users;

namespace Andrei_Mikhaleu_Task1.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUserService _userService;

		public AccountController(IUserService userService)
		{
			_userService = userService;
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

				return await Login(new(user), null);
			}
			return View(user);
		}

		[HttpGet]
		public IActionResult Login(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(UserLoginDTO user, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;

			if (ModelState.IsValid)
			{
				try
				{
					string jwtToken = await new GetLoginJWTTokenCommand(_userService, user).ExecuteAsync();
					DateTime? cookieExpiresUTC = user.RememberMe ? DateTime.UtcNow.AddDays(7) : null;
					HttpContext.Response.Cookies.Append("jwt", jwtToken, new CookieOptions
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
		public async Task<IActionResult> Logout()
		{
			HttpContext.Response.Cookies.Delete("jwt");
			return RedirectToAction("Index", "Home");
		}

		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}
	}
}
