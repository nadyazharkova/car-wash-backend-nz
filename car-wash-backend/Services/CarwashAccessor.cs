using System.Diagnostics;
using System.Net;
using System.Web.Http;
using car_wash_backend.Dto;
using car_wash_backend.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace car_wash_backend.Services;

public class CarwashAccessor(CarWashContext db, UserAccessor userAccessor
        //, ServicesAccessor servicesAccessor
    )
{
    
    public IQueryable<Carwash> GetAll()
    {
        var userId = userAccessor.Id;
        var carwashesIds = db.Employees.
            Where(e => e.UserId == userId)
            .Select(e => e.CarwashId)
            .ToList();
        return db.Carwashes.Where(c => carwashesIds.Contains(c.CarwashId));
    }
    
    public Carwash? GetById(Guid id)
    {
        var carwash = GetAll().FirstOrDefault( c => c.CarwashId == id);
        if (carwash == null)
            return null;
        //carwash.Services = servicesAccessor.GetAll();
        return carwash;
    }

    public Carwash Create(CarwashDto dto)
    {
        var carwashId = Guid.NewGuid();
        var newCarwash = new Carwash()
        {
            CarwashId = carwashId,
            Name = dto.Name,
            CarwashStreet = dto.CarwashStreet,
            BoxAmount = dto.BoxAmount,
            ContactInfo = dto.ContactInfo
        };
        db.Carwashes.Add(newCarwash);
        
        var ownerId = Guid.NewGuid();
        var newEmployee = new Employee
        {
            EmployeeId = ownerId,
            CarwashId = carwashId,
            Carwash = newCarwash,
            UserId = userAccessor.Id
        };
        // Добавляем владельца с прикрепленной автомойкой в БД
        db.Employees.Add(newEmployee);

        db.SaveChanges();
        
        return GetById(newCarwash.CarwashId);
    }

    public Carwash Update(CarwashDto dto)
    {
        var carwash = ValidateCarwashChange(dto.Id);
        carwash.Name = dto.Name;
        carwash.CarwashStreet = dto.CarwashStreet;
        carwash.BoxAmount = dto.BoxAmount;
        carwash.ContactInfo = dto.ContactInfo;

        db.SaveChanges();
        return GetById(carwash.CarwashId);
    }

    public void Delete(Guid id)
    {
        var carwash = ValidateCarwashChange(id);
        db.SaveChanges();
    }

    public Carwash ValidateCarwashChange(Guid? id)
    {
        var carwash = db.Carwashes.FirstOrDefault(c => c.CarwashId == id);
        if (carwash == null)
            throw new HttpResponseException(
                new HttpResponseMessage(HttpStatusCode.NotFound)
                    { Content = new StringContent("Автомойка не найдена") 
                        //ReasonPhrase = message
                        });
        if (carwash.CarwashId != userAccessor.Id)
            throw new HttpResponseException(
                new HttpResponseMessage(HttpStatusCode.Forbidden)
                    { Content = new StringContent("Вы не можете редактировать эту автомойку") });
        return carwash;
    }
}