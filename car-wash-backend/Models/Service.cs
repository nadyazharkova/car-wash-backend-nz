using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace car_wash_backend.Models;

public partial class Service
{
    public string Name { get; set; } = null!;

    public long Price { get; set; }

    public string Duration { get; set; } = null!;

    public Guid ServiceId { get; set; }

    public Guid StatusId { get; set; }

    public Guid CarwashId { get; set; }

    [JsonIgnore]
    public virtual Carwash Carwash { get; set; } = null!;

    [JsonIgnore]
    public virtual ServicesInOrder? ServicesInOrder { get; set; }

    [JsonIgnore]
    public virtual ServiceStatus Status { get; set; } = null!;
}
