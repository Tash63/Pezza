using Common.Enums;
using DataAcess.Mapping;

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

    public virtual DbSet<Side> Sides { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Topping> Toppings { get; set; }

    public virtual DbSet<OrderPizza> OrderPizzas { get; set; }

    public virtual DbSet<OrderPizzaTopping> OrderPizzaToppings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerMap());
        modelBuilder.ApplyConfiguration(new PizzaMap());
        modelBuilder.ApplyConfiguration(new NotifyMap());
        modelBuilder.ApplyConfiguration(new OrderMap());
        modelBuilder.ApplyConfiguration(new SideMap());
        modelBuilder.ApplyConfiguration(new ToppingMap());
        modelBuilder.ApplyConfiguration(new OrderPizzaMap());
        modelBuilder.ApplyConfiguration(new OrderPizzaToppingMap());

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
            .HasForeignKey(e=>e.OrderPizzaId)
            .IsRequired();

        // one to many relationship for Toppings to OrderPizzaToppings
        modelBuilder.Entity<Topping>()
            .HasMany<OrderPizzaTopping>()
            .WithOne()
            .HasForeignKey(e=>e.ToppingId)
            .IsRequired();

        // Seed database with intial data that will be used for testing
        modelBuilder.Entity<Pizza>()
        .HasData(
        new Pizza { Id = 1, Name = "Pepperoni Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow,Category=PizzaCategory.Meat,InStock=true },
        new Pizza { Id = 2, Name = "Meat Pizza", Price = 99, Description = string.Empty, DateCreated = DateTime.UtcNow,Category=PizzaCategory.Meat,InStock=true },
        new Pizza { Id = 3, Name = "Margherita Pizza", Price = 79, Description = string.Empty, DateCreated = DateTime.UtcNow,Category = PizzaCategory.Vegiatarian, InStock = true },
        new Pizza { Id = 4, Name = "Hawaiian Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow ,Category = PizzaCategory.Meat, InStock = true });
        Customer customer = new Customer()
        {
            Id = 1,
            Name = "Kiran Tash Nariansamy",
            Address = "3 plane crescent",
            Cellphone = "065 979 8511",
            DateCreated = DateTime.UtcNow,
            Email = "kirannariansamy1967@gmail.com"
        };
        modelBuilder.Entity<Customer>().HasData(customer);

        modelBuilder.Entity<Side>()
            .HasData( new Side
            {
                ID = 1,
                Description="A regular coke",
                InStock = true,
                Name="Coca Cola",
                Price=15,
            }
            );

        modelBuilder.Entity<Topping>().HasData(
            new Topping{ Id=1,PizzaId=1,Price = 15,Name="Extra Cheese",InStock=true,Additional=true,},
            new Topping{Id = 2, PizzaId = 2, Price=23,Name="Extra Cheese",InStock=true});
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(databaseName: "PezzaDb");
}