using Andrei_Mikhaleu_Task1.Models.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Andrei_Mikhaleu_Task1.Models.ViewModels;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserRepository _userRepository;

        public LoginController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = _userRepository.GetByUsername(model.UserName);

                if (user != null && user.Password == model.Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                    };
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    if (model.RememberMe)
                    {
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            userPrincipal,
                            new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTime.MaxValue
                            });
                    } else
                    {
						var authProperties = new AuthenticationProperties
						{
							IsPersistent = false,
							ExpiresUtc = null
						};
						await HttpContext.SignInAsync(
							CookieAuthenticationDefaults.AuthenticationScheme,
							userPrincipal,
							authProperties);
					}
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid credentials. Please, try again.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout() 
        { 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 
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
