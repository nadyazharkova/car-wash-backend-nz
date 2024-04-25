using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class ServicesInOrder
{
    public Guid ServiceId { get; set; }

    public Guid? ServicesInOrderId { get; set; }

    public Guid OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
