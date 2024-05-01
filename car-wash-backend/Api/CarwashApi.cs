using System.Collections.Immutable;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using car_wash_backend.Dto;
using car_wash_backend.Models;
using car_wash_backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace car_wash_backend.Api;

public static class CarwashApi
{
    
    public static RouteGroupBuilder MapCarwashesApi(this RouteGroupBuilder builder)
    {
        builder.MapGet("", (CarwashAccessor carwashAccessor) => carwashAccessor.GetAll());
        
        builder.MapPost("", ( CarwashAccessor carwashAccessor, CarwashDto dto) => carwashAccessor.Create(dto));
        
        // builder.MapPut("/{id}", Results<Ok<Carwash>, BadRequest<string>> (Guid id, CarwashAccessor carwashAccessor, CarwashDto dto)
        // {
        //     if (id != dto.Id)
        //         return BadRequest("Идентификаторы не совпадают");
        //     return Ok(carwashAccessor.Update(dto));
        // });

        builder.MapDelete("/{id}", (Guid id, CarwashAccessor carwashAccessor) => carwashAccessor.Delete(id));
        
        return builder;
    }
}