using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace car_wash_backend.Models;

public partial class Box
{
    public long BoxId { get; set; }
    
    [JsonIgnore]
    public virtual BoxesInCarwash? BoxesInCarwash { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
