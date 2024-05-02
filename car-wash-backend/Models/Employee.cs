using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace car_wash_backend.Models;

public partial class Employee
{
    public Guid EmployeeId { get; set; }

    public Guid UserId { get; set; }

    public Guid CarwashId { get; set; }
    
    [JsonIgnore]
    public virtual Carwash Carwash { get; set; } = null!;
    
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
