using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class Box
{
    public long BoxId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
