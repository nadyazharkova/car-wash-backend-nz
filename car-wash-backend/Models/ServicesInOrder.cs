using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace car_wash_backend.Models;

public partial class ServicesInOrder
{
    public Guid ServiceId { get; set; }

    public Guid OrderId { get; set; }

    public Guid ServicesInOrderId { get; set; }

    [JsonIgnore]
    public virtual Order Order { get; set; } = null!;
    
    [JsonIgnore]
    public virtual Service Service { get; set; } = null!;
}
