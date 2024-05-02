using car_wash_backend.Models;
using System.Net;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace car_wash_backend.Services;

public class ServicesAccessor(CarWashContext db, ServiceStatusAccessor statusAccessor) : Controller
{
    public IQueryable<Service> GetAll()
    {
        return db.Services;
    }
    
    public Service? GetById(Guid id)
    {
        var service = GetAll().FirstOrDefault( s => s.ServiceId == id);
        return service;
    }
    
    public IActionResult Create(Service serviceData)
    {
        var serviceId = Guid.NewGuid();
        var newService = new Service()
        {
            ServiceId = serviceId,
            Name = serviceData.Name,
            Price = serviceData.Price,
            Duration = serviceData.Duration,
            Status = statusAccessor.GetByName("available"),
            CarwashId = serviceData.CarwashId
        };
        db.Services.Add(newService);

        db.SaveChanges();
    
        // Получаем данные с помощью GetById и возвращаем их со статусом OK
        var result = GetById(serviceId);
        return Ok(result);
    }

    public IActionResult Update(Guid id, Service service)
    {
        if (id != service.ServiceId ) 
            return BadRequest("Идентификаторы не совпадают");
        
        var updatedService = ValidateServiceChange(service.ServiceId);
        updatedService.Name = service.Name;
        updatedService.Price = service.Price;
        updatedService.Duration = service.Duration;
        updatedService.Status = service.Status;

        db.SaveChanges();
        
        return Ok(GetById(updatedService.ServiceId));
    }

    public void Delete(Guid id)
    {
        var service = ValidateServiceChange(id);
        db.Services.Remove(service);
        db.SaveChanges();
    }

    public Service ValidateServiceChange(Guid? id)
    {
        var service = db.Services.FirstOrDefault(s => s.ServiceId == id);
        if (service == null)
            throw new HttpResponseException(
                new HttpResponseMessage(HttpStatusCode.NotFound)
                    { Content = new StringContent("Услуга не найдена") });
        // if (carwash.CarwashId != userAccessor.Id)
        //     throw new HttpResponseException(
        //         new HttpResponseMessage(HttpStatusCode.Forbidden)
        //             { Content = new StringContent("Вы не можете редактировать эту автомойку") });
        return service;
    }
}