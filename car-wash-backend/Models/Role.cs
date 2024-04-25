using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class Role
{
    public string RoleName { get; set; } = null!;

    public Guid RoleId { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
