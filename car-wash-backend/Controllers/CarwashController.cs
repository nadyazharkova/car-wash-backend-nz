using car_wash_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_wash_backend.Controllers;

public class CarwashController : Controller
{
    
    private readonly CarWashContext _context;

    public CarwashController(CarWashContext context)
    {
        _context = context;
    }

    // [HttpGet("/owner")]
    // public async Task<ActionResult<IEnumerable<Carwash>>> GetCarwashes()
    // {
    //     return await _context.Carwashes.ToListAsync();
    // }
    //
    // [HttpPut("/Carwash/{id}")]
    // public IActionResult PutCarwash(int id)
    // {
    //     return Ok("Hi there" + id);
    // }
}