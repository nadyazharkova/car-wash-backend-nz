using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace car_wash_backend.Models;

public partial class Day
{
    public string StartTime { get; set; } = null!;

    public string EndTime { get; set; } = null!;

    public Guid DayId { get; set; }

    public Guid TypeId { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    [JsonIgnore]
    public virtual DayType Type { get; set; } = null!;
}
