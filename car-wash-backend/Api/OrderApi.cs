using car_wash_backend.Models;
using car_wash_backend.Services;

namespace car_wash_backend.Api;

public static class OrderApi
{
    public static RouteGroupBuilder MapOrderApi(this RouteGroupBuilder builder)
    {
        builder.MapGet("/{id}", (Guid id, OrderAccessor orderAccessor) => orderAccessor.GetById(id));
        
        builder.MapGet("", (OrderAccessor orderAccessor) => orderAccessor.GetAll());
        
        builder.MapPost("", ( OrderAccessor orderAccessor, Order order) => orderAccessor.Create(order));
        
        builder.MapPut("/{id}", (Guid id, OrderAccessor orderAccessor, Order order) => orderAccessor.Update(id, order));

        builder.MapDelete("/{id}", (Guid id, OrderAccessor orderAccessor) => orderAccessor.Delete(id));
        
        return builder;
    }
}