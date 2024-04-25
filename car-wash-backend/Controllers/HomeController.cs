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

    [HttpPut("/Carwash/{id}")]
    public IActionResult Index(int id)
    {
        return Ok("Hi there" + id);
    }
    
    [HttpGet("/Carwash")]
    public IActionResult Index()
    {
        return Ok();
    }
    
}