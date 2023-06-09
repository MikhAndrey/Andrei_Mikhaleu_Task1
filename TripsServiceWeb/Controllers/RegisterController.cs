using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Users;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Controllers
{
	public class RegisterController : Controller
	{
		private readonly IUserService _userService;

		public RegisterController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(UserSignupDTO user)
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

				return RedirectToAction("Index", "Home");
			}
			return View(user);
		}
	}
}
