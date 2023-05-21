using Microsoft.AspNetCore.Mvc;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class TripsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
