using System.Net;
using System.Web.Http;
using car_wash_backend.Dto;
using car_wash_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace car_wash_backend.Services;

public class OrderAccessor (CarWashContext db, OrderStatusAccessor statusAccessor, 
    CarwashAccessor carwashAccessor, UserAccessor userAccessor) : Controller
{
    public IQueryable<Order> GetAll()
    {
        return db.Orders;
    }
    
    public Order? GetById(Guid id)
    {
        var order = GetAll().FirstOrDefault( u => u.OrderId == id);
        return order;
    }
    
    public IActionResult Create(Order orderData)
    {
        var orderID = Guid.NewGuid();
        var defaultStatus = statusAccessor.GetDefaultStatus();

        var order = new Order()
        {
            OrderId = orderID,
            DateTime = orderData.DateTime,
            CarwashId = orderData.CarwashId,
            Carwash = carwashAccessor.GetById(orderData.CarwashId),
            UserId = orderData.UserId,
            User = userAccessor.GetById(orderData.UserId),
            StatusId = defaultStatus.StatusId,
            Status = defaultStatus,
            BoxId = 
                
        };
        
        db.Orders.Add(order);
        db.SaveChanges();
    
        // Получаем данные с помощью GetById и возвращаем их со статусом OK
        var result = GetById(orderID);
        return Ok(result);
    }

    public IActionResult Update(Guid id, Order orderData)
    {
        if (id != orderData.OrderId) 
            return BadRequest("Идентификаторы не совпадают");
        var updatedOrder = ValidateOrderChange(id);
        var defaultStatus = statusAccessor.GetDefaultStatus();
        
        updatedOrder.DateTime = orderData.DateTime;
        updatedOrder.CarwashId = orderData.CarwashId;
        updatedOrder.Carwash = orderData.Carwash;
        updatedOrder.UserId = orderData.UserId;
        updatedOrder.User = orderData.User;
        updatedOrder.StatusId = defaultStatus.StatusId;
        updatedOrder.Status = defaultStatus;

        db.SaveChanges();
        
        return Ok(GetById(id));
    }

    public void Delete(Guid id)
    {
        var person = ValidateOrderChange(id);
        db.People.Remove(person);
        db.SaveChanges();
    }

    public Order ValidateOrderChange(Guid? id)
    {
        var order = db.Orders.FirstOrDefault(p => p.OrderId == id);
        if (order == null)
            throw new HttpResponseException(
                new HttpResponseMessage(HttpStatusCode.NotFound)
                    { Content = new StringContent("Бронь не найдена") });
        return order;
    }
}