using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace car_wash_backend.Models;

public partial class BoxesInCarwash
{
    public Guid CarwashId { get; set; }

    public long BoxId { get; set; }

    public Guid BoxesInCarwashId { get; set; }
    
    [JsonIgnore]
    public virtual Box Box { get; set; } = null!;

    [JsonIgnore]
    public virtual Carwash Carwash { get; set; } = null!;
}
