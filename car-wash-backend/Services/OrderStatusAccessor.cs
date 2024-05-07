using System.Net;
using System.Web.Http;
using car_wash_backend.Dto;
using car_wash_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace car_wash_backend.Services;

public class OrderStatusAccessor(CarWashContext db) : Controller
{
    public IQueryable<OrderStatus> GetAll()
    {
        return db.OrderStatuses;
    }
    
    public OrderStatus? GetById(Guid id)
    {
        var status = GetAll().FirstOrDefault( s => s.StatusId == id);
        return status;
    }

    public OrderStatus GetDefaultStatus()
    {
        var createdStatus = GetAll().FirstOrDefault(s => s.StatusName == OrderStatusName.Created);
        if (createdStatus == null)
            throw new HttpResponseException(
                new HttpResponseMessage(HttpStatusCode.NotFound)
                    { Content = new StringContent("Статус по умолчанию не найден") });
        return createdStatus;
    }

    public bool CheckStatus(Guid id, string requiredStatus)
    {
        return GetById(id)?.StatusName == requiredStatus;
    }
}