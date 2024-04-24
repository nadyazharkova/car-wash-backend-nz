namespace car_wash_backend.Data;

public class Schedule
{
    public Guid Id { get; set; }
    public Carwash Carwash {get; set;}
    public Day Day { get; set; }
}