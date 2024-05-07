using car_wash_backend.Dto;
using car_wash_backend.Models;
using car_wash_backend.Services;

namespace car_wash_backend.Api;

public static class CarwashApi
{
    
    public static RouteGroupBuilder MapCarwashesApi(this RouteGroupBuilder builder)
    {
        builder.MapGet("", (CarwashAccessor carwashAccessor) => carwashAccessor.GetAll());
        
        builder.MapPost("", ( CarwashAccessor carwashAccessor, CarwashDto dto) => carwashAccessor.Create(dto));
        
        builder.MapPut("/{id}", (Guid id, CarwashAccessor carwashAccessor, Carwash carwash) => carwashAccessor.Update(id, carwash));

        builder.MapDelete("/{id}", (Guid id, CarwashAccessor carwashAccessor) => carwashAccessor.Delete(id));
        
        return builder;
    }
}