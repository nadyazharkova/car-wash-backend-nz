using System.Net;
using System.Security.Claims;
using System.Web.Http;
using car_wash_backend.Dto;
using car_wash_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace car_wash_backend.Services;

public class UserAccessor : Controller
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public Guid PersonId { get; set; }
    
    private readonly CarWashContext _db;
    private readonly PersonAccessor _personAccessor;
    
    public UserAccessor(IHttpContextAccessor? accessor, CarWashContext db, PersonAccessor personAccessor)
    {
        Id = Guid.Parse(accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        RoleId = Guid.Parse(accessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
        _db = db;
        _personAccessor = personAccessor;
    }
    
    public IQueryable<User> GetAll()
    {
        return _db.Users;
    }
    
    public User? GetById(Guid id)
    {
        var user = GetAll().FirstOrDefault( u => u.UserId == id);
        return user;
    }
    
    public IActionResult Create(User userData)
    {
        var userId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        var newPerson = new Person()
        {
            PersonId = personId,
            FirstName = "",
            LastName = "",
            PhoneNumber = "",
        };
        
        var newUser = new User()
        {
            UserId = userId,
            RoleId = userData.RoleId,
            Login = userData.Login,
            Password = userData.Password,
            Person = newPerson,
            PersonId = personId,
        };
        
        _db.People.Add(newPerson);
        _db.Users.Add(newUser);
        
        _db.SaveChanges();
    
        // Получаем данные с помощью GetById и возвращаем их со статусом OK
        var result = GetById(userId);
        return Ok(result);
    }

    public IActionResult Update(Guid id, User user)
    {
        if (id != user.UserId) 
            return BadRequest("Идентификаторы не совпадают");
        
        var updatedUser = ValidateUserChange(user.UserId);
        updatedUser.Login = user.Login;
        updatedUser.Password = user.Password;

        _db.SaveChanges();
        
        return Ok(GetById(updatedUser.UserId));
    }

    public void Delete(Guid id)
    {
        var user = ValidateUserChange(id);
        _db.Users.Remove(user);
        _personAccessor.Delete(user.PersonId);
        _db.SaveChanges();
    }

    public User ValidateUserChange(Guid? id)
    {
        var user = _db.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
            throw new HttpResponseException(
                new HttpResponseMessage(HttpStatusCode.NotFound)
                    { Content = new StringContent("Пользователь не найден") });
        return user;
    }
}