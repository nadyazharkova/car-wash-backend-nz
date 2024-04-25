using System.Collections.Immutable;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using car_wash_backend.Models;
using car_wash_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace car_wash_backend.Api;

public static class CarwashApi
{
    
    public static RouteGroupBuilder MapCarwashesApi(this RouteGroupBuilder builder)
    {
        builder.MapGet("", async (UserAccessor userAccessor, CarWashContext result) =>
        {
            //берем id самого первого владельца

            var userId = userAccessor.Id;
            var carwashesIds = result.Employees.
                Where(e => e.UserId == userId)
                .Select(e => e.CarwashId)
                .ToList();
            return await result.Carwashes.Where(c => carwashesIds.Contains(c.CarwashId)).ToListAsync();
        });

        return builder;
    }
    
    // public static RouteGroupBuilder MapCarwashChange(this RouteGroupBuilder builder)
    // {
    //     builder.MapGet("", async (UserAccessor userAccessor, CarWashContext result) =>
    //     {
    //         //берем id самого первого владельца
    //
    //         var userId = userAccessor.Id;
    //         var carwashesIds = result.Employees.
    //             Where(e => e.UserId == userId)
    //             .Select(e => e.CarwashId)
    //             .ToList();
    //         return await result.Carwashes.Where(c => carwashesIds.Contains(c.CarwashId)).ToListAsync();
    //     });
    //
    //     return builder;
    // }
}