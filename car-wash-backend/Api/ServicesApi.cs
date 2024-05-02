using car_wash_backend.Models;
using car_wash_backend.Services;

namespace car_wash_backend.Api;

public static class ServicesApi
{
    public static RouteGroupBuilder MapServicesApi(this RouteGroupBuilder builder)
    {
        builder.MapGet("", async ( ServicesAccessor servicesAccessor) => servicesAccessor.GetAll());
        
        
        builder.MapPost("", ( ServicesAccessor servicesAccessor, Service service) => 
            servicesAccessor.Create(service));
        
        builder.MapPut("/{id}", (Guid id, ServicesAccessor servicesAccessor, Service service) => 
            servicesAccessor.Update(id, service));

        builder.MapDelete("/{id}", (Guid id, ServicesAccessor servicesAccessor) => servicesAccessor.Delete(id));
        
        return builder;
    }
}