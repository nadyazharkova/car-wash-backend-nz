using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class Service
{
    public string Name { get; set; } = null!;

    public long Price { get; set; }

    public string Duration { get; set; } = null!;

    public Guid ServiceId { get; set; }

    public Guid StatusId { get; set; }

    public Guid CarwashId { get; set; }

    public virtual Carwash Carwash { get; set; } = null!;

    public virtual ServiceStatus Status { get; set; } = null!;
}
