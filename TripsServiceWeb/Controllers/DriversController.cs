using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Drivers;
using TripsServiceBLL.Commands.Trips;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Services;

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

        [Authorize]
        [HttpGet]
        public IActionResult Index()
		{
			IEnumerable<ReadDriverDTO> drivers = new GetDriversOverallCommand(_driverService).Execute();
			return View(drivers);
		}

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                DriverDetailsDTO driver = await new GetDriverDetailsCommand(_driverService, id).ExecuteAsync();
                return View(driver);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
