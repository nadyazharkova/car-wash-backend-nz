using car_wash_backend.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;
using System.Web.Http;

namespace car_wash_backend.Services;

public class ServicesAccessor(CarWashContext db)
{
    public IQueryable<Service> GetAll()
    {
        return db.Services;
    }
}