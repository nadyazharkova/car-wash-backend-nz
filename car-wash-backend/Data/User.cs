namespace car_wash_backend.Data;

public class User
{
    public Guid Id { get; set; }
    public Role Role {get; set;}
    public string Login {get; set;}
    public string Password {get; set;}
    public Person Person {get; set;}
    
    public List<Order> Orders { get; set; }
}