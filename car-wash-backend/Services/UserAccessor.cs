using System.Security.Claims;

namespace car_wash_backend.Services;

public class UserAccessor
{
    private readonly IHttpContextAccessor _accessor;
    
    public UserAccessor(IHttpContextAccessor accessor)
    {
        Id = Guid.Parse(accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        RoleId = Guid.Parse(accessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
    }
    
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public Guid PersonId { get; set; }
}