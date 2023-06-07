using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Andrei_Mikhaleu_Task1.Models.ViewModels;
using TripsServiceBLL.Services;
using TripsServiceBLL.DTO;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserService _userService;

        public LoginController(UserService userService)
        {
            _userService = userService;
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
                UserLoginDTO user = new()
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    RememberMe = model.RememberMe
                };

                if (await _userService.UserExists(user))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                    };
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    if (user.RememberMe)
                    {
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            userPrincipal,
                            new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTime.UtcNow.AddDays(7)
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
