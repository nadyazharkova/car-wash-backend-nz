using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace car_wash_backend.Models;

public partial class CarWashContext : DbContext
{
    public CarWashContext()
    {
    }

    public CarWashContext(DbContextOptions<CarWashContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Box> Boxes { get; set; }

    public virtual DbSet<BoxesInCarwash> BoxesInCarwashes { get; set; }

    public virtual DbSet<Carwash> Carwashes { get; set; }

    public virtual DbSet<Day> Days { get; set; }

    public virtual DbSet<DayType> DayTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceStatus> ServiceStatuses { get; set; }

    public virtual DbSet<ServicesInOrder> ServicesInOrders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Username=postgres;Password=P@ssw0rd;Host=localhost;Port=5432;Database=car-wash;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Box>(entity =>
        {
            entity.HasKey(e => e.BoxId).HasName("Box_pkey");

            entity.ToTable("Box");

            entity.Property(e => e.BoxId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("box_ID");
        });

        modelBuilder.Entity<BoxesInCarwash>(entity =>
        {
            entity.HasKey(e => e.BoxesInCarwashId).HasName("BoxesInCarwash_pkey");

            entity.ToTable("BoxesInCarwash");

            entity.HasIndex(e => e.BoxId, "pk_box_ID").IsUnique();

            entity.Property(e => e.BoxesInCarwashId)
                .ValueGeneratedNever()
                .HasColumnName("boxesInCarwash_ID");
            entity.Property(e => e.BoxId).HasColumnName("box_ID");
            entity.Property(e => e.CarwashId).HasColumnName("carwash_ID");

            entity.HasOne(d => d.Box).WithOne(p => p.BoxesInCarwash)
                .HasForeignKey<BoxesInCarwash>(d => d.BoxId)
                .HasConstraintName("fk_box_ID");

            entity.HasOne(d => d.Carwash).WithMany(p => p.BoxesInCarwashes)
                .HasForeignKey(d => d.CarwashId)
                .HasConstraintName("fk_carwash_ID");
        });

        modelBuilder.Entity<Carwash>(entity =>
        {
            entity.HasKey(e => e.CarwashId).HasName("Carwashes_pkey");

            entity.ToTable("Carwash");

            entity.Property(e => e.CarwashId)
                .ValueGeneratedNever()
                .HasColumnName("carwash_ID");
            entity.Property(e => e.BoxAmount).HasColumnName("boxAmount");
            entity.Property(e => e.CarwashStreet)
                .HasMaxLength(100)
                .HasColumnName("carwashStreet");
            entity.Property(e => e.ContactInfo)
                .HasMaxLength(250)
                .HasColumnName("contactInfo");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Day>(entity =>
        {
            entity.HasKey(e => e.DayId).HasName("Day_pkey");

            entity.ToTable("Day");

            entity.Property(e => e.DayId)
                .ValueGeneratedNever()
                .HasColumnName("day_ID");
            entity.Property(e => e.EndTime)
                .HasMaxLength(50)
                .HasColumnName("endTime");
            entity.Property(e => e.StartTime)
                .HasMaxLength(50)
                .HasColumnName("startTime");
            entity.Property(e => e.TypeId).HasColumnName("type_ID");

            entity.HasOne(d => d.Type).WithMany(p => p.Days)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("fk_type_ID");
        });

        modelBuilder.Entity<DayType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("DayType_pkey");

            entity.ToTable("DayType");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("type_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("Employees_pkey");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("employee_ID");
            entity.Property(e => e.CarwashId).HasColumnName("carwash_ID");
            entity.Property(e => e.UserId).HasColumnName("user_ID");

            entity.HasOne(d => d.Carwash).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CarwashId)
                .HasConstraintName("fk_carwash_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_ID");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("Orders_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("order_ID");
            entity.Property(e => e.BoxId)
                .HasDefaultValueSql("nextval('\"Orders_box_ID_seq\"'::regclass)")
                .HasColumnName("box_ID");
            entity.Property(e => e.CarwashId).HasColumnName("carwash_ID");
            entity.Property(e => e.DateTime).HasColumnName("date_time");
            entity.Property(e => e.LicencePlate)
                .HasMaxLength(255)
                .HasColumnName("licencePlate");
            entity.Property(e => e.StatusId).HasColumnName("status_ID");
            entity.Property(e => e.UserId).HasColumnName("user_ID");

            entity.HasOne(d => d.Box).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BoxId)
                .HasConstraintName("fk_box_ID");

            entity.HasOne(d => d.Carwash).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CarwashId)
                .HasConstraintName("fk_carwash_ID");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("fk_status_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_ID");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("OrderStatus_pkey");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.StatusId)
                .ValueGeneratedNever()
                .HasColumnName("status_ID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(100)
                .HasColumnName("statusName");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("Person_pkey");

            entity.ToTable("Person");

            entity.Property(e => e.PersonId)
                .ValueGeneratedNever()
                .HasColumnName("person_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FathersName)
                .HasMaxLength(100)
                .HasColumnName("fathersName");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("lastName");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .HasColumnName("phoneNumber");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Roles_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("Schedule_pkey");

            entity.ToTable("Schedule");

            entity.Property(e => e.ScheduleId)
                .ValueGeneratedNever()
                .HasColumnName("schedule_ID");
            entity.Property(e => e.CarwashId).HasColumnName("carwash_ID");
            entity.Property(e => e.DayId).HasColumnName("day_ID");

            entity.HasOne(d => d.Day).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.DayId)
                .HasConstraintName("fk_day_ID");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("Service_pkey");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("service_ID");
            entity.Property(e => e.CarwashId).HasColumnName("carwash_ID");
            entity.Property(e => e.Duration)
                .HasMaxLength(100)
                .HasColumnName("duration");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.StatusId).HasColumnName("status_ID");

            entity.HasOne(d => d.Carwash).WithMany(p => p.Services)
                .HasForeignKey(d => d.CarwashId)
                .HasConstraintName("fk_carwash_ID");

            entity.HasOne(d => d.Status).WithMany(p => p.Services)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("fk_status_ID");
        });

        modelBuilder.Entity<ServiceStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("ServiceStatus_pkey");

            entity.ToTable("ServiceStatus");

            entity.Property(e => e.StatusId)
                .ValueGeneratedNever()
                .HasColumnName("status_ID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(100)
                .HasColumnName("statusName");
        });

        modelBuilder.Entity<ServicesInOrder>(entity =>
        {
            entity.HasKey(e => e.ServicesInOrderId).HasName("ServicesInOrder_pkey");

            entity.ToTable("ServicesInOrder");

            entity.HasIndex(e => e.ServiceId, "pk_service_ID").IsUnique();

            entity.Property(e => e.ServicesInOrderId)
                .ValueGeneratedNever()
                .HasColumnName("servicesInOrder_ID");
            entity.Property(e => e.OrderId).HasColumnName("order_ID");
            entity.Property(e => e.ServiceId).HasColumnName("service_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.ServicesInOrders)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_order_ID");

            entity.HasOne(d => d.Service).WithOne(p => p.ServicesInOrder)
                .HasForeignKey<ServicesInOrder>(d => d.ServiceId)
                .HasConstraintName("fk_service_ID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_pkey");

            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_ID");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.PersonId).HasColumnName("person_ID");
            entity.Property(e => e.RoleId).HasColumnName("role_ID");

            entity.HasOne(d => d.Person).WithMany(p => p.Users)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("fk_person_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_role_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
