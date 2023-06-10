using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
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
				int? idOfUserFromDb = await _userService.GetUserIdForLoginAsync(user);
				if (idOfUserFromDb != null)
				{
					DateTime jwtExpiresUTC = user.RememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(1);
					DateTime? cookieExpiresUTC = user.RememberMe ? DateTime.UtcNow.AddDays(7) : null;
					JwtSecurityTokenHandler tokenHandler = new();
					byte[] key = Encoding.ASCII.GetBytes(ProgramHelper.Configuration["Jwt:Key"]);
					SecurityTokenDescriptor tokenDescriptor = new()
					{
						Subject = new ClaimsIdentity(new Claim[]
						{
							new Claim(ClaimTypes.Name, user.UserName),
							new Claim ("userId", idOfUserFromDb.ToString())
						}),
						Audience = ProgramHelper.Configuration["Jwt:Issuer"],
						Issuer = ProgramHelper.Configuration["Jwt:Issuer"],
						Expires = jwtExpiresUTC,
						SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
					};
					SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

					string? jwtToken = tokenHandler.WriteToken(token);

					HttpContext.Response.Cookies.Append("jwt", jwtToken, new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						Expires = cookieExpiresUTC,
						SameSite = SameSiteMode.Strict
					});

					return RedirectToLocal(returnUrl);
				}

				ModelState.AddModelError(string.Empty, "Invalid credentials. Please, try again.");
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
