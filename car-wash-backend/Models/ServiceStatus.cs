using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class ServiceStatus
{
    public string StatusName { get; set; } = null!;

    public Guid StatusId { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
