﻿using Common.Models.Order;

namespace DataAccess;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Pizza> Pizzas { get; set; }

    public virtual DbSet<Notify> Notifies { get; set; }

    public virtual DbSet<CreateOrderModel> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerMap());
        modelBuilder.ApplyConfiguration(new PizzaMap());
        modelBuilder.ApplyConfiguration(new NotifyMap());
        modelBuilder.ApplyConfiguration(new OrderMap());
        modelBuilder.Entity<Pizza>()
        .HasData(
        new Pizza { Id = 1, Name = "Pepperoni Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow },
        new Pizza { Id = 2, Name = "Meat Pizza", Price = 99, Description = string.Empty, DateCreated = DateTime.UtcNow },
        new Pizza { Id = 3, Name = "Margherita Pizza", Price = 79, Description = string.Empty, DateCreated = DateTime.UtcNow },
        new Pizza { Id = 4, Name = "Hawaiian Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow });
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "Kiran Tash Narianamy", Address = "3 plane crescent waterways", Cellphone = "0659798511", DateCreated = DateTime.UtcNow, Email = "kirannariansamy1967@gmail.com" }
            );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(databaseName: "PezzaDb");
}