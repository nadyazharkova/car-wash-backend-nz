using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class Carwash
{
    public string Name { get; set; } = null!;

    public string CarwashStreet { get; set; } = null!;

    public int BoxAmount { get; set; }

    public string ContactInfo { get; set; } = null!;

    public Guid CarwashId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
