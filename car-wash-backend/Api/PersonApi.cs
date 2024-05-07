using car_wash_backend.Models;
using car_wash_backend.Services;

namespace car_wash_backend.Api;

public static class PersonApi
{
    public static RouteGroupBuilder MapPersonApi(this RouteGroupBuilder builder)
    {
        builder.MapGet("/{id}", (Guid id, PersonAccessor personAccessor) => personAccessor.GetById(id));
        
        builder.MapPut("/{id}", (Guid id, PersonAccessor personAccessor, Person person) => personAccessor.Update(id, person));
        
        return builder;
    }
}