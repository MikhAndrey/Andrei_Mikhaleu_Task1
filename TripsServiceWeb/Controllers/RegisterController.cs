using Microsoft.AspNetCore.Mvc;
using Andrei_Mikhaleu_Task1.Models.ViewModels;
using TripsServiceBLL.Services;
using TripsServiceBLL.Commands.Users;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.DTO.Users;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserService _userService;

        public RegisterController(UserService userService)
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
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserSignupDTO user = new()
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Email= model.Email
                };
                try
                {
                    await new RegisterUserCommand(_userService, user).ExecuteAsync();
			    }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);
                    return View(model);
                }

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
