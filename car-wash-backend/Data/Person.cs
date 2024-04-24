namespace car_wash_backend.Data;

public class Person
{
    public Guid Id { get; set; }
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string? FathersName {get; set;}
    public string Email {get; set;}
    public string PhoneNumber {get; set;}
}