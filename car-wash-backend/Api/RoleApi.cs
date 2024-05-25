using car_wash_backend.Services;

namespace car_wash_backend.Api;

public static class RoleApi
{
    public static RouteGroupBuilder MapRoleApi(this RouteGroupBuilder builder)
    {
        builder.MapGet("/{id}", (Guid id, RoleAccessor roleAccessor) => roleAccessor.GetById(id));
        
        builder.MapGet("", (RoleAccessor roleAccessor) => roleAccessor.GetAll());
        builder.MapGet("/{id}/{requiredRole}",
            (Guid id, string requiredRole, RoleAccessor roleAccessor) => roleAccessor.CheckRole(id, requiredRole));
        
        return builder;
    }
}