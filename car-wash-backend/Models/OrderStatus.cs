using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class OrderStatus
{
    public string StatusName { get; set; } = null!;

    public Guid StatusId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
