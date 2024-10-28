using Bogus.DataSets;
using Common.Enums;
using DataAcess.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
    protected override async void OnModelCreating(ModelBuilder modelBuilder)
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

        // one to many relationship for side to order
        modelBuilder.Entity<Side>()
            .HasMany<OrderPizza>()
            .WithOne()
            .HasForeignKey(e => e.OrderId)
            .IsRequired();

        // one to many relationship for OrderPizza to OrderPizzaToppings
        modelBuilder.Entity<OrderPizza>()
            .HasMany<OrderPizzaTopping>()
            .WithOne()
            .HasForeignKey(e => e.OrderPizzaId)
            .IsRequired();
        //TODO: Add wuanity to the cart table so we know how many of a certain menue item was ordered
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

        // seed  SuperAdmin data
        string ADMIN_ID = "02174cf0–9412–4cfe - afbf - 59f706d72cf6";
        //create user
        var appUser = new ApplicationUser
        {
            Id = ADMIN_ID,
            Email = "frankofoedu@gmail.com",
            EmailConfirmed = true,
            UserName = "frankofoedu@gmail.com",
            NormalizedUserName = "FRANKOFOEDU@GMAIL.COM",
            FullName = "Frank Ofoedu",
        };

        //set user password
        PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
        appUser.PasswordHash = ph.HashPassword(appUser, "mypassword_ ?");

        //seed user
        modelBuilder.Entity<ApplicationUser>().HasData(appUser);

        modelBuilder.Entity<IdentityUserClaim<string>>().HasData(
            new IdentityUserClaim<string>
            {
                ClaimType = "Role",
                ClaimValue = "Admin",
                Id = 1,
                UserId = appUser.Id
            },
            new IdentityUserClaim<string>
            {
                ClaimType = "Role",
                ClaimValue = "Customer",
                UserId = appUser.Id,
                Id = 2
            },
            new IdentityUserClaim<string>
            {
                ClaimType = "Role",
                ClaimValue = "Staff",
                UserId = appUser.Id,
                Id = 3
            }
            );

        modelBuilder.Entity<Pizza>()
        .HasData(
        new Pizza { Id = 1, Name = "Pepperoni", Price = 89, Description = "This is a peparoni Pizza", DateCreated = DateTime.UtcNow, Category = PizzaCategory.Meat, InStock = true },
        new Pizza { Id = 2, Name = "Meat", Price = 99, Description = "This is a meat Pizza", DateCreated = DateTime.UtcNow, Category = PizzaCategory.Meat, InStock = true },
        new Pizza { Id = 3, Name = "Margherita", Price = 79, Description = "This is a Margherita pizza", DateCreated = DateTime.UtcNow, Category = PizzaCategory.Vegiatarian, InStock = true },
        new Pizza { Id = 4, Name = "Hawaiian", Price = 89, Description = "This is a Hawaiin pizza", DateCreated = DateTime.UtcNow, Category = PizzaCategory.Meat, InStock = true },
        new Pizza { Id = 5, Name = "BBQ Chicken", Price = 120, Description = "This is a BBQ Chicken Pizza", DateCreated = DateTime.UtcNow, Category = PizzaCategory.Chiken, InStock = true },
        new Pizza { Id = 6, Name = "BBQ Bacon", Price = 99, Description = "This is a BBQ Bacon Pizza", DateCreated = DateTime.UtcNow, Category = PizzaCategory.Meat, InStock = true },
        new Pizza { Id = 7, Name = "Chicken And Mushroom", Price = 100, Description = "This is a Chicken and Mushroom Pizza", DateCreated = DateTime.UtcNow, Category = PizzaCategory.Chiken, InStock = true },
        new Pizza { Id = 8, Name = "Sweet Chilli Chicken", Price = 80, Description = "This is a Sweet Chilli Chicken Pizza", DateCreated = DateTime.UtcNow, Category = PizzaCategory.Chiken, InStock = true });


        modelBuilder.Entity<Side>()
            .HasData(new Side
            {
                ID = 1,
                Description = "A 500ml regular coke",
                InStock = true,
                Price = 15,
                Name = "Coke"
            },
            new Side
            {
                ID = 2,
                Description = "A 500ml regular Pepsi",
                InStock = true,
                Price = 15,
                Name = "Pepsi"
            },
            new Side
            {
                ID = 3,
                Description = "5 crispy chicken wings",
                InStock = true,
                Price = 55,
                Name = "5 Chicken Wings"
            },
            new Side
            {
                ID = 4,
                Description = "5 cheese sticks",
                InStock = true,
                Price = 24,
                Name = "5 cheese sticks"
            }
            );

        modelBuilder.Entity<Topping>().HasData(
            new Topping { Id = 1, PizzaId = 1, Price = 15, Name = "Extra Cheese", InStock = true, Additional = true, },
            new Topping { Id = 2, PizzaId = 1, Price = 27, Name = "Feta", InStock = true, Additional = true, },
            new Topping { Id = 3, PizzaId = 1, Price = 0, Name = "Olives", InStock = true, Additional = false, },
            new Topping { Id = 4, PizzaId = 1, Price = 0, Name = "Peporaini", InStock = true, Additional = false },
            new Topping { Id = 5, PizzaId = 2, Price = 23, Name = "Extra Cheese", InStock = true, Additional = true },
            new Topping { Id = 6, PizzaId = 5, Price = 0, Name = "No BBQ Sauce", InStock = true, Additional = false },
            new Topping { Id = 7, PizzaId = 6, Price = 24.90, Name = "Single Extra Mozzarella", InStock = true, Additional = true },
            new Topping { Id = 8, PizzaId = 6, Price = 49.80, Name = "Double Extra Mozzarella", InStock = true, Additional = true },
            new Topping { Id = 9, PizzaId = 6, Price = 0, Name = "No BBQ Sauce", InStock = true, Additional = false },
            new Topping { Id = 10, PizzaId = 8, Price = 24.90, Name = "Single Extra Mozzarella", InStock = true, Additional = true });
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(databaseName: "PezzaDb");
}