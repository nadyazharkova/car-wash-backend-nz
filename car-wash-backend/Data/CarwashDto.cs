using System.ComponentModel.DataAnnotations.Schema;
using car_wash_backend.Models;

namespace car_wash_backend.Dto;

public class CarwashDto
{
    [Column("carwashId")]
    public Guid? Id { get; set; }
    public string? Name { get; set; } 

    public string? CarwashStreet { get; set; } 

    public int BoxAmount { get; set; }

    public string? ContactInfo { get; set; }

    public string? EmployeeId { get; set; }
    
    public List<Service>? Services { get; set; }
}