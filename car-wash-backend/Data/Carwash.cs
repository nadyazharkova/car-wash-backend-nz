namespace car_wash_backend.Data;

public class Carwash
{
    public Guid Id{ get; set; }
    public string Name {get; set;}
    public string CarwashStreet {get; set;}
    public int BoxAmount {get; set;}
    public string ContactInfo {get; set;}
    
    public List<Service> Services { get; set; }
    public List<Box> Boxes { get; set; }
}