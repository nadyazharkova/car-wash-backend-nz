using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using car_wash_backend.Models;

namespace car_wash_backend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // public IActionResult Index()
    // {
    //     return Ok("Hi there");
    // }
    
}