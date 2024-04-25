using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class BoxesInCarwash
{
    public Guid CarwashId { get; set; }

    public Guid? BoxesInCarwashId { get; set; }

    public long BoxId { get; set; }

    public virtual Box Box { get; set; } = null!;

    public virtual Carwash Carwash { get; set; } = null!;
}
