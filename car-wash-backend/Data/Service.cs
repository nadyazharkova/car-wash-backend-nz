namespace car_wash_backend.Data;

public class Service
{
    public Guid Id { get; set; }
    public string Name {get; set;}
    public int Price {get; set;}
    public TimeOnly Time {get; set;}
    public ServiceStatus Status { get; set; }
    public Carwash Carwash { get; set; }
}