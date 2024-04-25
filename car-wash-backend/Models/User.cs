using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class User
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public Guid PersonId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Person Person { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
