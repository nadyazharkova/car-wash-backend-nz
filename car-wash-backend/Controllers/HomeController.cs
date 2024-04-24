using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using car_wash_backend.Models;

namespace car_wash_backend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICarwashService _carwashService;

    public HomeController(ILogger<HomeController> logger, ICarwashService carwashService)
    {
        _logger = logger;
        _carwashService = carwashService;
    }

    [HttpPut("/Carwash/{id}")]
    public IActionResult Index(int id)
    {
        return Ok("Hi there" + id);
    }
    
    [HttpGet("/Carwash")]
    public IActionResult Index()
    {
        return Ok(_carwashService.Carwash());
    }
    
}