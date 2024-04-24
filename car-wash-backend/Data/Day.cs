namespace car_wash_backend.Data;

public class Day
{
    public Guid Id { get; set; }
    public DayType Type { get; set; }
    public TimeOnly StartTime {get; set;}
    public TimeOnly EndTime {get; set;}
}