using car_wash_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_wash_backend.Controllers;

public class CarwashController : Controller
{
    
    [HttpGet("/server/Carwash")]
    public async Task<ActionResult<IEnumerable<Carwash>>> GetCarwashes(CarWashContext db)
    {
        return await db.Carwashes.ToListAsync();
    }
}