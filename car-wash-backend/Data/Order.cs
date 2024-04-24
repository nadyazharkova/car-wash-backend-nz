namespace car_wash_backend.Data;

public class Order
{
    public Guid Id { get; set; }
    public DateTime DateTime {get; set;}
    public string CarwashID {get; set;}
    public User User {get; set;}
    public string? LicensePlate {get; set;}
    public OrderStatus Status {get; set;}
    public int BoxID { get; set; }
    
    public List<Service> Services { get; set; }
}