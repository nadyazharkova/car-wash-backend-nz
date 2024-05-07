using car_wash_backend.Models;
using car_wash_backend.Services;

namespace car_wash_backend.Api;

public static class UserApi
{
    public static RouteGroupBuilder MapUserApi(this RouteGroupBuilder builder)
    {
        builder.MapGet("/{id}", async (Guid id, UserAccessor userAccessor) => userAccessor.GetById(id));
        
        builder.MapPost("", ( UserAccessor servicesAccessor, User service) => 
            servicesAccessor.Create(service));
        
        // builder.MapPut("/{id}", (Guid id, UserAccessor userAccessor, User user) => 
        //     userAccessor.Update(id, user));

        builder.MapDelete("/{id}", (Guid id, UserAccessor userAccessor) => userAccessor.Delete(id));
        
        return builder;
    }
}