using car_wash_backend.Models;

namespace car_wash_backend.Services;

public class ServiceStatusAccessor (CarWashContext db)
{
    public IQueryable<ServiceStatus> GetAll()
    {
        return db.ServiceStatuses;
    }
    
    public ServiceStatus? GetByName(string name)
    {
        var service = GetAll().FirstOrDefault( s => s.StatusName == name);
        return service;
    }
}