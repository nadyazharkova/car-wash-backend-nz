using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class Schedule
{
    public Guid ScheduleId { get; set; }

    public Guid DayId { get; set; }

    public Guid CarwashId { get; set; }

    public virtual Day Day { get; set; } = null!;
}
