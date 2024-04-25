using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class DayType
{
    public Guid TypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Day> Days { get; set; } = new List<Day>();
}
