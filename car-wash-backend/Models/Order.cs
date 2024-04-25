using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class Order
{
    public DateOnly DateTime { get; set; }

    public string? LicencePlate { get; set; }

    public long BoxId { get; set; }

    public Guid OrderId { get; set; }

    public Guid CarwashId { get; set; }

    public Guid StatusId { get; set; }

    public Guid UserId { get; set; }

    public virtual Box Box { get; set; } = null!;

    public virtual Carwash Carwash { get; set; } = null!;

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
