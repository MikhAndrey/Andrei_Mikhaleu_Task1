using Microsoft.AspNetCore.Mvc;
using Andrei_Mikhaleu_Task1.Models.ViewModels;
using TripsServiceBLL.Services;
using TripsServiceBLL.DTO;
using TripsServiceBLL.Commands;
using TripsServiceBLL.Commands.Users;
using TripsServiceBLL.Infrastructure;

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
                    Password = model.Password
                };
                AsyncCommandInvoker invoker = new()
                {
                    Command = new RegisterUserCommand(_userService, user)
                };
                try
                {
                    await invoker.ExecuteCommandAsync();
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
