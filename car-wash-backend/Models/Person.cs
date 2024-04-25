using System;
using System.Collections.Generic;

namespace car_wash_backend.Models;

public partial class Person
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? FathersName { get; set; }

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public Guid PersonId { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
