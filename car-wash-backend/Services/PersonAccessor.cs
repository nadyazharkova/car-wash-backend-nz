using System.Net;
using System.Web.Http;
using car_wash_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace car_wash_backend.Services;

public class PersonAccessor (CarWashContext db) : Controller
{
    public IQueryable<Person> GetAll()
    {
        return db.People;
    }
    
    public Person? GetById(Guid id)
    {
        var person = GetAll().FirstOrDefault( u => u.PersonId == id);
        return person;
    }
    
    public IActionResult Create(Person personData)
    {
        var personId = Guid.NewGuid();

        var newPerson = new Person()
        {
            PersonId = personId,
            FirstName = personData.FirstName,
            LastName = personData.LastName,
            FathersName = personData.FirstName,
            PhoneNumber = personData.PhoneNumber,
            Email = personData.Email,
        };
        
        db.People.Add(newPerson);
        
        db.SaveChanges();
    
        // Получаем данные с помощью GetById и возвращаем их со статусом OK
        var result = GetById(personId);
        return Ok(result);
    }

    public IActionResult Update(Guid id, Person personData)
    {
        if (id != personData.PersonId) 
            return BadRequest("Идентификаторы не совпадают");
        
        var updatedPerson = ValidatePersonChange(id);
        updatedPerson.FirstName = personData.FirstName;
        updatedPerson.LastName = personData.LastName;
        updatedPerson.FathersName = personData.FathersName;
        updatedPerson.PhoneNumber = personData.PhoneNumber;
        updatedPerson.Email = personData.Email;

        db.SaveChanges();
        
        return Ok(GetById(id));
    }

    public void Delete(Guid id)
    {
        var person = ValidatePersonChange(id);
        db.People.Remove(person);
        db.SaveChanges();
    }

    public Person ValidatePersonChange(Guid? id)
    {
        var person = db.People.FirstOrDefault(p => p.PersonId == id);
        if (person == null)
            throw new HttpResponseException(
                new HttpResponseMessage(HttpStatusCode.NotFound)
                    { Content = new StringContent("Пользователь не найден") });
        return person;
    }
}