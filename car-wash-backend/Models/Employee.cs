using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class Employee
{
    public Guid EmployeeId { get; set; }

    public Guid UserId { get; set; }

    public Guid CarwashId { get; set; }

    public virtual Carwash Carwash { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
