using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;

namespace car_wash_backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
    {
        
    }
    
    // public DbSet<Carwash> Carwashes {get; set;} 
    // public DbSet<Order> Orders { get; set; }
    // public DbSet<User> Users { get; set; }
    // public DbSet<Person> People { get; set; }
}