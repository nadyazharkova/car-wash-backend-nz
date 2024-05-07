using car_wash_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace car_wash_backend.Services;

public class RoleAccessor(CarWashContext db) : Controller
{
    public IQueryable<Role> GetAll()
    {
        return db.Roles;
    }
    
    public Role? GetById(Guid id)
    {
        var role = GetAll().FirstOrDefault( r => r.RoleId == id);
        return role;
    }

    public bool CheckRole(Guid id, string requiredRole)
    {
        return GetById(id)?.RoleName == requiredRole;
    }
}