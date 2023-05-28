using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Andrei_Mikhaleu_Task1.Models.Repos;
using Andrei_Mikhaleu_Task1.Models.Entities.Common;
using Andrei_Mikhaleu_Task1.Models.ViewModels;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserRepository _userRepository;

        public RegisterController(UserRepository userRepository)
        {
            _userRepository = userRepository;
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
				User? existingUser = _userRepository.GetByUsername(model.UserName);
				if (existingUser != null)
				{
					ModelState.AddModelError(string.Empty, "This username is already taken");
					return View(model);
				}
				User user = new()
                {
                    UserName = model.UserName,
                    Password= model.Password
                };

                _userRepository.Add(user);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
