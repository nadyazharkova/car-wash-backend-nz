using System.Net;
using System.Web.Http;
using car_wash_backend.Models;

namespace car_wash_backend.Services;

public class ServiceStatusAccessor (CarWashContext db)
{
    public IQueryable<ServiceStatus> GetAll()
    {
        return db.ServiceStatuses;
    }
    
    public ServiceStatus GetDefaultStatus()
    {
        var service = GetAll().FirstOrDefault( s => s.StatusName == ServiceStatusName.Available);
        if (service == null)
            throw new HttpResponseException(
                new HttpResponseMessage(HttpStatusCode.NotFound)
                    { Content = new StringContent("Статус по умолчанию не найден") });
        return service;
    }
}