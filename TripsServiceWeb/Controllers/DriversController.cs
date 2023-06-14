using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Drivers;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class DriversController : Controller
    {

        private readonly IDriverService _driverService;

        private readonly IWebHostEnvironment _environment;

        public DriversController(
            IDriverService driverService,
            IWebHostEnvironment environment
            )
        {
            _driverService = driverService;
            _environment = environment;
        }

        public IActionResult Index()
        {
            IQueryable<ReadDriverDTO> drivers = new GetDriversOverallCommand(_driverService).Execute();
            return View(drivers);
        }
    }
}
