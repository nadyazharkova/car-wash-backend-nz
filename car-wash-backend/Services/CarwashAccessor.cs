using System.Net;
using System.Web.Http;
using car_wash_backend.Dto;
using car_wash_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace car_wash_backend.Services;

public class CarwashAccessor(CarWashContext db, UserAccessor userAccessor, ServicesAccessor servicesAccessor) : Controller
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
        carwash.Services = servicesAccessor.GetAll().ToList();
        return carwash;
    }
    
    public IActionResult Create(CarwashDto dto)
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
        
        var lastBoxId = db.Boxes.Count();

        for (var i = 1; i <= dto.BoxAmount; i++)
        {
            var box = new Box();
            db.Boxes.Add(box);
            
            var boxInCarwashId = Guid.NewGuid();
        
            var boxInCarwash = new BoxesInCarwash()
            {
                BoxesInCarwashId = boxInCarwashId,
                BoxId = lastBoxId + i,
                CarwashId = carwashId,
                Carwash = newCarwash,
                Box = box,
            };
            db.BoxesInCarwashes.Add(boxInCarwash);
        }
        

        db.SaveChanges();
        
        
    
        // Получаем данные с помощью GetById и возвращаем их со статусом OK
        var result = GetById(carwashId);
        return Ok(result);
    }

    public IActionResult Update(Guid id, Carwash carwash)
    {
        if (id != carwash.CarwashId ) 
            return BadRequest("Идентификаторы не совпадают");
        
        var updatedCarwash = ValidateCarwashChange(carwash.CarwashId);
        updatedCarwash.Name = carwash.Name;
        updatedCarwash.CarwashStreet = carwash.CarwashStreet;
        updatedCarwash.BoxAmount = carwash.BoxAmount;
        updatedCarwash.ContactInfo = carwash.ContactInfo;

        db.SaveChanges();
        
        return Ok(GetById(updatedCarwash.CarwashId));
    }

    public void Delete(Guid id)
    {
        var carwash = ValidateCarwashChange(id);
        db.Carwashes.Remove(carwash);
        db.SaveChanges();
    }

    public Carwash ValidateCarwashChange(Guid? id)
    {
        var carwash = db.Carwashes.FirstOrDefault(c => c.CarwashId == id);
        if (carwash == null)
            throw new HttpResponseException(
                new HttpResponseMessage(HttpStatusCode.NotFound)
                    { Content = new StringContent("Автомойка не найдена") });
        // if (carwash.CarwashId != userAccessor.Id)
        //     throw new HttpResponseException(
        //         new HttpResponseMessage(HttpStatusCode.Forbidden)
        //             { Content = new StringContent("Вы не можете редактировать эту автомойку") });
        return carwash;
    }
}