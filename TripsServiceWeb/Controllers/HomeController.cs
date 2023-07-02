using System.Diagnostics;
using Andrei_Mikhaleu_Task1.Models;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _environment;

    private readonly IImageService _imageService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, IImageService imageService)
    {
        _logger = logger;
        _environment = environment;
        _imageService = imageService;
    }

    public IActionResult Index()
    {
        _imageService.CreateImagesDirectory(_environment.WebRootPath);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}