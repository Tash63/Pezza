﻿using Common.Enums;
using DataAcess.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccess;

public class DatabaseContext : IdentityDbContext<ApplicationUser>
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Pizza> Pizzas { get; set; }

    public virtual DbSet<Notify> Notifies { get; set; }

    public virtual DbSet<Side> Sides { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Topping> Toppings { get; set; }

    public virtual DbSet<OrderPizza> OrderPizzas { get; set; }

    public virtual DbSet<OrderPizzaTopping> OrderPizzaToppings { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<CartTopping> CartToppings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PizzaMap());
        modelBuilder.ApplyConfiguration(new NotifyMap());
        modelBuilder.ApplyConfiguration(new OrderMap());
        modelBuilder.ApplyConfiguration(new SideMap());
        modelBuilder.ApplyConfiguration(new ToppingMap());
        modelBuilder.ApplyConfiguration(new OrderPizzaMap());
        modelBuilder.ApplyConfiguration(new OrderPizzaToppingMap());
        modelBuilder.ApplyConfiguration(new CartMap());
        modelBuilder.ApplyConfiguration(new CartToppingMap());

        // Configure relationships between tables

        // Many to one relationship for pizza
        modelBuilder.Entity<Pizza>()
            .HasMany<Topping>()
            .WithOne()
            .HasForeignKey(e => e.PizzaId)
            .IsRequired();

        // one to many relationship for order to order pizza
        modelBuilder.Entity<Order>()
            .HasMany<OrderPizza>()
            .WithOne()
            .HasForeignKey(e => e.OrderId)
            .IsRequired();

        // one to many relationship for pizza to order
        modelBuilder.Entity<Pizza>()
            .HasMany<OrderPizza>()
            .WithOne()
            .HasForeignKey(e => e.PizzaId)
            .IsRequired();

        // one to many relationship for OrderPizza to OrderPizzaToppings
        modelBuilder.Entity<OrderPizza>()
            .HasMany<OrderPizzaTopping>()
            .WithOne()
            .HasForeignKey(e => e.OrderPizzaId)
            .IsRequired();

        // one to many relationship for Toppings to OrderPizzaToppings
        modelBuilder.Entity<Topping>()
            .HasMany<OrderPizzaTopping>()
            .WithOne()
            .HasForeignKey(e => e.ToppingId)
            .IsRequired();

        // one to many relationship for ApplicationUser to cart
        modelBuilder.Entity<ApplicationUser>()
         .HasMany<Cart>()
         .WithOne()
         .HasForeignKey(e => e.UserEmail)
         .IsRequired();

        // one to many realtionship for side to cart
        modelBuilder.Entity<Side>()
            .HasMany<Cart>()
            .WithOne()
            .HasForeignKey(e => e.SideID);

        // one to many relationship for pizza to cart
        modelBuilder.Entity<Pizza>()
            .HasMany<Cart>()
            .WithOne()
            .HasForeignKey(e => e.PizzaID);

        // one to many relationship for cart to cart topping
        modelBuilder.Entity<Cart>()
            .HasMany<CartTopping>()
            .WithOne()
            .HasForeignKey(e => e.CartID)
            .IsRequired();

        // one to many relationship for topping to cart topping
        modelBuilder.Entity<Topping>()
            .HasMany<CartTopping>()
            .WithOne()
            .HasForeignKey(e => e.ToppingId)
            .IsRequired();

        // one to many relationship for User to noitify
        modelBuilder.Entity<ApplicationUser>()
            .HasMany<Notify>()
            .WithOne()
            .HasForeignKey(e => e.UserEmail);

        // Seed database with intial data that will be used for testing
        modelBuilder.Entity<Pizza>()
        .HasData(
        new Pizza { Id = 1, Name = "Pepperoni Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow, Category = PizzaCategory.Chiken, InStock = true },
        new Pizza { Id = 2, Name = "Meat Pizza", Price = 99, Description = string.Empty, DateCreated = DateTime.UtcNow, Category = PizzaCategory.Meat, InStock = true },
        new Pizza { Id = 3, Name = "Margherita Pizza", Price = 79, Description = string.Empty, DateCreated = DateTime.UtcNow, Category = PizzaCategory.Vegiatarian, InStock = true },
        new Pizza { Id = 4, Name = "Hawaiian Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow, Category = PizzaCategory.Meat, InStock = true });


        modelBuilder.Entity<Side>()
            .HasData(new Side
            {
                ID = 1,
                Description = "A regular coke",
                InStock = true,
                Name = "Coca Cola",
                Price = 15,
            }
            );

        modelBuilder.Entity<Topping>().HasData(
            new Topping { Id = 1, PizzaId = 1, Price = 15, Name = "Extra Cheese", InStock = true, Additional = true, },
            new Topping { Id = 2, PizzaId = 2, Price = 23, Name = "Extra Cheese", InStock = true });
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(databaseName: "PezzaDb");
}