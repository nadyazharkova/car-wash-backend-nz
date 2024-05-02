using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
    
    [JsonIgnore]
    public virtual Box Box { get; set; } = null!;

    [JsonIgnore]
    public virtual Carwash Carwash { get; set; } = null!;
    
    public virtual ICollection<ServicesInOrder> ServicesInOrders { get; set; } = new List<ServicesInOrder>();
    
    [JsonIgnore]
    public virtual OrderStatus Status { get; set; } = null!;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
